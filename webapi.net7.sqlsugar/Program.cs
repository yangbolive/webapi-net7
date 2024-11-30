
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using webapi.net7.sqlsugar;
using WatchDog;
using System.Reflection;
using Microsoft.Extensions.Options;
using SqlSugar;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace webapi.net7.sqlsugar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //注入Http
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            #region
            // 添加JWT token验证
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:SignKey"))),//JWT秘钥
                    ValidIssuer = builder.Configuration.GetValue<string>("JWT:ISyouuser"),//发行者,
                    ValidAudience = builder.Configuration.GetValue<string>("JWT:IsAudience"),//接收者,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime = true,
                    //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.Zero
                };
            });
            //添加Swagger的Authiozer的按钮鉴权
            builder.Services.AddSwaggerGen(s =>
            {
                #region 实现多版本
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    s.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = "温岭中和接口平台",
                        Version = version.ToString(),
                        Description = $"{version}WebApi接口"

                    });
                });

                #endregion
                //s.SwaggerDoc("v1", new OpenApiInfo { Title = "中和软件接口平台", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                s.IncludeXmlComments(xmlPath, true);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"}
                           },new string[] { }
                        }
                    });

                #region 扩展文件上传按钮
                {
                    s.OperationFilter<FileUploadfilter>();
                }
                #endregion

            });
            #endregion
            //下列注释代码为统一返回格式json
            //builder.Services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(ActionFilter));
            //});

            //builder.Services.AddSwaggerExt();
            #region 解决跨域问题
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
            #endregion
            #region  sqlsugar注入IOC
            //注册上下文：AOP里面可以获取IOC对象，如果有现成框架比如Furion可以不写这一行
            builder.Services.AddHttpContextAccessor();

            //注册SqlSugar
            builder.Services.AddSingleton<ISqlSugarClient>(s =>
            {
                //单例不要写在泛型类中， 类<T>.Db 这种是无效单例 ，T不同就会有多个实例
                SqlSugarScope Db = new SqlSugarScope(new List<ConnectionConfig>() {

                new ConnectionConfig(){ConfigId="0",DbType=DbType.SqlServer,ConnectionString="Data Source=192.168.0.76;Initial Catalog=ufdata_777_2018;Persist Security Info=True;User ID=sa;Password=Adef1234",IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="1",DbType=DbType.SqlServer,ConnectionString="Data Source=192.168.0.76;Initial Catalog=ufsystem;Persist Security Info=True;User ID=sa;Password=Adef1234",IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="2",DbType=DbType.SqlServer,ConnectionString=JsonHelper.GetAppSettings("Connectionstrings_sql:DBConnection"),IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="3",DbType=DbType.SqlServer,ConnectionString=JsonHelper.GetAppSettings("Connectionstrings_ufsystem_sql:DBConnection"),IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="4",DbType=DbType.SqlServer,ConnectionString=JsonHelper.GetAppSettings("Connectionstrings_bwsql:DBConnection"),IsAutoCloseConnection = true },
                },

                 //ConfigId用来区别是哪个库
 
                 db =>
                 {
                     //如果是GetConnectionScope或者GetConnectionScopeWithAttr这边也应该是GetConnectionScope
                     //如果用的是GetConnection或者GetConnectionWithAttr这边也应该是GetConnection
                     //用哪个就AOP添加哪个
                     //技巧：这边可以循环处理这个更方便些
                     db.GetConnection("0").Aop.OnLogExecuting = (sql, p) =>
                     {
                         Console.WriteLine(sql);
                     };
                     db.GetConnection("1").Aop.OnLogExecuting = (sql, p) =>
                     {
                         Console.WriteLine(sql);
                     };
                     db.GetConnection("2").Aop.OnLogExecuting = (sql, p) =>
                     {
                         Console.WriteLine(sql);
                     };
                     db.GetConnection("3").Aop.OnLogExecuting = (sql, p) =>
                     {
                         Console.WriteLine(sql);
                     };
                     db.GetConnection("4").Aop.OnLogExecuting = (sql, p) =>
                     {
                         Console.WriteLine(sql);
                     };


                     //获取IOC对象不要求在一个上下文
                     //vra log=s.GetService<Log>()

                     //获取IOC对象要求在一个上下文
                     //var appServive = s.GetService<IHttpContextAccessor>();
                     //var log= appServive?.HttpContext?.RequestServices.GetService<Log>();
                 });
                
                return Db;
            });
            #endregion

            #region Nlog日志引入
            {
                LogManager.Setup().LoadConfigurationFromFile("ConfigFile/NLog.config").GetCurrentClassLogger();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                builder.Host.UseNLog();
            }
            #endregion

            //看门狗注册
            builder.Services.AddWatchDogServices();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())   //如果是测试进入swagger 否则不进入，正式环境部署发布必须注释此IF
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        foreach (string version in typeof(ApiVersions).GetEnumNames())
            //        {
            //            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"U8 OPENAPI【{version}】版本");
            //        }
            //    });
            //    #region 使用Swagger
            //    //app.UseSwagger();
            //    //app.UseSwaggerUI(c =>
            //    //{
            //    //    foreach(string version in typeof(ApiVersions).GetEnumNames())
            //    //    {
            //    //        c.SwaggerEndpoint($"/swagger/{version}/swagger.json",$"温岭中和接口平台第【{version}】版本");
            //    //    }
            //    //}
            //    //    );
            //    #endregion
            //    app.UseSwaggerExt();
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (string version in typeof(ApiVersions).GetEnumNames())
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"【{version}】接口");
                }
            });
            #region 使用Swagger
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    foreach(string version in typeof(ApiVersions).GetEnumNames())
            //    {
            //        c.SwaggerEndpoint($"/swagger/{version}/swagger.json",$"温岭中和接口平台第【{version}】版本");
            //    }
            //}
            //    );
            #endregion
            app.UseSwaggerExt();

            app.UseHttpsRedirection();
            app.UseAuthentication();// 认证中间件
            app.UseAuthorization();//授权中间件

            //配置Cors跨域问题
            app.UseCors("any");
            app.MapControllers();
            #region 看门狗代码

            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = "admin";
                opt.WatchPagePassword = "@wlzh44338";

            });
            #endregion
            app.Run();
        }
    }
}
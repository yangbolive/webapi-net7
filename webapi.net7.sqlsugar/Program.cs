
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
            //ע��Http
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            #region
            // ���JWT token��֤
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:SignKey"))),//JWT��Կ
                    ValidIssuer = builder.Configuration.GetValue<string>("JWT:ISyouuser"),//������,
                    ValidAudience = builder.Configuration.GetValue<string>("JWT:IsAudience"),//������,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                    ValidateLifetime = true,
                    //ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
                    ClockSkew = TimeSpan.Zero
                };
            });
            //���Swagger��Authiozer�İ�ť��Ȩ
            builder.Services.AddSwaggerGen(s =>
            {
                #region ʵ�ֶ�汾
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    s.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = "�����кͽӿ�ƽ̨",
                        Version = version.ToString(),
                        Description = $"{version}WebApi�ӿ�"

                    });
                });

                #endregion
                //s.SwaggerDoc("v1", new OpenApiInfo { Title = "�к�����ӿ�ƽ̨", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                s.IncludeXmlComments(xmlPath, true);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
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

                #region ��չ�ļ��ϴ���ť
                {
                    s.OperationFilter<FileUploadfilter>();
                }
                #endregion

            });
            #endregion
            //����ע�ʹ���Ϊͳһ���ظ�ʽjson
            //builder.Services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(ActionFilter));
            //});

            //builder.Services.AddSwaggerExt();
            #region �����������
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
            #endregion
            #region  sqlsugarע��IOC
            //ע�������ģ�AOP������Ի�ȡIOC����������ֳɿ�ܱ���Furion���Բ�д��һ��
            builder.Services.AddHttpContextAccessor();

            //ע��SqlSugar
            builder.Services.AddSingleton<ISqlSugarClient>(s =>
            {
                //������Ҫд�ڷ������У� ��<T>.Db ��������Ч���� ��T��ͬ�ͻ��ж��ʵ��
                SqlSugarScope Db = new SqlSugarScope(new List<ConnectionConfig>() {

                new ConnectionConfig(){ConfigId="0",DbType=DbType.SqlServer,ConnectionString="Data Source=192.168.0.76;Initial Catalog=ufdata_777_2018;Persist Security Info=True;User ID=sa;Password=Adef1234",IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="1",DbType=DbType.SqlServer,ConnectionString="Data Source=192.168.0.76;Initial Catalog=ufsystem;Persist Security Info=True;User ID=sa;Password=Adef1234",IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="2",DbType=DbType.SqlServer,ConnectionString=JsonHelper.GetAppSettings("Connectionstrings_sql:DBConnection"),IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="3",DbType=DbType.SqlServer,ConnectionString=JsonHelper.GetAppSettings("Connectionstrings_ufsystem_sql:DBConnection"),IsAutoCloseConnection = true },
                new ConnectionConfig(){ConfigId="4",DbType=DbType.SqlServer,ConnectionString=JsonHelper.GetAppSettings("Connectionstrings_bwsql:DBConnection"),IsAutoCloseConnection = true },
                },

                 //ConfigId�����������ĸ���
 
                 db =>
                 {
                     //�����GetConnectionScope����GetConnectionScopeWithAttr���ҲӦ����GetConnectionScope
                     //����õ���GetConnection����GetConnectionWithAttr���ҲӦ����GetConnection
                     //���ĸ���AOP����ĸ�
                     //���ɣ���߿���ѭ���������������Щ
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


                     //��ȡIOC����Ҫ����һ��������
                     //vra log=s.GetService<Log>()

                     //��ȡIOC����Ҫ����һ��������
                     //var appServive = s.GetService<IHttpContextAccessor>();
                     //var log= appServive?.HttpContext?.RequestServices.GetService<Log>();
                 });
                
                return Db;
            });
            #endregion

            #region Nlog��־����
            {
                LogManager.Setup().LoadConfigurationFromFile("ConfigFile/NLog.config").GetCurrentClassLogger();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                builder.Host.UseNLog();
            }
            #endregion

            //���Ź�ע��
            builder.Services.AddWatchDogServices();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())   //����ǲ��Խ���swagger ���򲻽��룬��ʽ�������𷢲�����ע�ʹ�IF
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        foreach (string version in typeof(ApiVersions).GetEnumNames())
            //        {
            //            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"U8 OPENAPI��{version}���汾");
            //        }
            //    });
            //    #region ʹ��Swagger
            //    //app.UseSwagger();
            //    //app.UseSwaggerUI(c =>
            //    //{
            //    //    foreach(string version in typeof(ApiVersions).GetEnumNames())
            //    //    {
            //    //        c.SwaggerEndpoint($"/swagger/{version}/swagger.json",$"�����кͽӿ�ƽ̨�ڡ�{version}���汾");
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
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"��{version}���ӿ�");
                }
            });
            #region ʹ��Swagger
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    foreach(string version in typeof(ApiVersions).GetEnumNames())
            //    {
            //        c.SwaggerEndpoint($"/swagger/{version}/swagger.json",$"�����кͽӿ�ƽ̨�ڡ�{version}���汾");
            //    }
            //}
            //    );
            #endregion
            app.UseSwaggerExt();

            app.UseHttpsRedirection();
            app.UseAuthentication();// ��֤�м��
            app.UseAuthorization();//��Ȩ�м��

            //����Cors��������
            app.UseCors("any");
            app.MapControllers();
            #region ���Ź�����

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
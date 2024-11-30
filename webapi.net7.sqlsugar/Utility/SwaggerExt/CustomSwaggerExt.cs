using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;
using WatchDog;
using WatchDog.src.Enums;

namespace webapi.net7.sqlsugar
{
    /// <summary>
    /// program中的swagger配置迁移到此处
    /// </summary>
    public static class CustomSwaggerExt
    {
        public static void AddSwaggerExt(this IServiceCollection services)
        {
            #region Swagger的配置
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                #region 实现多版本
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    options.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = "温岭中和Api文档",
                        Version = version.ToString(),
                        Description = $"通用版本的Api版本{version}"

                    });
                });
                #endregion
                #region 支持注释
                //xml文档绝对路径---堆区根据控制器api生成的xml文件
                var file = Path.Combine(AppContext.BaseDirectory, "webapi.net7.sqlsugar.xml");
                //true:显示控制器层注释
                options.IncludeXmlComments(file, true);
                //对action的名称进行排序,如果有多个，就可以看见效果了
                options.OrderActionsBy(o => o.RelativePath);
                #endregion
            });
            #endregion

        }

        public static void UseSwaggerExt(this WebApplication app)
        {
           
            #region 使用Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (string version in typeof(ApiVersions).GetEnumNames())
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"用友接口平台第【{version}】版本");
                }
            }
                );

            #endregion
        }
    }
}

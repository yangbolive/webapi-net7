using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace webapi.net7.sqlsugar
{
    /// <summary>
    /// 文件上传webapi功能类
    /// </summary>
    public class FileUploadfilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            const string FileUploadContentType = "multipart/form-data";
            if(operation.RequestBody==null||!operation.RequestBody.Content.Any(x=>x.Key.Equals(FileUploadContentType,StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }
            if (context.ApiDescription.ParameterDescriptions[0].Type==typeof(IFormCollection))
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Description = "文件上传",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                          FileUploadContentType,new OpenApiMediaType
                          {
                              Schema=new OpenApiSchema
                              {
                                  Type="object",
                                  Required=new HashSet<string>{"file"},
                                  Properties=new Dictionary<string, OpenApiSchema>
                                  {
                                      {
                                          "file",new OpenApiSchema()
                                          {
                                              Type="string",
                                              Format="binary"
                                          }
                                      }
                                  }
                              }

                          }

                        }

                    }
                };
            }
        }
    }
}

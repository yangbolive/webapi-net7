using DnsClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using Ubiety.Dns.Core;
using webapi.net7.sqlsugar;
using webapi.net7.sqlsugar.Model;
using Newtonsoft.Json; // 引入命名空间
using System.Data.SqlClient;
using SqlSugar; // 导入System.Data.SqlClient命名空间（如果需要连接到数据库）


namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// 文件上传下载接口
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名称,action则按方法名显示
                             //[Authorize] //token生效
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.文件上传接口))]

    public class FileController : ControllerBase
    {
        //获取数据库连接
        public static string conset = JsonHelper.GetAppSettings("Connectionstrings_sql:DBConnection");
        //声明获取客户端IP注入变量
        private readonly IHttpContextAccessor httpContextAccessor;
        //依赖注入IWebHostEnvironment
        private static IWebHostEnvironment? _webHostingEnvironment;

        public readonly ISqlSugarClient DBdata;
        public readonly ISqlSugarClient DBsystem;
        public readonly ISqlSugarClient DBsql;
        public readonly ISqlSugarClient DBufsystemsql;
        //创建日志记录对象
        public readonly ILogger<WorkShopController> _Logger;
        public FileController(ISqlSugarClient dbsystem, ILogger<WorkShopController> Logger,IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostingEnvironment = hostingEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this._Logger = Logger;
            var db = dbsystem as SqlSugarScope;

            this.DBdata = db.GetConnection("0"); //获取第一个数据库
            this.DBsystem = db.GetConnection("1"); //获取第二个数据库
            this.DBsql = db.GetConnection("2"); //获取大众精密账套数据库
            this.DBufsystemsql = db.GetConnection("3"); //获取大众精密ufsystem数据库
        }

        /// <summary>
        /// 单文件上传接口
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ccode"></param>
        /// <param name="vouchType"></param>
        /// <param name="autoid"></param>
        /// <param name="maker"></param>
        /// <returns></returns>
        [HttpPost(Name = "File")]
        public JsonResult UploadFile(IFormCollection form, string vouchType, string ccode,string autoid,string maker)
        {
            
            //获取GID
            string uuid=Guid.NewGuid().ToString().Replace("-","");
            //处理接收的文件
            var formFile = form.Files[0];
            var currentDate = DateTime.Now;
            var webRootPath = _webHostingEnvironment.WebRootPath;//>>>相当于HttpContext.Current.Server.MapPath("") 

            try
            {
                var filePath = $"/UploadFile/{currentDate:yyyyMMdd}/";

                //创建每日存储文件夹
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }

                if (formFile != null)
                {
                    //文件后缀
                    var fileExtension = Path.GetExtension(formFile.FileName);//获取文件格式，拓展名

                    //判断文件大小
                    var fileSize = formFile.Length;

                    if (fileSize > 1024 * 1024 * 20) //20M TODO:(1mb=1024X1024b)
                    {
                        return new JsonResult(new { isSuccess = false, resultMsg = "上传的文件不能大于20M" });
                    }
                    //Guid.NewGuid().ToString() 
                    //保存的文件名称(以名称和保存时间命名)
                    //var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;
                    //获取原文件名称
                    var filename = formFile.FileName.ToString();
                    //写入uuid作为前台存放的文件命
                    var saveName = uuid+ fileExtension;
                    

                    //文件保存
                    using (var fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    //完整的文件路径
                    var completeFilePath = Path.Combine(filePath, saveName);
                    
                    //var ip = HttpContext.GetClientUserIp();  //直接通过调用扩展方法实现
                    string ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); //通过注入获取客户端IP
                    //后台插入上传文件的相关信息
                    StringBuilder sb=new StringBuilder();
                    sb.Append($"insert into wlzh_Dz_UploadFile(gid,fileName,filepath,vouchType,ccode,autoid,maker,maketime,ip,dataName)\r\nvalues('{uuid}','{filename}','{completeFilePath}','{vouchType}','{ccode}','{autoid}','{maker}',getdate(),'{ip}','{saveName}')");
                    DBsql.Ado.ExecuteCommand(sb.ToString());
                    return new JsonResult(new
                    {
                        Success = true,
                        Message = "上传成功",
                        FileName = saveName,
                        completeFilePath=completeFilePath,
                        gid=uuid,
                        Ip =ip
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        Success = false,
                        Message = "上传失败，未检测上传的文件信息~",
                        FileName = form.Files.FirstOrDefault()?.FileName,
                        completeFilePath = "",
                        gid = "",
                        Ip = ""
                    });

                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Success = false,
                    Message = "文件保存失败，异常信息为：" + ex.Message,
                    FileName = form.Files.FirstOrDefault()?.FileName,
                    completeFilePath = "",
                    gid = "",
                    Ip = ""
                });

            }



        }


    }
}
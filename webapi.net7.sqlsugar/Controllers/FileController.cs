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
using Newtonsoft.Json; // ���������ռ�
using System.Data.SqlClient;
using SqlSugar; // ����System.Data.SqlClient�����ռ䣨�����Ҫ���ӵ����ݿ⣩


namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// �ļ��ϴ����ؽӿ�
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //·������action�޸�Ϊcontroller��Ĭ��·������,action�򰴷�������ʾ
                             //[Authorize] //token��Ч
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.�ļ��ϴ��ӿ�))]

    public class FileController : ControllerBase
    {
        //��ȡ���ݿ�����
        public static string conset = JsonHelper.GetAppSettings("Connectionstrings_sql:DBConnection");
        //������ȡ�ͻ���IPע�����
        private readonly IHttpContextAccessor httpContextAccessor;
        //����ע��IWebHostEnvironment
        private static IWebHostEnvironment? _webHostingEnvironment;

        public readonly ISqlSugarClient DBdata;
        public readonly ISqlSugarClient DBsystem;
        public readonly ISqlSugarClient DBsql;
        public readonly ISqlSugarClient DBufsystemsql;
        //������־��¼����
        public readonly ILogger<WorkShopController> _Logger;
        public FileController(ISqlSugarClient dbsystem, ILogger<WorkShopController> Logger,IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostingEnvironment = hostingEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this._Logger = Logger;
            var db = dbsystem as SqlSugarScope;

            this.DBdata = db.GetConnection("0"); //��ȡ��һ�����ݿ�
            this.DBsystem = db.GetConnection("1"); //��ȡ�ڶ������ݿ�
            this.DBsql = db.GetConnection("2"); //��ȡ���ھ����������ݿ�
            this.DBufsystemsql = db.GetConnection("3"); //��ȡ���ھ���ufsystem���ݿ�
        }

        /// <summary>
        /// ���ļ��ϴ��ӿ�
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
            
            //��ȡGID
            string uuid=Guid.NewGuid().ToString().Replace("-","");
            //������յ��ļ�
            var formFile = form.Files[0];
            var currentDate = DateTime.Now;
            var webRootPath = _webHostingEnvironment.WebRootPath;//>>>�൱��HttpContext.Current.Server.MapPath("") 

            try
            {
                var filePath = $"/UploadFile/{currentDate:yyyyMMdd}/";

                //����ÿ�մ洢�ļ���
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }

                if (formFile != null)
                {
                    //�ļ���׺
                    var fileExtension = Path.GetExtension(formFile.FileName);//��ȡ�ļ���ʽ����չ��

                    //�ж��ļ���С
                    var fileSize = formFile.Length;

                    if (fileSize > 1024 * 1024 * 20) //20M TODO:(1mb=1024X1024b)
                    {
                        return new JsonResult(new { isSuccess = false, resultMsg = "�ϴ����ļ����ܴ���20M" });
                    }
                    //Guid.NewGuid().ToString() 
                    //������ļ�����(�����ƺͱ���ʱ������)
                    //var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;
                    //��ȡԭ�ļ�����
                    var filename = formFile.FileName.ToString();
                    //д��uuid��Ϊǰ̨��ŵ��ļ���
                    var saveName = uuid+ fileExtension;
                    

                    //�ļ�����
                    using (var fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    //�������ļ�·��
                    var completeFilePath = Path.Combine(filePath, saveName);
                    
                    //var ip = HttpContext.GetClientUserIp();  //ֱ��ͨ��������չ����ʵ��
                    string ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); //ͨ��ע���ȡ�ͻ���IP
                    //��̨�����ϴ��ļ��������Ϣ
                    StringBuilder sb=new StringBuilder();
                    sb.Append($"insert into wlzh_Dz_UploadFile(gid,fileName,filepath,vouchType,ccode,autoid,maker,maketime,ip,dataName)\r\nvalues('{uuid}','{filename}','{completeFilePath}','{vouchType}','{ccode}','{autoid}','{maker}',getdate(),'{ip}','{saveName}')");
                    DBsql.Ado.ExecuteCommand(sb.ToString());
                    return new JsonResult(new
                    {
                        Success = true,
                        Message = "�ϴ��ɹ�",
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
                        Message = "�ϴ�ʧ�ܣ�δ����ϴ����ļ���Ϣ~",
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
                    Message = "�ļ�����ʧ�ܣ��쳣��ϢΪ��" + ex.Message,
                    FileName = form.Files.FirstOrDefault()?.FileName,
                    completeFilePath = "",
                    gid = "",
                    Ip = ""
                });

            }



        }


    }
}
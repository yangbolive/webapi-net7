using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509;
using ServiceReference1;
using SqlSugar;
using System.Data.Common;
using System.ServiceModel;
using System.Web.Services.Description;
using System.Xml.Linq;
using webapi.net7.sqlsugar.Model;
using U8Login;

namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// 生产管理
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名
    [Tenant("1")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8移动端接口))]
    public class WorkShopController : Controller
    {
        
        public readonly ISqlSugarClient DBdata;
        public readonly ISqlSugarClient DBsystem;
        //创建日志记录对象
        public readonly ILogger<WorkShopController> _Logger;
        public WorkShopController(ISqlSugarClient dbsystem,ILogger<WorkShopController> Logger)
        {
            this._Logger = Logger;
            var db = dbsystem as SqlSugarScope;
             this.DBdata= db.GetConnection("0"); //获取第一个数据库
             this.DBsystem = db.GetConnection("1"); //获取第二个数据库
 

        }


        /// <summary>
        /// 扫码获取生产订单信息
        /// </summary>
        /// <param name="code">生产订单行条码</param>
        /// <returns></returns>
        [HttpPost]
        public object ProductionBarcode(object ob)
        {  
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? code = ((dynamic)obj).code ?? "";
                //List<ua_account_v> list = DBdata.Queryable<ua_account_v>().ToList();
                //return list;
                wlzh_v_Production wl = new wlzh_v_Production();
                List<wlzh_v_Production> list = SqlSugarHelp.SqlSugarListProduction(wl, code);
                if (list.Count <= 0)
                {
                    return new { errcode = "1", errmesg = "无对应记录", data = "", success = false };
                }
                else
                {
                    return new { errcode = "0", errmesg = "", data = list, success = true };
                }
            }
            catch (Exception ex) { return new { errcode = "1", errmesg = ex.ToString(), data = "", success = false }; }
        }
        /// <summary>
        /// U8审批流提交接口
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitApplicationMessage(object ob)
        {
            //动态获取对象
            JObject obj = JObject.Parse(ob.ToString());
            string? sAccID = "(default)@"+JsonHelper.GetAppSettings("U8WebService:account"); // 账套号(default)@101
            string? sUserID = ((dynamic)obj).sUserID ?? ""; //用户ID demo
            string? sPassword = ((dynamic)obj).sPassword ?? ""; //用户密码 DEMO
            string? sDate = DateTime.Now.ToString("yyyy-MM-dd");//登录日期 2021-05-29
            string? sServer = JsonHelper.GetAppSettings("U8WebService:server");//服务器地址 192.168.3.86
            string? ID= ((dynamic)obj).ID ?? "";//材料出库单唯一标识ID
            string? mesg = "";
            int? Code = 0;
            //U8 登录信息
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSubId = "DP";  //模块
            String sYear = DateTime.Now.Year.ToString(); //获取登录年份
            String sSerial = "";
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                mesg = "登陆失败，原因：" + u8Login.ShareString;

            }
            else
            {
                ServiceReference1.EmailServiceSoapClient semt = new ServiceReference1.EmailServiceSoapClient(ServiceReference1.EmailServiceSoapClient.EndpointConfiguration.EmailServiceSoap);
                mesg = semt.SubmitApplicationMessage("0411", ID, u8Login.userToken);
                Code = 1;
            }
            return new { Code, mesg };
  ;
        }
        ///// <summary>
        ///// 扫码获取生产订单信息
        ///// </summary>
        ///// <param name="code">生产订单行条码</param>
        ///// <returns></returns>
        //[HttpPost]
        //public object ProductionBarcode_system(string code = "")
        //{
        //    try
        //    {
        //        //List<ua_account_v> list = DBsystem.Queryable<ua_account_v>().ToList();
        //        //return list;
        //        wlzh_v_Production wl = new wlzh_v_Production();
        //        List<wlzh_v_Production> list = SqlSugarHelp.SqlSugarListProduction(wl, code);
        //        if (list.Count <= 0)
        //        {
        //            return new { errcode = "1", errmesg = "无对应记录", data = "", success = false };
        //        }
        //        else
        //        {
        //            return new { errcode = "0", errmesg = "", data = list, success = true };
        //        }
        //    }
        //    catch (Exception ex) { return new { errcode = "1", errmesg = ex.ToString(), data = "", success = false }; }
        //}
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using webapi.net7.sqlsugar.Model;
using Newtonsoft.Json; // 引入命名空间
using Newtonsoft.Json.Linq;

namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// 质量管理
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8移动端接口))]
    public class QualityControlController : Controller
    {
       
        /// <summary>
        /// 根据条码生成产品报检单列表数据
        /// </summary>
        /// <param name="ob">生产订单行条码</param>
        /// <returns></returns>
        [HttpPost]
        public object ProductInspectionSelect(object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? code = ((dynamic)obj).tm;
                wlzh_v_ProductInspectionList wl = new wlzh_v_ProductInspectionList();
                List<wlzh_v_ProductInspectionList> list = SqlSugarHelp.SqlSugarListProductInspectionList(wl, code);
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
        /// 删除产品报检单列表数据
        /// </summary>
        /// <param name="ob">报检单号</param>
        /// <returns></returns>
        [HttpPost]
        public object ProductInspectionDelete(object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? cCode = ((dynamic)obj).ccode;
                DataTable? dt = SqlSugarHelp.SqlSugarTableFL($"exec wlzh_p_deletebjd '{cCode}'");
                if (dt.Rows.Count <= 0)
                {
                    return new { state = 0, mesg = "删除失败", success = false };
                }
                else
                {
                    string? qx = dt.Rows[0][0].ToString();
                    string? ms = dt.Rows[0][1].ToString();
                    return new { state = qx == "1" ? 1 : 0, mesg = ms, success = qx == "1" ? true : false };
                }
            }
            catch (Exception e)
            {
                return new { state = 0, mesg = e.ToString(), success = false };
            }

        }

        /// <summary>
        /// 生产产品报检单
        /// </summary>
        /// <param name="obj">传入json对象</param>

        /// <returns></returns>
        [HttpPost]
        public object ProductInspectionAdd(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? table = ((dynamic)obj).table;
            string? modid = ((dynamic)obj).modid;
            string? qty = ((dynamic)obj).qty;
            string? user = ((dynamic)obj).user;
            string? equipment = ((dynamic)obj).equipment;
            string? banci = ((dynamic)obj).banci;
            string? jt = ((dynamic)obj).jt;
            string? cms = ((dynamic)obj).cms;
            string? bzlx = ((dynamic)obj).bzlx;
            string? ph = ((dynamic)obj).ph;
            string? lx = ((dynamic)obj).lx;
            try
            {
                DataTable? dt = SqlSugarHelp.SqlSugarTableFL($" exec wlzh_mom_qm_byTemp_ybapp '{table}','{modid}','{qty}','{user}','{equipment}','{banci}','{jt}','{cms}','{bzlx}','{ph}','{lx}'");
                if (dt.Rows.Count <= 0)
                {
                    return new { state = 0, mesg = "失败：无数据生成", success = false };
                }
                else
                {
                    string? qx = dt.Rows[0][0].ToString();
                    string? ms = dt.Rows[0][1].ToString();
                    return new { state = qx == "1" ? 1 : 0, mesg = ms, success = qx == "1" ? true : false };
                }
            }
            catch (Exception e)
            {
                return new { state = 0, mesg = e.ToString(), success = false };
            }
        }


        /// <summary>
        /// 获取可检验列表数据
        /// </summary>
        /// <param name="ob">生产订单行条码</param>
        /// <returns></returns>
        [HttpPost]
        public object ProductInspectionSelectSY(object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? code = ((dynamic)obj).tm;
                wlzh_v_ProductInspectionList_sy wl = new wlzh_v_ProductInspectionList_sy();
                List<wlzh_v_ProductInspectionList_sy> list = SqlSugarHelp.SqlSugarListProductInspectionListSY(wl, code);
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
        /// 获取检验单数据
        /// </summary>
        /// <param name="ob">生产订单行条码</param>
        /// <returns></returns>
        [HttpPost]
        public object InspectionForm (object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? code = ((dynamic)obj).tm;
                wlzh_v_InspectionForm wl = new wlzh_v_InspectionForm();
                List<wlzh_v_InspectionForm> list = SqlSugarHelp.SqlSugarListInspectionForm(wl, code);
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

    }
}

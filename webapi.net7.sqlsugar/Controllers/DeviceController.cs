using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Xml.Linq;
using webapi.net7.sqlsugar.Model;

namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// 设备管理相关接口
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名称,action则按方法名显示
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8移动端接口))]
    public class DeviceController : Controller
    {
        /// <summary>
        /// 设备信息查询
        /// </summary>
        /// <param name="sbbm">设备编码</param>
        /// <param name="sbzt">设备状态</param>
        /// <returns></returns>
        [HttpPost]
        public object DeviceSelect(FLsbcx ob)
        {
            
            

            try
            {
                wlzh_v_device wl = new wlzh_v_device();
                List<wlzh_v_device> list = SqlSugarHelp.SqlSugarListTop1(wl, ob.sbbm??"", ob.sbzt??"");
                if (list.Count <= 0)
                {
                    return new { errcode = "1", errmesg = "无对应记录", data = "", success = false };
                }
                else
                {
                    return new { errcode = "0", errmesg = "成功", data = list, success = true };
                }
            }
            catch (Exception ex) { return new { errcode = "1", errmesg = ex.ToString(), data = "", success = false }; }


        }

        /// <summary>
        /// 设备维修保养
        /// </summary>
        /// <param name="sbbm">设备编码</param>
        /// <param name="upsbzt">修改后状态</param>
        /// <param name="username">操作人员</param>
        /// <returns></returns>
        [HttpPost]
        public object DeviceUpdate(object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? sbbm = ((dynamic)obj).sbbm;
                string? upsbzt = ((dynamic)obj).upsbzt;
                string? username = ((dynamic)obj).username;
                string? bgyy = ((dynamic)obj).bgyy;

                DataTable dt = SqlSugarHelp.SqlSugarTableFL($"wlzh_p_device '{sbbm}','{upsbzt}','{username}','{bgyy}'");
                string? s = dt.Rows[0]["s"].ToString();
                string? m = dt.Rows[0]["m"].ToString();
                return new { errcode = "0", errmesg = m, data = "", success = true };


            }
            catch (Exception ex) { return new { errcode = "1", errmesg = ex.ToString(), data = "", success = false }; }


        }
        /// <summary>
        /// 设备全过程跟踪
        /// </summary>
        /// <param name="sbbm"></param>
        /// <returns></returns>
        [HttpPost]
        public object DeviceTrack(FLsbcx ob)
        {
            try
            {
                wlzh_v_device_record wl = new wlzh_v_device_record();
                List<wlzh_v_device_record> list = SqlSugarHelp.SqlSugarListTop1(wl, ob.sbbm??"", "");
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
        /// 设备和班次获取人员信息
        /// </summary>
        /// <param name="jtname">机台号</param>
        /// <param name="bz">班次</param>
        /// <returns></returns>
        [HttpPost]

        public object DevicePerson(object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? jtname = ((dynamic)obj).jtname;
                wlzh_DevicePerson wl = new wlzh_DevicePerson();
                List<wlzh_DevicePerson> list = SqlSugarHelp.SqlSugarListPerson(wl, jtname);
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
        /// 获取车间所有人员档案
        /// </summary>
        /// <param name="value">包含车间、人员条件</param>
        /// <returns></returns>
        [HttpPost]
        public object DeviceAllPerson(object ob)
        {
            try
            {
                JObject obj = JObject.Parse(ob.ToString());
                string? id = ((dynamic)obj).id??"";
                wlzh_v_person wl = new wlzh_v_person();
                List<wlzh_v_person> list;
                if (id == "")
                {
                    list=SqlSugarHelp.SqlSugarList(wl).ToList();
                    if (list.Count <= 0)
                    {
                        return new { errcode = "1", errmesg = "无对应记录", data = "", success = false };
                    }
                    else
                    {
                        return new { errcode = "0", errmesg = "", data = list, success = true };
                    }
                }
                else
                {

                     list = SqlSugarHelp.SqlSugarListAllPerson(wl, id);
                    if (list.Count <= 0)
                    {
                        return new { errcode = "1", errmesg = "无对应记录", data = "", success = false };
                    }
                    else
                    {
                        return new { errcode = "0", errmesg = "", data = list, success = true };
                    }
                }
            }
            catch (Exception ex) { return new { errcode = "1", errmesg = ex.ToString(), data = "", success = false }; }
        }

    }
}

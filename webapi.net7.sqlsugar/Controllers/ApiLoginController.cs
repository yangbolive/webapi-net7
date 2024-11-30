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
    /// 登录管理
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名称,action则按方法名显示
    [Authorize] //token生效
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8移动端接口))]

    public class ApiLoginController : ControllerBase
    {
        public static string conset = JsonHelper.GetAppSettings("Connectionstrings_fl_system:DBConnection"); //"Data Source=WIN-7TGRQ268Q0G;Initial Catalog=OA;Persist Security 
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageLogin PassWordPost(User obj)
        {
            
            string ps = Login.PassWordEncryption($"{obj.Password}");
            DataTable dt = SqlSugarHelp.SqlSugarTableFL($"select top 1 cpassword from ufsystem..ua_user where cuser_id='{obj.UserName}'");
            MessageLogin ms1 = new MessageLogin();
            if (dt.Rows.Count > 0)
            {
                string? u8ps = dt.Rows[0]["cpassword"].ToString();
                if (ps == u8ps)
                {
                    ms1.errcode = "0";
                    ms1.mesg = "成功";
                    ms1.succes = true;
                    return ms1;
                }
                else
                {
                    ms1.errcode = "1";
                    ms1.mesg = "密码错误";
                    ms1.succes = false;
                    return ms1;
                }
            }
            else
            {
                ms1.errcode = "1";
                ms1.mesg = "用户不存在";
                ms1.succes = false;
                return ms1;
            }
        }


        /// <summary>
        /// 通过U8账号和密码登录API
        /// </summary>
        /// <param name="acccount">账套号</param>
        /// <param name="sDate">登录日期</param>
        /// <param name="sServer">服务器地址</param>
        /// <param name="sUserID">用户</param>
        /// <param name="sPassword">密码</param>
        /// <returns></returns>
        [HttpPost]
        public object U8UserPassword(string acccount = "", string sDate = "", string sServer = "", string sUserID = "", string sPassword = "")
        {
            string ms = Login.wlzh_u8login(acccount, sDate, sServer, sUserID, sPassword);
            return new { state = ms == "登录成功" ? 1 : 0, mesg = ms, success = ms == "登录成功" ? true : false };
        }
        /// <summary>
        /// 用户获取权限
        /// </summary>
        /// <param name="au"></param>
        /// <returns></returns>
        [HttpPost]
        public object U8authority( Authority au)
        {
            try
            {
                DataTable? dt = SqlSugarHelp.SqlSugarTableFL($"exec wlzh_yhqx '888','2018','{au.czy}','9CA4CC96-E8AA-42F5-9DC3-CC02D9BC5D2B'");
                if (dt.Rows.Count <= 0)
                {
                    return new { state = 0, mesg = "获取权限失败", success = false };
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
        /// 获取U8APP的移动版本号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object U8appVersion()
        {
            try
            {
                version_app vs=new version_app();
               var vslist= SqlSugarHelp.SqlSugarListTopversion_app(vs);
                if (vslist.Count <= 0)
                {

                    return new { state = 0, mesg = "获取服务器版本号失败", data = "", success = false };
                }
                else
                {
                   
                    return new { state = 1, mesg = "成功", data = vslist, success = true };
                }

            }
            catch (Exception e)
            {
                return new { state = 0, mesg = e.ToString(), data = "", success = false };
            }
        }
        //作废测试接口
        //[HttpPost]
        //public object Test(object ob)
        //{
        //    JObject obj = JObject.Parse(ob.ToString());
        //    string? Name = ((dynamic)obj).name;
        //    string? Password = ((dynamic)obj).password;
        //    string jsonResult;



        //    string? sqlQuery = Name;

        //    // 创建并打开与数据库的连接
        //    using (var connection = new SqlConnection("Data Source=192.168.0.121;Initial Catalog=ufdata_777_2018;Persist Security Info=True;User ID=sa;Password=Adef1234"))
        //    {
        //        connection.Open();

        //        // 创建并执行SQL查询
        //        using (var command = new SqlCommand(sqlQuery, connection))
        //        {
        //            var reader = command.ExecuteReader();

        //            // 定义一个列表来存储查询结果
        //            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

        //            while (reader.Read())
        //            {
        //                Dictionary<string, object> rowDict = new Dictionary<string, object>();

        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    string columnName = reader.GetName(i);

        //                    if (!rowDict.ContainsKey(columnName))
        //                    {
        //                        rowDict[columnName] = null;
        //                    }

        //                    rowDict[columnName] = reader[i];
        //                }

        //                resultList.Add(rowDict);
        //            }

        //            // 关闭数据流
        //            reader.Close();

        //            // 将查询结果序列化为JSON字符串
        //            jsonResult = JsonConvert.SerializeObject(resultList);

        //        }
        //        return new { ccode = "", detail = jsonResult };
        //    }
        //}
        //作废
        //[HttpPost]
        //public object user(object user)
        //{
        //    JObject obj = JObject.Parse(user.ToString());
        //    string? Name = ((dynamic)obj).name;
        //    string? Password = ((dynamic)obj).password;
        //    string? Url= ((dynamic)obj).url;
        //    return new { name = Url,username=Name,password=Password};
        //}

        //[HttpPost]
        //public object mysqluser(object user)
        //{
        //    JObject obj = JObject.Parse(user.ToString());
        //    string? ID = ((dynamic)obj).id;
        //    DataTable dt= SqlSugarHelp.mySqlSugarTable($"select nickname from diy_user where id={ID};");
        //    string? Name= dt.Rows[0][0].ToString();
        //    return new {  id = ID, name = Name };
        //}
 
        //[HttpPost]
        //public object ceshi(object ob)
        //{
        //    JObject obj = JObject.Parse(ob.ToString());
        //    string? arry = ((dynamic)obj).sql;
        //    char[] separator = { ',' };
        //    string[] arr = arry.Split(separator);
        //    return arr;

        //}
    }
}
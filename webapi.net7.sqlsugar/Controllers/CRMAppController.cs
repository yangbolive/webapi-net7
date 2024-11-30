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
using SqlSugar;
using System.Security.Cryptography.Xml;
using Microsoft.OpenApi.Validations.Rules; // 导入System.Data.SqlClient命名空间（如果需要连接到数据库）
namespace webapi.net7.sqlsugar.Controllers
{


    /// <summary>
    /// 登录管理
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名称,action则按方法名显示
    //[Authorize] //token生效
    //版本生效
    [ApiExplorerSettings(IgnoreApi =false, GroupName=nameof(ApiVersions.东泽房源管理接口))]
    public class CRMAppControlle : ControllerBase
    {
        //获取登录的接口地址
        public static string appurl = JsonHelper.GetAppSettings("apiconfig:url");
        //获取数据库链接

        public static string conset = JsonHelper.GetAppSettings("Connectionstrings:DBConnection"); //"Data Source=WIN-7TGRQ268Q0G;Initial Catalog=OA;Persist Security Info=True;User ID=sa;Password=@wlzh44338";
        /// <summary>
        /// CRM用户登录校验
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object usertest(object user)
        {
            JObject obj = JObject.Parse(user.ToString());
            string? iphone = ((dynamic)obj).username;
            string? Password = ((dynamic)obj).password;
            string jsonResult;

            string? sqlQuery = $"select '成功' s,jiaose,username name,cast(gjrid as varchar(100)) gjrid from crm_user where iphone='{iphone}' and password='{Password}' ";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;


        }


        /// <summary>
        /// 修改密码接口
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object Getpassword(object user)
        {
            JObject obj = JObject.Parse(user.ToString());
            string? userName = ((dynamic)obj).username;
            string? newPassword = ((dynamic)obj).newpassowrd;
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_password '{userName}','{newPassword}' ";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCcuscodeList(object user)
        {
            JObject obj = JObject.Parse(user.ToString());
            string? userName = ((dynamic)obj).username ?? "";
            string? id = ((dynamic)obj).id ?? "";
            string? khlx = ((dynamic)obj).khlx ?? "";
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_khlist '{userName}','{id}','{khlx}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }
        /// <summary>
        /// 获取客户跟踪明细
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetkhgzList(object user)
        {
            JObject obj = JObject.Parse(user.ToString());
            string? id = ((dynamic)obj).id;
            string jsonResult;

            string? sqlQuery = $"select ROW_NUMBER() OVER ( ORDER BY ids DESC ) AS xh,cast(ROW_NUMBER() OVER ( ORDER BY ids DESC ) as varchar(10))+'、跟进内容：'+cast(gjnr as varchar(200)) nr,* from wlzh_v_khgzlist where id='{id}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }

        /// <summary>
        /// 跟进方式获取接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetgjfsList()
        {

            string jsonResult;

            string? sqlQuery = $"select cast(id as varchar(100)) value,SHOWVALUE label from CTP_ENUM_ITEM where REF_ENUMID=-384727839980370794"; //跟进方式查询

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }


        /// <summary>
        /// 生成跟进明细接口
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public object SetgjfsList(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id;
            string? gjr = ((dynamic)obj).gjr;
            string? gjfs = ((dynamic)obj).gjfs;
            string? gjnr = ((dynamic)obj).gjnr;
            string? xcgjrq = ((dynamic)obj).xcgjrq;
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_khgj '{id}','{gjr}','{gjfs}','{gjnr}','{xcgjrq}'"; //跟进方式查询

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }

        /// <summary>
        /// 按客户类型获取数据量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetkhlxList(object user)
        {
            JObject obj = JObject.Parse(user.ToString());
            string? khlx = ((dynamic)obj).khlx ?? "";
            string? ywy = ((dynamic)obj).ywy ?? "";
            string jsonResult;

            string? sqlQuery = $"select * from wlzh_v_khlist where (khlx='{khlx}' or '{khlx}'='') and (ywyid='{ywy}' or '{ywy}'='')";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }
        /// <summary>
        /// 客户回收分配业务接口
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public object Setkhywy(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? ywy = ((dynamic)obj).ywy ?? "";
            string? lx = ((dynamic)obj).lx ?? "";
            string jsonResult;

            string? sqlQuery = $"wlzh_p_khupdate '{lx}','{id}','{ywy}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }
        /// <summary>
        /// 客户汇总查询，无参查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getkhhzcx()
        {

            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_khslhz";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }

        /// <summary>
        /// 获取业务员接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetywyList()
        {
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_getywy";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }
        /// <summary>
        /// 获取所有枚举值接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetEunmList()
        {
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_getxx";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {

                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }
        /// <summary>
        /// 客户新增和修改
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public object Customer(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? lx = ((dynamic)obj).lx ?? "";
            string? id = ((dynamic)obj).id ?? "";
            string? xm = ((dynamic)obj).xm ?? "";
            string? xb = ((dynamic)obj).xb ?? "";
            string? sjh = ((dynamic)obj).sjh ?? "";
            string? lhkh = ((dynamic)obj).lhkh ?? "";
            string? bq = ((dynamic)obj).bq ?? "";
            string? bf = ((dynamic)obj).bf ?? "";
            string? gjfs = ((dynamic)obj).gjfs ?? "";
            string? yxjb = ((dynamic)obj).yxjb ?? "";
            string? djsm = ((dynamic)obj).djsm ?? "";
            string? xcgj = ((dynamic)obj).xcgj ?? "";
            string? rztj = ((dynamic)obj).rztj ?? "";
            string? jzqy = ((dynamic)obj).jzqy ?? "";
            string? jtjg = ((dynamic)obj).jtjg ?? "";
            string? nld = ((dynamic)obj).nld ?? "";
            string? ssyh = ((dynamic)obj).ssyh ?? "";
            string? yxmj = ((dynamic)obj).yxmj ?? "";
            string? yxcp = ((dynamic)obj).yxcp ?? "";
            string? gfts = ((dynamic)obj).gfts ?? "";
            string? dfly = ((dynamic)obj).dfly ?? "";
            string? gfyt = ((dynamic)obj).gfyt ?? "";
            string? gzys = ((dynamic)obj).gzys ?? "";
            string? yxzj = ((dynamic)obj).yxzj ?? "";
            string? yxdj = ((dynamic)obj).yxdj ?? "";
            string? yxlc = ((dynamic)obj).yxlc ?? "";
            string? yxld = ((dynamic)obj).yxld ?? "";
            string? ejqy = ((dynamic)obj).ejqy ?? "";
            string? txdz = ((dynamic)obj).txdz ?? "";
            string? sfz = ((dynamic)obj).sfz ?? "";
            string? ywy = ((dynamic)obj).ywy ?? "";
            string? wylx = ((dynamic)obj).wylx ?? "";
            string? sfaj = ((dynamic)obj).sfaj ?? "";
            string? yxfh = ((dynamic)obj).yxfh ?? "";
            string? yxfhstring = ((dynamic)obj).yxfhstring ?? "";
            string? cwxq = ((dynamic)obj).cwxq ?? "";
            string? fkfs = ((dynamic)obj).fkfs ?? "";
            string? khms = ((dynamic)obj).khms ?? "";
            string? lfcs = ((dynamic)obj).lfcs ?? "";
            string? glxm = ((dynamic)obj).glxm ?? "";
            string? glsj = ((dynamic)obj).glsj ?? "";
            string jsonResult;

            string? sqlQuery = $"wlzh_p_Customer '{lx}','{id}','{xm}','{xb}','{sjh}','{lhkh}','{bq}','{bf}','{gjfs}','{yxjb}','{djsm}','{xcgj}','{rztj}','{jzqy}','{jtjg}','{nld}','{ssyh}','{yxmj}','{yxcp}','{gfts}','{dfly}','{gfyt}','{gzys}','{yxzj}','{yxdj}','{yxlc}','{yxld}','{ejqy}','{txdz}','{sfz}','{ywy}','{wylx}','{sfaj}','{yxfh}','{yxfhstring}','{cwxq}','{fkfs}','{khms}','{lfcs}','{glxm}','{glsj}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }

        /// <summary>
        /// 页面传参接口
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetOAUser(object user)
        {

            JObject obj = JObject.Parse(user.ToString());
            string? userid = ((dynamic)obj).userid ?? "";
            string? sqlQuery = $"select * from wlzh_v_login where  login_name='{userid}'";
            DataTable dt = SqlSugarHelp.SqlSugarTable(sqlQuery);
            if (dt.Rows.Count > 0)
            {
                string? iphone = dt.Rows[0]["iphone"].ToString();
                string? ywy = dt.Rows[0]["ywy"].ToString();
                string? ywyid = dt.Rows[0]["ywyid"].ToString();
                string? login_name = dt.Rows[0]["login_name"].ToString();
                string? jiaose = dt.Rows[0]["jiaose"].ToString();
                string? url = appurl+@"/#/pages/crm_login?login_name=" + login_name + "&jiaose=" + jiaose + "&ywy=" + ywy + "&ywyid=" + ywyid + "&iphone=" + iphone;
                return new { url = url };
            }
            else
            {
                return new { url = "http://tzdzzy666.ufyct.com:7514/api" };
            }

        }
        /// <summary>
        /// 获取业务员汇总数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetYwykhhz(object user)
        {

            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? ywyid = ((dynamic)obj).ywyid ?? "";

            string? sqlQuery = $"exec wlzh_p_ywykhhz '{ywyid}'";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }
        /// <summary>
        /// 获取客户分配回收详细记录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public object Gethspflist(object user)
        {

            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? khid = ((dynamic)obj).khid ?? "";

            string? sqlQuery = $"exec wlzh_p_khfphslist '{khid}'";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;

        }
        /// <summary>
        /// 获取客户级别数据接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetkhjbList()
        {
            string jsonResult;
            string? sqlQuery = $"select *　from wlzh_v_khjblist";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }

        /// <summary>
        /// 获取即将逾期客户数据接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getjjyqkh(object user)
        {
            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? ywy = ((dynamic)obj).ywy ?? "";

            string? sqlQuery = $"exec wlzh_p_jjyqkhhz {ywy}";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }



        /// <summary>
        /// 获取客户变更信息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getkhbglist(object user)
        {
            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? id = ((dynamic)obj).id ?? "";

            string? sqlQuery = $"exec wlzh_p_khbglb {id}";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取客户变更详细信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getkhbgxq(object user)
        {
            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? id = ((dynamic)obj).id ?? "";

            string? sqlQuery = $"exec wlzh_p_khbgxq {id}";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }

        /// <summary>
        /// 获取房源详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getfyxq(object user)
        {
            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? fid = ((dynamic)obj).fid ?? "";

            string? sqlQuery = $"exec wlzh_p_fyxq {fid}";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 通过传入楼栋获取房间号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getfyfjh(object user)
        {
            string jsonResult;
            JObject obj = JObject.Parse(user.ToString());
            string? id = ((dynamic)obj).id ?? "";

            string? sqlQuery = $"exec wlzh_p_fyfjh {id}";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取所有楼栋
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getfyld()
        {
            string jsonResult;
            string? sqlQuery = $"select * from wlzh_v_fyld";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取房源中客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getfykhlist(object ob)
        {
            string jsonResult;
            JObject obj = JObject.Parse(ob.ToString());
            string? ywy = ((dynamic)obj).ywy ?? "";
            string? sqlQuery = $"select * from wlzh_v_fykh where ywy='{ywy}' or ''='{ywy}'";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }

        /// <summary>
        /// 按客户编码获取姓名手机身份证信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getfykhmx(object ob)
        {
            string jsonResult;
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? sqlQuery = $"select * from wlzh_v_fykh where value in ({id})";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }



        /// <summary>
        /// 获取计价方式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getjjfs()
        {
            string jsonResult;
            string? sqlQuery = $"select * from wlzh_v_jjfs";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取付款方式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getfkfs()
        {
            string jsonResult;
            string? sqlQuery = $"select * from wlzh_v_fkfs";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取款项类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getkxlx()
        {
            string jsonResult;
            string? sqlQuery = $"select * from wlzh_v_kxlx";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 签约状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getqyzt()
        {
            string jsonResult;
            string? sqlQuery = $"select * from wlzh_v_qyzt";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }



        /// <summary>
        /// 生成认购书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object PostRGS(wlzh_rgdzb obj)
        {
            string sqlfkmx="";
            try
            {
                string? uuid = Guid.NewGuid().ToString().Replace("-", "");
                List<wlzh_rgdzbkh>? khmx = new List<wlzh_rgdzbkh>();
                khmx = obj.khmx;
                List<wlzh_rgdzbfkmx>? fkmx = new List<wlzh_rgdzbfkmx>();
                fkmx = obj.fkmx;
                //明细数据插入认购书客户明细
                int khrow = 1;
             
                foreach (var item in khmx)
                {
                    //进行赋值处理
                    khrow++;
                    SqlSugarHelp.SqlSugarExecuteCommand1($"insert into wlzh_rgdzbkh(GID,sort,field0088,field0089,field0090,field0092,id2) values('{uuid}',{khrow},'{item.xm}','{item.sj}','{item.sfz}','{item.value}',(case when '{item.id2}'<>'' then '{item.id2}' else null end))");
                }
                //明细数据插入认购书付款计划明细
                int fkmxrow = 1;
                foreach (var item in fkmx)
                {
                    sqlfkmx = $"insert into wlzh_rgdzbfkmx(GID,id2,sort,field0075,field0076,field0077,field0080,field0081,field0082,field0083,field0084,field0085,field0086,field0087,field0091,field0096) select '{uuid}',(case when '{item.id2}'<>'' then '{item.id2}' else null end) id2,{fkmxrow},'{fkmxrow}',null,case when '{item.kxqxid}'='' then null else '{item.kxqxid}' end,'{item.kxqx}','人民币',{item.xssl ?? 0},{item.xxl ?? 0},{item.wsje ?? 0},null,null,{item.hsje ?? 0},'{item.fkqx}',null";
                    fkmxrow++;
                    SqlSugarHelp.SqlSugarExecuteCommand1($"insert into wlzh_rgdzbfkmx(GID,id2,sort,field0075,field0076,field0077,field0080,field0081,field0082,field0083,field0084,field0085,field0086,field0087,field0091,field0096) select '{uuid}',(case when '{item.id2}'<>'' then '{item.id2}' else null end) id2,{fkmxrow},'{fkmxrow}',null,case when '{item.kxqxid}'='' then null else '{item.kxqxid}' end,'{item.kxqx}','人民币',{item.xssl ?? 0},{item.xxl ?? 0},{item.wsje ?? 0},null,null,{item.hsje ?? 0},'{item.fkqx}',null");
                      
                }
                
                //明细数据插入认购书明细
                SqlSugarHelp.SqlSugarExecuteCommand1($"insert into  wlzh_rgdzb(ID,GID,modifytip,state,start_member_id,start_date,approve_member_id,approve_date,finishedflag,ratifyflag,ratify_member_id,ratify_date,sort,modify_member_id,modify_date,field0093,field0003,field0004,field0005,field0006,field0007,field0008,field0009,field0010,field0011,field0012,field0013,field0014,field0015,field0016,field0017,field0018,field0019,field0020,field0021,field0022,field0023,field0024,field0025,field0026,field0027,field0028,field0029,field0030,field0031,field0032,field0033,field0034,field0035,field0036,field0037,field0038,field0039,field0040,field0041,field0042,field0043,field0044,field0045,field0046,field0047,field0048,field0049,field0050,field0051,field0052,field0053,field0054,field0055,field0056,field0057,field0058,field0059,field0060,field0061,field0062,field0063,field0064,field0065,field0066,field0067,field0079,field0094,field0095,field0097,field0098,field0099,field0100,field0101,field0105)  select (case when '{obj.ID}'<>'' then '{obj.ID}' else null end) ID,'{uuid}','{obj.modifytip}' modifytip,1 state,'{obj.start_member_id}' start_member_id,getdate() start_date,0  approve_member_id,NULL approve_date,0 finishedflag,0 ratifyflag,0 ratify_member_id,NULL ratify_date,0 sort,'{obj.modify_member_id}' modify_member_id,getdate() modify_date,'{obj.field0093}' field0093,'{obj.field0003}' field0003,null field0004,'{obj.field0005}' field0005,null field0006,'{obj.field0007}' field0007,'{obj.field0008}' field0008,'{obj.field0009}' field0009,'{obj.field0010}' field0010,'{obj.field0011}' field0011,'{obj.field0012}' field0012,case when '{obj.field0013}'='' then null else '{obj.field0013}' end field0013,'{obj.field0014}' field0014,'{obj.field0015}' field0015,'{obj.field0016}' field0016,'{obj.field0017}' field0017,'{obj.field0018}' field0018,null field0019,null field0020,null field0021,null field0022,null field0023,null field0024,null field0025,null field0026,null field0027,null field0028,'{obj.field0029}' field0029,case when '{obj.field0030}'='' then null else '{obj.field0030}' end field0030,'{obj.field0031}' field0031,null field0032,'{obj.field0033}' field0033,null field0034,'{obj.field0035}' field0035,null field0036,case when '{obj.field0037}'='' then null else '{obj.field0037}' end field0037,case when '{obj.field0038}'='' then null else '{obj.field0038}' end  field0038,case when '{obj.field0039}'='' then null else '{obj.field0039}' end  field0039,case when '{obj.field0040}'='' then null else '{obj.field0040}' end  field0040,null field0041,'{obj.field0042}' field0042,null field0043,null field0044,null field0045,null field0046,null field0047,null field0048,null field0049,null field0050,null field0051,'{obj.field0052}' field0052,{obj.field0053} field0053,null field0054,null field0055,null field0056,null field0057,null field0058,'{obj.field0059}' field0059,case when '{obj.field0060}'='' then null else '{obj.field0060}' end  field0060,case when '{obj.field0061}'='' then null else '{obj.field0061}' end field0061,null field0062,'{obj.field0063}' field0063,null field0064,null field0065,null field0066,null field0067,null field0079,'{obj.field0094}' field0094,'{obj.field0095}' field0095,'{obj.field0097}' field0097,case when '{obj.field0098}'='' then null else '{obj.field0098}' end field0098,'{obj.field0099}','{obj.field0100}','{obj.field0101}','{obj.field0105}'");
                //数据插入完成后，执行存储过程
                DataTable dt = SqlSugarHelp.SqlSugarTable($"exec wlzh_crrgd_yb '{uuid}'");
                string result = dt.Rows[0]["result"].ToString() ?? "";
                string msg = dt.Rows[0]["cmsg"].ToString() ?? "";
                
                return new { s = result, m = msg };
            }
            catch (Exception ex) { return new { s = "0", m = ex.ToString(),sql= sqlfkmx.ToString() }; }
        }
        /// <summary>
        /// 获取顾问认购书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getrgsbt(object ob)
        {
            string jsonResult;
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? ywy= ((dynamic)obj).ywy ?? "";
            string? sqlQuery = $"select * from wlzh_v_fyrgs where fyid='{id}' and (ywy='{ywy}' or ''='{ywy}' ) ";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }

        /// <summary>
        /// 获取认购书客户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getrgskhmx(object ob)
        {
            string jsonResult;
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? ywy = ((dynamic)obj).id ?? "";
            string? sqlQuery = $"select * from wlzh_v_rgskh where fyid='{id}'";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取认购书付款计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getrgsfkmx(object ob)
        {
            string jsonResult;
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? ywy = ((dynamic)obj).id ?? "";
            string? sqlQuery = $"select * from wlzh_v_rgsfkjx where fyid='{id}'";

            // 创建并打开与数据库的连接
            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(conset))
            {
                connection.Open();

                // 创建并执行SQL查询
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var reader = command.ExecuteReader();

                    // 定义一个列表来存储查询结果
                    List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);

                            if (!rowDict.ContainsKey(columnName))
                            {
                                rowDict[columnName] = null;
                            }

                            rowDict[columnName] = reader[i];
                        }

                        resultList.Add(rowDict);
                    }

                    // 关闭数据流
                    reader.Close();

                    // 将查询结果序列化为JSON字符串
                    jsonResult = JsonConvert.SerializeObject(resultList);

                }

            }

            return jsonResult;
        }


        /// <summary>
        /// 获取认购书是否存在并且是当前业务员认购书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Getrgsdyywy(object ob)
        {
            string jsonResult;
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? ywy = ((dynamic)obj).ywy ?? "";
            string? sqlQuery = $"select top 1 fyid from wlzh_v_fyrgs where fyid='{id}' ";
            string? sqlQuery1 = $"select top 1 ywy from wlzh_v_fyrgs where ywy='{ywy}' ";
            int count=SqlSugarHelp.SqlSugarTable(sqlQuery).Rows.Count;
            int count1 = SqlSugarHelp.SqlSugarTable(sqlQuery1).Rows.Count;
            return new { fycount = count, ywycount = count1 };
            
            
        }

        /// <summary>
        /// datatable表转json功能
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDataTableToJson(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            string? name = ((dynamic)obj).name ?? "";
            DataTable dt=new DataTable();
            dt.Columns.Add("id",typeof(string));
            dt.Columns.Add("name",typeof(string) );

            dt.Rows.Add("id1","name1");
            dt.Rows.Add("id2", "name2");
            dt.Rows.Add("id3", "name3");
            

            string json= JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;

        }
        /// <summary>
        /// 获取认购书按揭银行
        /// </summary>
        /// <returns></returns>

        [HttpPost]

        public object Getrgsajyy()
        {
            string? sqlQuery = $"select * from wlzh_v_ajyh";
            DataTable dt = SqlSugarHelp.SqlSugarTable(sqlQuery);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }

        /// <summary>
        /// 获取认筹书
        /// </summary>
        /// <returns></returns>

        [HttpPost]

        public object Getrcs(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).fjh ?? "";
            string? rcdh = ((dynamic)obj).rcdh ?? "";
            string? sqlQuery = $"select * from wlzh_v_rcs where fjh='{id}' and isnull(code,'')<>'{rcdh}'";
            DataTable dt = SqlSugarHelp.SqlSugarTable(sqlQuery);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }

        /// <summary>
        /// 锁定和解锁认购书状态
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public object Updatercszt(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? id = ((dynamic)obj).id ?? "";
            int zt = ((dynamic)obj).zt ?? 0;
            string? sqlQuery = $"wlzh_p_rgssd '{id}',{zt}";
            DataTable dt = SqlSugarHelp.SqlSugarTable(sqlQuery);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }

    }
}

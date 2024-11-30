using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using Newtonsoft.Json; // 引入命名空间
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Runtime.InteropServices.ObjectiveC;
using NetTaste;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// 报表相关接口
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名称,action则按方法名显示
    //[Authorize] //token生效
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.报表接口))]
    public class ReportController : ControllerBase
    {
        public static string con = JsonHelper.GetAppSettings("Connectionstrings_zl:DBConnection");

        /// <summary>
        /// 仓库档案查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object WareHouse(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? cwhcode = ((dynamic)obj).cwhcode??"";
            string jsonResult;

            string? sqlQuery = $"select cwhcode,cwhname from warehouse where cwhcode='{cwhcode}' or ''='{cwhcode}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }
        }

        /// <summary>
        /// 客户档案查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object CustomerCx(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? ccuscode = ((dynamic)obj).ccuscode ?? "";
            string jsonResult;

            string? sqlQuery = $"select ccuscode,ccusname  from Customer where (cast(ccuscode as varchar(100))+cast(ccusname as varchar(100))) like '%{ccuscode}%'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }
        }

        /// <summary>
        /// 供应商档案查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Vendor(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? cvencode = ((dynamic)obj).cvencode ?? "";
            string jsonResult;

            string? sqlQuery = $"select cvencode,cvenname  from Vendor where (cast(cvencode as varchar(100))+cast(cvenname as varchar(100))) like '%{cvencode}%'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }
        }
        /// <summary>
        /// 查询现存量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object StandingsStock(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? cwhcode = ((dynamic)obj).cwhcode;
            string? cinvcode = ((dynamic)obj).cinvcode??"";
            string jsonResult;

            string? sqlQuery = $"select * from wlzh_v_xcl where cwhcode='{cwhcode}' and (cast(cinvcode as varchar(100))+cast(cinvname as varchar(100))+cast(cinvstd as varchar(100))) like '%{cinvcode}%'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
        /// 应付款查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Payable(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? sddate = ((dynamic)obj).sddate ?? "";
            string? eddate = ((dynamic)obj).eddate ?? "";
            string? cvencode = ((dynamic)obj).cvencode ?? "";
            string jsonResult;

            string? sqlQuery = $"wlzh_p_yfzk '{sddate}','{eddate}','{cvencode}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }
        }
        /// <summary>
        /// 应收款查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Receivables(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? sddate = ((dynamic)obj).sddate ?? "";
            string? eddate = ((dynamic)obj).eddate ?? "";
            string? ccuscode = ((dynamic)obj).ccuscode ?? "";
            string jsonResult;

            string? sqlQuery = $"wlzh_p_ysye '{sddate}','{eddate}','{ccuscode}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }

            
        }

        /// <summary>
        /// 当日收款查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Collection(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? ccuscode = ((dynamic)obj).ccuscode ?? "";
            string jsonResult;

            string? sqlQuery = $"select * from wlzh_v_drsk where (cast(ccuscode as varchar(100))+cast(ccusname as varchar(100))) like '%{ccuscode}%'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }


        }

        /// <summary>
        /// 销售统计数据表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SalesStatistics(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? lx = ((dynamic)obj).lx ?? "";
            string? sddate = ((dynamic)obj).sddate ?? "";
            string? eddate = ((dynamic)obj).eddate ?? "";
            string? ym = ((dynamic)obj).ym ?? "";
            string? cx = ((dynamic)obj).cx ?? "";
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_xstjb '{lx}','{sddate}','{eddate}','{ym}','{cx}'";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }


        }

        /// <summary>
        /// 销售统计数据表汇总数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SalesStatisticsCount(object ob)
        {
            JObject obj = JObject.Parse(ob.ToString());
            string? lx = ((dynamic)obj).lx ?? "";
            string? sddate = ((dynamic)obj).sddate ?? "";
            string? eddate = ((dynamic)obj).eddate ?? "";
            string? cx = ((dynamic)obj).cx ?? "";
            string jsonResult;

            string? sqlQuery = $"exec wlzh_p_xstjb_count  '{lx}','{sddate}','{eddate}','{cx}' ";

            // 创建并打开与数据库的连接
            using (var connection = new SqlConnection(con))
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
                return jsonResult;
            }


        }
    }
}

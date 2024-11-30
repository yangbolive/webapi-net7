using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using webapi.net7.sqlsugar.Model;




namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// 通过SQL查询
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //路由配置action修改为controller则默认路由名
    [Tenant("1")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8移动端接口))]
    public class SqlController : Controller
    {
        public readonly ISqlSugarClient DBdata;
        public readonly ISqlSugarClient DBsystem;
        public readonly ISqlSugarClient DBsql;
        public readonly ISqlSugarClient DBufsystemsql;
        public readonly ISqlSugarClient DBbwsql;

        //创建日志记录对象
        public readonly ILogger<WorkShopController> _Logger;
        public SqlController(ISqlSugarClient dbsystem, ILogger<WorkShopController> Logger)
        {
            this._Logger = Logger;
            var db = dbsystem as SqlSugarScope;
           
            this.DBdata = db.GetConnection("0"); //获取第一个数据库
            this.DBsystem = db.GetConnection("1"); //获取第二个数据库
            this.DBsql = db.GetConnection("2"); //获取大众精密账套数据库
            this.DBufsystemsql = db.GetConnection("3"); //获取大众精密ufsystem数据库
            this.DBbwsql = db.GetConnection("4"); //获取大众精密ufsystem数据库

        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public object UserLogin(object obj)
        {
            JObject ob = JObject.Parse(obj.ToString());
            string? CommandType = ((dynamic)ob).CommandType ?? "";
            string? database = ((dynamic)ob).database ?? "";
            string? Password = ((dynamic)ob).cpassword ?? "";
            string? UserName = ((dynamic)ob).cuser_id ?? "";
            string? SqlsStr = ((dynamic)ob).SqlsStr ?? "";
            try
            {
                if (CommandType == "login")
                {
                    string ps = Login.PassWordEncryption($"{Password}");
                    DataTable dt = DBufsystemsql.Ado.GetDataTable($"select top 1 cpassword from ufsystem..ua_user where cuser_id='{UserName}'");
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
                else
                {
                    DataTable dt = DBufsystemsql.Ado.GetDataTable(SqlsStr);
                    return JsonConvert.SerializeObject(dt, Formatting.Indented);

                }
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public string SqlHelp(object sql)
        {

            try
            {

                JObject obj = JObject.Parse(sql.ToString());
                string? CommandType = ((dynamic)obj).CommandType ?? "";
                string? SqlsStr = ((dynamic)obj).SqlsStr ?? "";
                string jsonResult="";
                DataTable dt;
                if (CommandType == "select")
                {
                     dt = DBsql.Ado.GetDataTable(SqlsStr);
                    jsonResult = JsonConvert.SerializeObject(dt);
                   
                }
                else if(CommandType=="update") 
                {
                    int i= DBsql.Ado.ExecuteCommand(SqlsStr);
                     var res =new { errCode=0,errMesg="",data=i.ToString() };
                    jsonResult = JsonConvert.SerializeObject(res);
                    
                }
                else
                {
                    var res = new { errCode = 1, errMesg = "CommandType类型传入错误", data = ""};
                    jsonResult = JsonConvert.SerializeObject(res);
                }
                // 将查询结果序列化为JSON字符串

                return jsonResult;
            }
            catch (Exception ex) 
            {
                var res = new
                {
                    errCode = 1,
                    errMsg = ex.Message,
                    data=""
                };
                string errResult = JsonConvert.SerializeObject(res);
                return errResult;
            }
        }

        [HttpPost]
        public string BwcySqlHelp(object sql)
        {

            try
            {

                JObject obj = JObject.Parse(sql.ToString());
                string? CommandType = ((dynamic)obj).CommandType ?? "";
                string? SqlsStr = ((dynamic)obj).SqlsStr ?? "";
                string jsonResult = "";
                DataTable dt;
                if (CommandType == "select")
                {
                    dt = DBbwsql.Ado.GetDataTable(SqlsStr);
                    var res = new { errCode = dt.Rows[0][0].ToString(), errMesg = dt.Rows[0][1].ToString(), data =dt };
                    jsonResult = JsonConvert.SerializeObject(res);

                }
                else if (CommandType == "update")
                {
                    int i = DBbwsql.Ado.ExecuteCommand(SqlsStr);
                    var res = new { errCode = 0, errMesg = "", data = i.ToString() };
                    jsonResult = JsonConvert.SerializeObject(res);

                }
                else
                {
                    var res = new { errCode = 1, errMesg = "CommandType类型传入错误", data = "" };
                    jsonResult = JsonConvert.SerializeObject(res);
                }
                // 将查询结果序列化为JSON字符串

                return jsonResult;
            }
            catch (Exception ex)
            {
                var res = new
                {
                    errCode = 1,
                    errMsg = ex.Message,
                    data = ""
                };
                string errResult = JsonConvert.SerializeObject(res);
                return errResult;
            }
        }


    }
}

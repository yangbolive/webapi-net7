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
    /// ��¼����
    /// </summary>
    [ApiController]
    [Route("api/[action]")]  //·������action�޸�Ϊcontroller��Ĭ��·������,action�򰴷�������ʾ
    [Authorize] //token��Ч
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8�ƶ��˽ӿ�))]

    public class ApiLoginController : ControllerBase
    {
        public static string conset = JsonHelper.GetAppSettings("Connectionstrings_fl_system:DBConnection"); //"Data Source=WIN-7TGRQ268Q0G;Initial Catalog=OA;Persist Security 
        /// <summary>
        /// �û���¼
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
                    ms1.mesg = "�ɹ�";
                    ms1.succes = true;
                    return ms1;
                }
                else
                {
                    ms1.errcode = "1";
                    ms1.mesg = "�������";
                    ms1.succes = false;
                    return ms1;
                }
            }
            else
            {
                ms1.errcode = "1";
                ms1.mesg = "�û�������";
                ms1.succes = false;
                return ms1;
            }
        }


        /// <summary>
        /// ͨ��U8�˺ź������¼API
        /// </summary>
        /// <param name="acccount">���׺�</param>
        /// <param name="sDate">��¼����</param>
        /// <param name="sServer">��������ַ</param>
        /// <param name="sUserID">�û�</param>
        /// <param name="sPassword">����</param>
        /// <returns></returns>
        [HttpPost]
        public object U8UserPassword(string acccount = "", string sDate = "", string sServer = "", string sUserID = "", string sPassword = "")
        {
            string ms = Login.wlzh_u8login(acccount, sDate, sServer, sUserID, sPassword);
            return new { state = ms == "��¼�ɹ�" ? 1 : 0, mesg = ms, success = ms == "��¼�ɹ�" ? true : false };
        }
        /// <summary>
        /// �û���ȡȨ��
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
                    return new { state = 0, mesg = "��ȡȨ��ʧ��", success = false };
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
        /// ��ȡU8APP���ƶ��汾��
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

                    return new { state = 0, mesg = "��ȡ�������汾��ʧ��", data = "", success = false };
                }
                else
                {
                   
                    return new { state = 1, mesg = "�ɹ�", data = vslist, success = true };
                }

            }
            catch (Exception e)
            {
                return new { state = 0, mesg = e.ToString(), data = "", success = false };
            }
        }
        //���ϲ��Խӿ�
        //[HttpPost]
        //public object Test(object ob)
        //{
        //    JObject obj = JObject.Parse(ob.ToString());
        //    string? Name = ((dynamic)obj).name;
        //    string? Password = ((dynamic)obj).password;
        //    string jsonResult;



        //    string? sqlQuery = Name;

        //    // �������������ݿ������
        //    using (var connection = new SqlConnection("Data Source=192.168.0.121;Initial Catalog=ufdata_777_2018;Persist Security Info=True;User ID=sa;Password=Adef1234"))
        //    {
        //        connection.Open();

        //        // ������ִ��SQL��ѯ
        //        using (var command = new SqlCommand(sqlQuery, connection))
        //        {
        //            var reader = command.ExecuteReader();

        //            // ����һ���б����洢��ѯ���
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

        //            // �ر�������
        //            reader.Close();

        //            // ����ѯ������л�ΪJSON�ַ���
        //            jsonResult = JsonConvert.SerializeObject(resultList);

        //        }
        //        return new { ccode = "", detail = jsonResult };
        //    }
        //}
        //����
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
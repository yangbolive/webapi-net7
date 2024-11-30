using System.Security.Cryptography;
using webapi.net7.sqlsugar.BLL;

namespace webapi.net7.sqlsugar
{
    public static class Login
    {
        //U8用户登录密码加密方式，仅供参考
        public static string PassWordEncryption(string passWord)
        {
            //加密后，最后一个特殊字符：Unicode编码
            string lastChar = "\u0003";
            //密码转换为加密字符串
            byte[] src = System.Text.Encoding.Default.GetBytes(passWord);
            string dst = Convert.ToBase64String(SHA1.Create().ComputeHash(src)) + lastChar;
            return dst;
        }



        public static string wlzh_u8login(string account= "", string sDate="", string sServer = "", string sUserID= "", string sPassword = "")
        {
            //DxComObject dxComObject = new DxComObject("U8Login.clsLogin");
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            string sSubId = "DP"; //AS,采购PU
            string sAccID = "(default)@" + account;
            string sYear = "";
            string sSerial = "";
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                return "登录失败："+u8Login.ShareString;
            }
            else
            {
                return "登录成功";
            }
        }


        public static U8Login.clsLogin? wlzh_login(string account = "", string sDate = "", string sServer = "", string sUserID = "", string sPassword = "")
        {
            //DxComObject dxComObject = new DxComObject("U8Login.clsLogin");
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            string sSubId = "SA"; //AS,采购PU
            string sAccID = "(default)@" + account;
            string sYear = "";
            string sSerial = "";
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                return null;
            }
            else
            {
                return u8Login;
            }
        }

    }
}

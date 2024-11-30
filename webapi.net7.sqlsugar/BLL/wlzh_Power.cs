using Microsoft.IdentityModel.Tokens;
using UFSoft.U8.Framework.LoginContext;
using UFSoft.U8.Framework.SecurityCommon;
using Microsoft.VisualBasic.CompilerServices;

namespace webapi.net7.sqlsugar
{
    public static class wlzh_Power
    {
        public static bool powers(object u8lgin, string mk, string mx)
        {
            //需要添加引用
            CalledContext Context = new CalledContext();
            Context.subId = mk;
            Context.token = Convert.ToString(NewLateBinding.LateGet(u8lgin, null, "userToken", new object[0], null, null, null));
            ModuleAuth auth = new ModuleAuth(Context);
            if (!auth.TaskExec(mx, -1))
            {
                if (auth.ErrNumber != 0)
                {

                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {

                return true;
            }
        }
    }
}

namespace webapi.net7.sqlsugar
{
    public static class Extension
    {
        /// <summary>
	    /// 获取客户Ip
	    /// </summary>
	    /// <param name="context"></param>
	    /// <returns></returns>
	    public static string GetClientUserIp(this HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

    }
}

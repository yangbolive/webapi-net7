﻿namespace webapi.net7.sqlsugar
{
    /// <summary>
    /// 用户友好异常
    /// </summary>
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message) : base(message)
        {

        }
    }
}

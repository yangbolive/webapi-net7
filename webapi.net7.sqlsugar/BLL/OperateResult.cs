﻿namespace webapi.net7.sqlsugar.BLL
{
    /// <summary>
    /// 操作结果封装
    /// </summary>
    public class OperateResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 操作结果
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="message">消息</param>
        /// <param name="data">操作结果</param>
        public OperateResult(bool state, string message, object data = null)
        {
            this.State = state;
            this.Message = message;
            this.Data = data;
        }
    }
}

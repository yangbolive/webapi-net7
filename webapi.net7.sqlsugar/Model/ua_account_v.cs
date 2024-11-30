using SqlSugar;

namespace webapi.net7.sqlsugar
{
    [Tenant("1")]
    public class ua_account_v
    {
        
        public string? cAcc_id { get; set; }
        public string? cAcc_Name {  get; set; }
        public string? cAcc_Path {  get; set; }
    }
}

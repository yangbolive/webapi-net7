namespace webapi.net7.sqlsugar.Model
{
    public class MessageLogin
    {
        //state = 0, mesg = e.ToString(), success
        public string? errcode {  get; set; }
        public string? mesg { get; set; }
        public bool succes {  get; set; }
    }
}

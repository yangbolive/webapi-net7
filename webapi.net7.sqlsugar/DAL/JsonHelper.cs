namespace webapi.net7.sqlsugar
{
    public class JsonHelper
    {
        public static string GetAppSettings(string key)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration Configuration = builder.Build();
            return Configuration[key];
        }

    }
}

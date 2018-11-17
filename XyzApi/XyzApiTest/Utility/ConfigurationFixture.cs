using Microsoft.Extensions.Configuration;

namespace XyzApiTest.Utility
{
    public class ConfigurationFixture 
    {
        public ConfigurationFixture()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            //
            //    Dapper config
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public IConfigurationRoot Config { get; }
    }
}
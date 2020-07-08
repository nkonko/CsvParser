namespace Rise.Innovation.CSVTools
{
    using Rise.Innovation.CSVTools.Interfaces;
    using System.Configuration;

    public class Configuration : IConfiguration
    {
        public string GetInputPath()
        {
            return ConfigurationManager.AppSettings["SourceFolder"];
        }

        public string GetOutputPath()
        {
            return ConfigurationManager.AppSettings["OutputFolder"];
        }
    }
}

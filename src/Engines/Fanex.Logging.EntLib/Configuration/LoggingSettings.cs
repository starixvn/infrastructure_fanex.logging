namespace Fanex.Logging.EntLib.Configuration
{
    public class EntLibConfig
    {
        public EntLibConfig(
            string msmqPath,
            string filePath)
        {
            MsmqPath = msmqPath;
            FilePath = filePath;
        }

        public string MsmqPath { get; }

        public string FilePath { get; }
    }
}
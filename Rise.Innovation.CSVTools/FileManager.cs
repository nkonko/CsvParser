namespace Rise.Innovation.CSVTools
{
    using Rise.Innovation.CSVTools.Interfaces;
    using System.IO;

    public class FileManager : IFileManager
    {
        public StreamReader GetStreamReader(string path)
        {
            return new StreamReader(path);
        }

        public StreamWriter GetStreamWriter(string path)
        {
            return new StreamWriter(path);
        }
    }
}

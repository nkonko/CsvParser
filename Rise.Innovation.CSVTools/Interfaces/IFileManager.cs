namespace Rise.Innovation.CSVTools.Interfaces
{
    using System.IO;

    public interface IFileManager
    {
        StreamReader GetStreamReader(string path);

        StreamWriter GetStreamWriter(string path);
    }
}

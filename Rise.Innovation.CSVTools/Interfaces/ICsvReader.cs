namespace Rise.Innovation.CSVTools.Interfaces
{
    using System.Data;

    public interface ICsvReader
    {
        DataTable ConvertCSVtoDataTable(string name, string filepath, string joinId);
    }
}

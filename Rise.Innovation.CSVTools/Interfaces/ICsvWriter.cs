namespace Rise.Innovation.CSVTools.Interfaces
{
    using System.Data;
    using System.Threading.Tasks;

    public interface ICsvWriter
    {
        Task WriteCsv(DataTable mergedTable, string joinCol, string output);
    }
}
namespace Rise.Innovation.CSVTools.Interfaces
{
    using Model;
    using System.Threading.Tasks;

    public interface ICSVFileMerge
    {
        Task GetMergedFile(FileMergeRequest request);
    }
}

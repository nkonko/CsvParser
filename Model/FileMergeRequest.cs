namespace Model
{
    public class FileMergeRequest
    {
        public string FileNameOne { get; set; }

        public string FileNameTwo { get; set; }

        public string OutputFileName { get; set; }

        public string JoinColumnName { get; set; }

        public FileMergeRequest(string fileNameOne, string fileNameTwo, string outputFileName, string joinColumnName)
        {
            FileNameOne = fileNameOne;
            FileNameTwo = fileNameTwo;
            OutputFileName = outputFileName;
            JoinColumnName = joinColumnName;
        }
    }
}

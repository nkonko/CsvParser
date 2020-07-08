namespace Rise.Innovation.SkillsAssessment
{
    using Model;
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class CSVFileMerge : ICSVFileMerge
    {
        private const string StartLabel = "Start Merge Service";
        private const string MergedLabel = "Files Merged";
        private readonly ICsvReader reader;
        private readonly ICsvWriter writer;
        private readonly IConfiguration config;

        public CSVFileMerge(ICsvReader reader, ICsvWriter writer, IConfiguration config)
        {
            this.reader = reader;
            this.writer = writer;
            this.config = config;
        }

        public async Task GetMergedFile(FileMergeRequest request)
        {
            try
            {
                Console.WriteLine(StartLabel);
                var file1 = $"{config.GetInputPath()}{request.FileNameOne}";
                var file2 = $"{config.GetInputPath()}{request.FileNameTwo}";
                var output = $"{config.GetOutputPath()}{request.OutputFileName}";

                var dt1 = reader.ConvertCSVtoDataTable("1", file1, request.JoinColumnName);
                var dt2 = reader.ConvertCSVtoDataTable("2", file2, request.JoinColumnName);

                dt1.Merge(dt2, false, MissingSchemaAction.AddWithKey);
                Console.WriteLine(MergedLabel);

                await writer.WriteCsv(dt1, request.JoinColumnName, output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                await Task.FromException(ex);
            }
        }
    }
}

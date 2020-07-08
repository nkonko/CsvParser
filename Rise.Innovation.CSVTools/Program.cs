namespace Rise.Innovation.SkillsAssessment
{
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using Rise.Innovation.CSVTools;
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddSingleton<ICSVFileMerge, CSVFileMerge>()
            .AddSingleton<ICsvReader, CsvReader>()
            .AddSingleton<ICsvWriter, CsvWriter>()
            .AddSingleton<IConfiguration, Configuration>()
            .AddSingleton<IFileManager, FileManager>()
            .BuildServiceProvider();

            var request = new FileMergeRequest(
                fileNameOne: args[0],
                fileNameTwo: args[1],
                outputFileName: args[2],
                joinColumnName: args[3]);

            var fileMergeTool = serviceProvider.GetService<ICSVFileMerge>();

            var task = Task.Run(() => fileMergeTool.GetMergedFile(request));
            task.Wait();

            if (task.IsFaulted)
            {
                Console.WriteLine($"An error occurred while performing the merge");
            }
            else
            {
                Console.WriteLine($"Result file created");
            }
        }
    }
}

namespace Rise.Innovation.CSVTools
{
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.Data;
    using System.Text.RegularExpressions;

    public class CsvReader : ICsvReader
    {
        private const string StartLabel = "Start Read Service";
        private const string FinishLabel = "Finished Read Service";

        private readonly IFileManager fileManager;

        public CsvReader(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        public DataTable ConvertCSVtoDataTable(string name, string filepath, string joinId)
        {
            var dt = new DataTable();

            try
            {
                Console.WriteLine(StartLabel);

                var sr = fileManager.GetStreamReader(filepath);
                var headers = sr.ReadLine().Split(',');

                foreach (var header in headers)
                {
                    dt.Columns.Add(GetHeader(name, header, joinId));

                    if (header == joinId)
                    {
                        dt.PrimaryKey = new DataColumn[] { dt.Columns[joinId] };
                    }
                }

                while (!sr.EndOfStream)
                {
                    var rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    var dr = dt.NewRow();

                    for (int i = 0; i < rows.Length; i++)
                    {
                        dr[i] = !string.IsNullOrWhiteSpace(rows[i]) ? rows[i] : "NULL";
                    }

                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }

            Console.WriteLine(FinishLabel);
            return dt;
        }

        private string GetHeader(string name, string header, string joinId)
        {
            return header != joinId ? $"{name}_" + header : header;
        }
    }
}

namespace Rise.Innovation.CSVTools
{
    using Model;
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class CsvWriter : ICsvWriter
    {
        private const string StartLabel = "Start Write Service";
        private const string FinishLabel = "Finished Write Service";

        private readonly IFileManager fileManager;

        public CsvWriter(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        public async Task WriteCsv(DataTable mergedTable, string joinCol, string output)
        {
            try
            {
                Console.WriteLine(StartLabel);

                var writer = fileManager.GetStreamWriter(output);

                var dictColumns = new Dictionary<string, Columns>();

                foreach (DataColumn column in mergedTable.Columns)
                {
                    if (column.ToString() == joinCol)
                    {
                        AddColumn(dictColumns, column);
                    }
                    else
                    {
                        var nameCol = column.ToString().Substring(2, column.ToString().Length - 2);

                        if (dictColumns.ContainsKey(nameCol))
                        {
                            dictColumns[nameCol].SourceNames.Add(column.ToString());
                        }
                        else
                        {
                            AddColumn(dictColumns, column, nameCol);
                        }
                    }
                }

                foreach (var column in dictColumns)
                {
                    await writer.WriteAsync(column.Key + ",");
                }

                writer.WriteLine();

                foreach (DataRow row in mergedTable.Rows)
                {
                    foreach (var col in dictColumns)
                    {
                        var sourceCols = col.Value.SourceNames;
                        var temp = (object)null;

                        foreach (var colName in sourceCols)
                        {
                            if (row[colName] != DBNull.Value)
                            {
                                temp = row[colName];
                            }
                        }

                        await writer.WriteAsync(temp + ",");
                    }

                    writer.WriteLine();
                }

                Console.WriteLine(FinishLabel);

                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                await Task.FromException(ex);
            }
        }

        private void AddColumn(Dictionary<string, Columns> dictColumns, DataColumn column, string modifiedName = null)
        {
            if (string.IsNullOrEmpty(modifiedName))
            {
                dictColumns.Add(
                                column.ToString(),
                                new Columns()
                                {
                                    Name = column.ToString(),
                                    SourceNames = new List<string>() { column.ToString() }
                                });
            }
            else
            {
                dictColumns.Add(
                                modifiedName,
                                new Columns()
                                {
                                    Name = modifiedName,
                                    SourceNames = new List<string>() { column.ToString() }
                                });
            }
        }
    }
}

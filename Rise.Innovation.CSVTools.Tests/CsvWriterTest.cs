namespace Rise.Innovation.CSVTools.Tests
{
    using Moq;
    using NUnit.Framework;
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class CsvWriterTest
    {
        private Mock<IFileManager> mockFileManager;
        private CsvWriter writer;

        [SetUp]
        public void SetUp()
        {
            mockFileManager = new Mock<IFileManager>();
            writer = new CsvWriter(mockFileManager.Object);
        }

        [Test]
        public void WriterMustReturnOk()
        {
            var fakeStream = new MemoryStream();

            mockFileManager
                .Setup(f => f.GetStreamWriter(It.IsAny<string>()))
                .Returns(() => new StreamWriter(fakeStream));
            var mock = GetMockedCsv();

            var task = Task.Run(() => writer.WriteCsv(mock, "Id", "output"));
            task.Wait();

            Assert.AreEqual(task.Status, TaskStatus.RanToCompletion);
        }

        [Test]
        public void WriterMustReturnError()
        {
            var fakeStream = new MemoryStream();

            mockFileManager
                .Setup(f => f.GetStreamWriter(It.IsAny<string>()))
                .Throws<Exception>();
            var mock = default(DataTable);

            var task = Task.Run(() => writer.WriteCsv(mock, "Id", "output"));

            Assert.Throws<AggregateException>(() => task.Wait());
        }

        private DataTable GetMockedCsv()
        {
            var table = new DataTable();

            var colTypeId = new DataColumn()
            {
                ColumnName = "Id",
                DataType = typeof(int)
            };
            table.Columns.Add(colTypeId);

            var colName = new DataColumn()
            {
                ColumnName = "1_City",
                DataType = typeof(string)
            };
            table.Columns.Add(colName);

            var colJoinId = new DataColumn()
            {
                ColumnName = "1_Population",
                DataType = typeof(int)
            };
            table.Columns.Add(colJoinId);

            var colbio = new DataColumn()
            {
                ColumnName = "1_Test",
                DataType = typeof(string)
            };
            table.Columns.Add(colbio);

            var colbio2 = new DataColumn()
            {
                ColumnName = "2_Test",
                DataType = typeof(string)
            };
            table.Columns.Add(colbio2);

            var datarow = table.NewRow();
            datarow["Id"] = 1;
            datarow["1_City"] = "Test 1";
            datarow["1_Population"] = 1000;
            datarow["1_Test"] = string.Empty;
            table.Rows.Add(datarow);

            var datarow2 = table.NewRow();
            datarow2["Id"] = 1;
            datarow2["1_City"] = "Test 2";
            datarow2["1_Population"] = 2000;
            datarow2["1_Test"] = "-";
            table.Rows.Add(datarow2);

            var datarow3 = table.NewRow();
            datarow3["Id"] = 1;
            datarow3["1_City"] = "Test 3";
            datarow3["1_Population"] = 3000;
            datarow3["1_Test"] = string.Empty;
            table.Rows.Add(datarow3);

            var datarow4 = table.NewRow();
            datarow4["Id"] = 1;
            datarow4["1_City"] = "Test 4";
            datarow4["1_Population"] = 4000;
            datarow4["1_Test"] = "!";
            table.Rows.Add(datarow4);

            return table;
        }
    }
}

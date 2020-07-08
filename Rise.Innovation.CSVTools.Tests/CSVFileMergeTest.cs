namespace Rise.Innovation.SkillsAssessment.Tests
{
    using Model;
    using Moq;
    using NUnit.Framework;
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    [TestFixture]
    public class CSVFileMergeTest
    {
        private Mock<ICsvWriter> mockWritter;
        private Mock<ICsvReader> mockReader;
        private Mock<IConfiguration> mockConfiguration;
        private CSVFileMerge csvFileMerge;

        [SetUp]
        public void SetUp()
        {
            mockWritter = new Mock<ICsvWriter>();
            mockReader = new Mock<ICsvReader>();
            mockConfiguration = new Mock<IConfiguration>();
            csvFileMerge = new CSVFileMerge(mockReader.Object, mockWritter.Object, mockConfiguration.Object);
        }

        [Test]
        public void MergerMustReturnOk()
        {
            mockConfiguration
                .Setup(f => f.GetInputPath())
                .Returns("C:\\csv\\");

            mockConfiguration
                .Setup(c => c.GetOutputPath())
                .Returns("C:\\csv\\output\\");

            mockReader
                .SetupSequence(r => r.ConvertCSVtoDataTable(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetMockedCsv1())
                .Returns(GetMockedCsv2());

            mockWritter
                .Setup(w => w.WriteCsv(It.IsAny<DataTable>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var request = new FileMergeRequest("first.csv", "second.csv", "CSV_Files_Merged.csv", "IdCity");

            var task = Task.Run(() => csvFileMerge.GetMergedFile(request));
            task.Wait();

            Assert.AreEqual(task.Status, TaskStatus.RanToCompletion);
            mockWritter.Verify(w => w.WriteCsv(It.IsAny<DataTable>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void MergerMustReturnError()
        {
            mockConfiguration
               .Setup(f => f.GetInputPath())
               .Returns(string.Empty);

            mockConfiguration
                .Setup(c => c.GetOutputPath())
                .Returns("C:\\csv\\output\\");

            mockReader
               .Setup(r => r.ConvertCSVtoDataTable(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            var request = new FileMergeRequest("first.csv", "second.csv", "CSV_Files_Merged.csv", "IdCity");
            var task = Task.Run(() => csvFileMerge.GetMergedFile(request));

            Assert.Throws<AggregateException>(() => task.Wait());
            mockWritter.Verify(w => w.WriteCsv(It.IsAny<DataTable>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        private DataTable GetMockedCsv1()
        {
            var table = new DataTable();

            var colTypeId = new DataColumn()
            {
                ColumnName = "PersonId",
                DataType = typeof(int)
            };
            table.Columns.Add(colTypeId);

            var colName = new DataColumn()
            {
                ColumnName = "Name",
                DataType = typeof(string)
            };
            table.Columns.Add(colName);

            var colJoinId = new DataColumn()
            {
                ColumnName = "CityId",
                DataType = typeof(int)
            };
            table.Columns.Add(colJoinId);

            var colbio = new DataColumn()
            {
                ColumnName = "Bio",
                DataType = typeof(string)
            };
            table.Columns.Add(colbio);

            var datarow = table.NewRow();
            datarow["PersonId"] = 1;
            datarow["Name"] = "Jorge";
            datarow["CityId"] = 1;
            datarow["Bio"] = string.Empty;
            table.Rows.Add(datarow);

            var datarow2 = table.NewRow();
            datarow2["PersonId"] = 2;
            datarow2["Name"] = "Gabriel";
            datarow2["CityId"] = 2;
            datarow2["Bio"] = string.Empty;
            table.Rows.Add(datarow2);

            var datarow3 = table.NewRow();
            datarow3["PersonId"] = 3;
            datarow3["Name"] = "Lisandro";
            datarow3["CityId"] = 3;
            datarow3["Bio"] = string.Empty;
            table.Rows.Add(datarow3);

            var datarow4 = table.NewRow();
            datarow4["PersonId"] = 4;
            datarow4["Name"] = "Nicolas";
            datarow4["CityId"] = 4;
            datarow4["Bio"] = string.Empty;
            table.Rows.Add(datarow4);

            return table;
        }

        private DataTable GetMockedCsv2()
        {
            var table = new DataTable();

            var colJoinId = new DataColumn()
            {
                ColumnName = "CityId",
                DataType = typeof(int)
            };
            table.Columns.Add(colJoinId);

            var colName = new DataColumn()
            {
                ColumnName = "CityName",
                DataType = typeof(string)
            };
            table.Columns.Add(colName);

            var colPopulation = new DataColumn()
            {
                ColumnName = "Population",
                DataType = typeof(int)
            };
            table.Columns.Add(colPopulation);

            var colbio = new DataColumn()
            {
                ColumnName = "Bio",
                DataType = typeof(string)
            };
            table.Columns.Add(colbio);

            var datarow = table.NewRow();
            datarow["CityId"] = 1;
            datarow["CityName"] = "Buenos Aires";
            datarow["Population"] = 210000;
            datarow["Bio"] = "Test1";
            table.Rows.Add(datarow);

            var datarow2 = table.NewRow();
            datarow["CityId"] = 2;
            datarow["CityName"] = "Paris";
            datarow["Population"] = 220000;
            datarow["Bio"] = "Test1";
            table.Rows.Add(datarow2);

            var datarow3 = table.NewRow();
            datarow["CityId"] = 3;
            datarow["CityName"] = "Rome";
            datarow["Population"] = 230000;
            datarow["Bio"] = "Test1";
            table.Rows.Add(datarow3);

            var datarow4 = table.NewRow();
            datarow["CityId"] = 4;
            datarow["CityName"] = "Miami";
            datarow["Population"] = 240000;
            datarow["Bio"] = "Test1";
            table.Rows.Add(datarow4);

            return table;
        }
    }
}

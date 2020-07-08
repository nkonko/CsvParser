namespace Rise.Innovation.CSVTools.Tests
{
    using Moq;
    using NUnit.Framework;
    using Rise.Innovation.CSVTools.Interfaces;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class CsvReaderTest
    {
        private Mock<IFileManager> mockFileManager;
        private CsvReader reader;

        [SetUp]
        public void SetUp()
        {
            mockFileManager = new Mock<IFileManager>();
            reader = new CsvReader(mockFileManager.Object);
        }

        [Test]
        public void ReaderMustReturnOk()
        {
            var fakeFileContents = "Id,Person,Population,Test \n 1,2,3,4";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);
            var fakeStream = new MemoryStream(fakeFileBytes);

            mockFileManager
                .Setup(f => f.GetStreamReader(It.IsAny<string>()))
                .Returns(() => new StreamReader(fakeStream));

            var arg1 = "1";
            var arg2 = "first.csv";
            var arg3 = "Id";
            var table = reader.ConvertCSVtoDataTable(arg1, arg2, arg3);

            Assert.IsTrue(table.PrimaryKey.FirstOrDefault().ColumnName == "Id");
            Assert.IsTrue(table.Rows.Count == 1);
            Assert.IsTrue(table.Columns.Count == 4);
        }

        [Test]
        public void MergerMustReturnError()
        {
            mockFileManager
               .Setup(f => f.GetStreamReader(It.IsAny<string>()))
               .Throws<Exception>();

            var arg1 = "1";
            var arg2 = "first.csv";
            var arg3 = "Id";
            var table = reader.ConvertCSVtoDataTable(arg1, arg2, arg3);

            Assert.IsNull(table.PrimaryKey.FirstOrDefault());
            Assert.IsTrue(table.Rows.Count == 0);
            Assert.IsTrue(table.Columns.Count == 0);
        }
    }
}

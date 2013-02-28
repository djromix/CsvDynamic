using System;
using System.IO;
using NUnit.Framework;

namespace CsvDynamic.UnitTests
{
    [TestFixture]
    public class CsvDynamicTests
    {
        private string _fileName;
        private string[] _fileContents;
        private string _fileBroken;
        private string _fileMismatched;
        private string _fileEmpty;
        private string _fileMalformed;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _fileName = @"Sample.csv";
            _fileContents = File.ReadAllLines(_fileName);

            _fileBroken = @"SampleBroken.csv";
            _fileMismatched = @"SampleMismatched.csv";
            _fileMalformed = @"SampleMalformed.csv";
            _fileEmpty = @"SampleEmpty.csv";
        }

        #endregion

        #region Tests


        [Test]
        public void ReadCsv_FileIsOK_ReturnsValidItems()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test
            var result = CsvDynamic.ReadCsv(_fileName);

            //
            // Assert
            //
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Date == @"2/27/2013");
            Assert.IsTrue(result[0].Description == @"Jimmy John's");
            Assert.IsTrue(result[0].OriginalDescription == @"JIMMY JOHNS");
            Assert.IsTrue(result[0].TransactionType == @"debit");
            Assert.IsTrue(result[0].Category == @"Restaurants");
            Assert.IsTrue(result[0].AccountName == @"CREDIT CARD");
            Assert.IsTrue(result[0].Labels == string.Empty);
            Assert.IsTrue(result[0].Notes == string.Empty);
        }

        [Test]
        public void ReadCsv_FileIsBroken_ReturnsException()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test

            //
            // Assert
            //
            Assert.Throws<CsvDynamicException>(() => CsvDynamic.ReadCsv(_fileBroken),
                "Only one row was found.");
        }


        [Test]
        public void ReadCsv_FileIsMismatched_ReturnsException()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test

            //
            // Assert
            //
            Assert.Throws<CsvDynamicException>(() => CsvDynamic.ReadCsv(_fileMismatched),
                "Not all rows had equal cell count.");
        }


        [Test]
        public void ReadCsv_FileIsMalformed_ReturnsException()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test

            //
            // Assert
            //
            Assert.Throws<Microsoft.VisualBasic.FileIO.MalformedLineException>(
                () => CsvDynamic.ReadCsv(_fileMalformed));
        }


        [Test]
        public void ReadCsv_FilePathIsEmpty_ReturnsException()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test

            //
            // Assert
            //
            Assert.Throws<ArgumentException>(() => CsvDynamic.ReadCsv(string.Empty),
                "Empty path name is not legal.");
        }


        [Test]
        public void ReadCsv_FileDoesNotExist_ReturnsException()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test

            //
            // Assert
            //
            Assert.Throws<FileNotFoundException>(() => CsvDynamic.ReadCsv("FileNotFound.csv"));
        }


        [Test]
        public void ReadCsv_FileIsEmpty_ReturnsException()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test

            //
            // Assert
            //
            Assert.Throws<CsvDynamicException>(() => CsvDynamic.ReadCsv(_fileEmpty),
                "No rows were found.");
        }


        [Test]
        public void ReadCsvMapped_FileIsOK_ReturnsValidItems()
        {
            //
            // Arrange
            //

            //
            // Act
            //

            // Call function being test
            var result = CsvDynamic.ReadCsv(_fileName, i => new SampleItem(i));

            //
            // Assert
            //
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Date == new DateTime(2013, 2, 27));
            Assert.IsTrue(result[0].Description == @"Jimmy John's");
            Assert.IsTrue(result[0].OriginalDescription == @"JIMMY JOHNS");
            Assert.IsTrue(result[0].Amount == 6.58m);
            Assert.IsTrue(result[0].TransactionType == @"debit");
            Assert.IsTrue(result[0].Category == @"Restaurants");
            Assert.IsTrue(result[0].AccountName == @"CREDIT CARD");
            Assert.IsTrue(result[0].Labels == string.Empty);
            Assert.IsTrue(result[0].Notes == string.Empty);
        }

        [Test]
        public void ConvertFromCsv_FileIsOK_ReturnsValidItems()
        {
            //
            // Arrange
            //


            //
            // Act
            //

            // Call function being test
            var result = _fileContents.ConvertFromCsv();

            //
            // Assert
            //
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Date == @"2/27/2013");
            Assert.IsTrue(result[0].Description == @"Jimmy John's");
            Assert.IsTrue(result[0].OriginalDescription == @"JIMMY JOHNS");
            Assert.IsTrue(result[0].TransactionType == @"debit");
            Assert.IsTrue(result[0].Category == @"Restaurants");
            Assert.IsTrue(result[0].AccountName == @"CREDIT CARD");
            Assert.IsTrue(result[0].Labels == string.Empty);
            Assert.IsTrue(result[0].Notes == string.Empty);
        }

        [Test]
        public void ConvertFromCsvMapped_FileIsOK_ReturnsValidItems()
        {
            //
            // Arrange
            //

            //
            // Act
            //

            // Call function being test
            var result = _fileContents.ConvertFromCsv(i => new SampleItem(i));

            //
            // Assert
            //
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Date == new DateTime(2013, 2, 27));
            Assert.IsTrue(result[0].Description == @"Jimmy John's");
            Assert.IsTrue(result[0].OriginalDescription == @"JIMMY JOHNS");
            Assert.IsTrue(result[0].Amount == 6.58m);
            Assert.IsTrue(result[0].TransactionType == @"debit");
            Assert.IsTrue(result[0].Category == @"Restaurants");
            Assert.IsTrue(result[0].AccountName == @"CREDIT CARD");
            Assert.IsTrue(result[0].Labels == string.Empty);
            Assert.IsTrue(result[0].Notes == string.Empty);
        }

        #endregion

        #region Supporting Code

        public class SampleItem
        {
            public SampleItem(dynamic item)
            {
                Date = DateTime.Parse(item.Date.Trim('\"'));
                Description = item.Description.Trim('\"');
                OriginalDescription = item.OriginalDescription.Trim('\"');
                Amount = decimal.Parse(item.Amount.Trim('\"'));
                TransactionType = item.TransactionType.Trim('\"');
                Category = item.Category.Trim('\"');
                AccountName = item.AccountName.Trim('\"');
                Labels = item.Labels.Trim('\"');
                Notes = item.Notes.Trim('\"');
            }

            public DateTime Date { get; set; }
            public string Description { get; set; }
            public string OriginalDescription { get; set; }
            public decimal Amount { get; set; }
            public string TransactionType { get; set; }
            public string Category { get; set; }
            public string AccountName { get; set; }
            public string Labels { get; set; }
            public string Notes { get; set; }
        }

        #endregion
    }
}

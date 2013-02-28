using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CsvToLinq.UnitTests
{
    [TestFixture]
    public class CsvToLinqExtensionTests
    {
        private string _fileName;
        private string[] _fileContents;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _fileName = @"Sample.csv";
            _fileContents = File.ReadAllLines(_fileName);
        }

        #endregion

        #region Tests

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

            Assert.IsTrue(result[0].Date == @"""2/27/2013""");
        }

        #endregion
    }
}

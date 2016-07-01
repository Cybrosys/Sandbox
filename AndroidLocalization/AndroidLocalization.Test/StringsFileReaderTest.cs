using System;
using AndroidLocalization.Data;
using Rhino.Mocks;
using System.Xml.Linq;
using NUnit.Framework;
using System.Reflection;
using System.IO;

namespace AndroidLocalization.Test
{
    [TestFixture]
    public partial class StringsFileReaderTest
    {
        [Test]
        [TestCase("TestData\\values\\Strings.xml")]
        public void Read_all_returns_all_rows(string relativePath)
        {
            // Arrange
            var filePath = Path.Combine(_directoryName, relativePath);
            var document = XDocument.Load(filePath);

            // Act
            var rows = _reader.ReadAll(document);

            // Assert
            Assert.That(rows, Is.Not.Empty);
            Assert.That(rows.Count, Is.EqualTo(5));
        }
    }

    public partial class StringsFileReaderTest
    {
        private StringsFileReader _reader;
        private string _directoryName;

        [SetUp]
        public void Init()
        {
            _reader = new StringsFileReader();
            _directoryName = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(StringsFileLoaderTest)).Location), @"..\..\")}";
        }
    }
}

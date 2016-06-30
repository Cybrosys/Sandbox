using System;
using AndroidLocalization.Data;
using Rhino.Mocks;
using System.Xml.Linq;
using NUnit.Framework;
using System.Reflection;

namespace AndroidLocalization.Test
{
    [TestFixture]
    public partial class StringsFileReaderTest
    {
        [Test]
        public void Read_all_returns_all_rows()
        {
            // Arrange
            var document = XDocument.Load(_filePath);

            // Act
            var rows = _reader.ReadAll(document);

            // Assert
            Assert.That(rows, Is.Not.Empty);
            Assert.That(rows.Count, Is.EqualTo(5));
        }
    }

    public partial class StringsFileReaderTest
    {
        private string _filePath;
        private StringsFileReader _reader;

        [SetUp]
        public void Init()
        {
            _filePath = $"{System.IO.Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location)}\\TestData\\values-sv\\Strings.xml";
            _reader = new StringsFileReader();
        }
    }
}

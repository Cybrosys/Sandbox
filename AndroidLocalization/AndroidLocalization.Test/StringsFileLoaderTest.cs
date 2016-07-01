using AndroidLocalization.Data;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Test
{
    [TestFixture]
    public partial class StringsFileLoaderTest
    {
        [Test]
        [TestCase("TestData\\values\\Strings.xml", "")]
        [TestCase("TestData\\values-sv\\Strings.xml", "sv")]
        [TestCase("TestData\\values-de\\Strings.xml", "de")]
        public void Load_returns_country_code_and_all_rows(string relativePath, string expectedCountryCode)
        {
            // Arrange
            var filePath = Path.Combine(_directoryName, relativePath);

            // Act
            var result = _loader.Load(filePath);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.LanguageCode, Is.EqualTo(expectedCountryCode));
            Assert.That(result.Rows, Is.Not.Empty);
            Assert.That(result.Rows.Count, Is.EqualTo(5));
        }
    }

    public partial class StringsFileLoaderTest
    {
        private StringsFileLoader _loader;
        private string _directoryName;

        [SetUp]
        public void Init()
        {
            _loader = new StringsFileLoader(new StringsFileReader());
            _directoryName = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(StringsFileLoaderTest)).Location), @"..\..\")}";
        }
    }
}

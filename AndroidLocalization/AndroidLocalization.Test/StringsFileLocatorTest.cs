using AndroidLocalization.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Test
{
    [TestFixture]
    public partial class StringsFileLocatorTest
    {
        [Test]
        public void Get_file_paths_returns_file_paths()
        {
            // Arrange
            // Act
            var result = _locator.GetFilePaths(_directoryName).ToList();

            // Assert
            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(5));
        }
    }

    public partial class StringsFileLocatorTest
    {
        private IStringsFileLocator _locator;
        private string _directoryName;

        [SetUp]
        public void Init()
        {
            _locator = new StringsFileLocator();
            _directoryName = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(StringsFileLoaderTest)).Location), @"..\..\")}";
        }
    }
}

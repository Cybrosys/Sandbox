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
    public partial class StringsFileDataTableBuilderTest
    {
        [Test]
        public void Build_returns_populated_data_table()
        {
            // Arrange
            var stringsFiles = _locator.GetFilePaths(_directoryName).Select(_loader.Load).ToList();

            // Act
            var result = _builder.Build(stringsFiles);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(1 + stringsFiles.Count));
            Assert.That(result.Rows.Count, Is.EqualTo(5));
        }
    }

    public partial class StringsFileDataTableBuilderTest
    {
        private IStringsFileLocator _locator;
        private IStringsFileLoader _loader;
        private IStringsFileDataTableBuilder _builder;
        private string _directoryName;

        [SetUp]
        public void Init()
        {
            _locator = new StringsFileLocator();
            _loader = new StringsFileLoader(new StringsFileReader());
            _builder = new StringsFileDataTableBuilder();
            _directoryName = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(StringsFileLoaderTest)).Location), @"..\..\")}";
        }
    }
}

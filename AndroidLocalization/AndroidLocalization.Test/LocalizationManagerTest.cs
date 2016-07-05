using AndroidLocalization.Data;
using AndroidLocalization.Managers;
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
    public partial class LocalizationManagerTest
    {
        [Test]
        public void Get_strings_files_returns_all_strings_files()
        {
            // Arrange
            // Act
            var result = _manager.GetStringsFiles(_directoryName);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        async public Task Get_strings_files_async_returns_all_strings_files()
        {
            // Arrange
            // Act
            var result = await _manager.GetStringsFilesAsync(_directoryName).ConfigureAwait(false);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void Create_data_table_returns_populated_data_table()
        {
            // Arrange
            var files = _manager.GetStringsFiles(_directoryName);

            // Act
            var result = _manager.CreateDataTable(files);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(4));
            Assert.That(result.Rows.Count, Is.EqualTo(5));
        }
    }

    public partial class LocalizationManagerTest
    {
        private LocalizationManager _manager;
        private string _directoryName;

        [SetUp]
        public void Init()
        {
            _manager = new LocalizationManager(
                new StringsFileLocator(),
                new StringsFileLoader(new StringsFileReader()),
                new StringsFileDataTableBuilder(),
                new DataTableStringsFileMapper(),
                new StringsFileSaver(new StringsFileWriter()));
            _directoryName = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(StringsFileLoaderTest)).Location), @"..\..\")}";
        }
    }
}

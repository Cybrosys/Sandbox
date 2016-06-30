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
    }

    public partial class LocalizationManagerTest
    {
        private LocalizationManager _manager;
        private string _directoryName;

        [SetUp]
        public void Init()
        {
            _manager = new LocalizationManager(new StringsFileLoader(new StringsFileReader()));
            _directoryName = $"{Path.GetDirectoryName(Assembly.GetAssembly(typeof(StringsFileLoaderTest)).Location)}\\TestData\\";
        }
    }
}

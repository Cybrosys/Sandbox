using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Test
{
    [TestFixture]
    public partial class FactoryTest
    {
        [Test]
        public void Get_objet_with_id_of_0()
        {
            // Arrange
            const int id = 0;

            // Act
            var model = Factory.Get<object>(id);

            // Assert
            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void Get_object_with_id_of_0_twice_same_reference_returned()
        {
            // Arrange
            const int id = 0;

            // Act
            var firstReference = Factory.Get<object>(id);
            var secondReference = Factory.Get<object>(id);

            // Assert
            Assert.That(secondReference, Is.SameAs(firstReference));
        }

        [Test]
        public void Get_object_with_id_0_and_1_not_same_reference_returned()
        {
            // Arrange
            const int firstId = 0;
            const int secondId = 1;

            // Act
            var firstReference = Factory.Get<object>(firstId);
            var secondReference = Factory.Get<object>(secondId);

            // Assert
            Assert.That(secondReference, Is.Not.SameAs(firstReference));
        }
    }

    public partial class FactoryTest
    {
        [SetUp]
        public void Init()
        {

        }
    }
}

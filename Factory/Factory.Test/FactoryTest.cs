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
        public void Get_objet_with_id_0()
        {
            // Arrange
            const int id = 0;

            // Act
            var model = Factory.Get<object>(id);

            // Assert
            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void Get_object_with_id_0_twice_same_reference_returned()
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
        public void Get_object_with_id_0_and_0_as_string_same_reference_returned()
        {
            // Arrange
            const int id = 0;
            const string stringId = "0";

            // Act
            var firstReference = Factory.Get<object>(id);
            var secondReference = Factory.Get<object>(stringId);

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

        [Test]
        public void Get_value_holder_with_id_0_force_gc_and_get_value_holder_with_id_0_not_same_reference_returned()
        {
            // Arrange
            const int id = 0;
            const string value = "I am the first reference!";

            // Act
            var reference = Factory.Get<ValueHolder<string>>(id);
            reference.Value = value;
            reference = null;
            GC.Collect();
            reference = Factory.Get<ValueHolder<string>>(id);

            // Assert
            Assert.That(reference.Value, Is.Not.EqualTo(value));
        }

        [Test]
        public void Get_value_holder_with_id_0_store_reference_value_get_value_holder_with_id_0_same_reference_with_same_value_reference_returned()
        {
            // Arrange
            const int id = 0;
            const string value = "I am the value!";

            // Act
            var firstReference = Factory.Get<ValueHolder<string>>(id);
            firstReference.Value = value;
            var secondReference = Factory.Get<ValueHolder<string>>(id);

            // Assert
            Assert.That(firstReference.Value, Is.SameAs(secondReference.Value));
            Assert.That(firstReference.Value, Is.EqualTo(secondReference.Value));
        }

        [Test]
        public void Get_value_holder_with_id_0_store_value_type_value_get_value_holder_with_id_0_same_reference_with_same_value_returned()
        {
            // Arrange
            const int id = 0;
            const long value = long.MaxValue;

            // Act
            var firstReference = Factory.Get<ValueHolder<long>>(id);
            firstReference.Value = value;
            var secondReference = Factory.Get<ValueHolder<long>>(id);

            // Assert
            Assert.That(firstReference.Value, Is.EqualTo(secondReference.Value));
        }

        [Test]
        public void Get_value_holder_with_id_0_store_value_get_value_holder_with_id_1_other_reference_with_other_value_returned()
        {
            // Arrange
            const int firstId = 0;
            const int secondId = 1;
            const string value = "I am the value!";

            // Act
            var firstReference = Factory.Get<ValueHolder<string>>(firstId);
            firstReference.Value = value;
            var secondReference = Factory.Get<ValueHolder<string>>(secondId);

            // Assert
            Assert.That(firstReference.Value, Is.Not.SameAs(secondReference.Value));
            Assert.That(firstReference.Value, Is.Not.EqualTo(secondReference.Value));
        }
    }

    public partial class FactoryTest
    {
        class ValueHolder<T>
        {
            public T Value { get; set; }
        }

        [SetUp]
        public void Init()
        {

        }
    }
}

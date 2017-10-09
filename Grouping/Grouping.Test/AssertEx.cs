using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping.Test
{
    public class AssertEx
    {
        public static void AreEqual<T>(IList<T> expected, IList<T> actual, string propertyName = null)
        {
            if (ReferenceEquals(expected, actual))
                return;

            if (expected.Count == 0 && actual.Count == 0)
                return;
            
            var property = string.IsNullOrWhiteSpace(propertyName) ? null : typeof(T).GetProperty(propertyName);

            Assert.AreEqual(expected.Count, actual.Count);
            if (property == null)
            {
                for (int i = 0; i < expected.Count; ++i)
                    Assert.AreEqual(expected[i], actual[i]);
            }
            else
            {
                for (int i = 0; i < expected.Count; ++i)
                {
                    var expectedValue = property.GetValue(expected[i]);
                    var actualValue = property.GetValue(actual[i]);
                    Assert.AreEqual(expectedValue, actualValue);
                }
            }
        }
    }
}

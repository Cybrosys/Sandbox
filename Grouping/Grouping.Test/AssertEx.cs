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
        public static void AreEqual<T>(IList<T> expected, IList<T> actual)
        {
            if (ReferenceEquals(expected, actual))
                return;

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}

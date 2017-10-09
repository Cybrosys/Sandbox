using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grouping.Test
{
    public abstract class Tests<T>
    {
        protected Random _random = new Random();
        protected List<T> _data;
        protected List<T> _sortedData;

        public Tests()
        {
            PrepareData();
        }

        public abstract void PrepareData();
        public abstract void Constructor_as_list();
        public abstract void Constructor_as_enumerable();
        public abstract void Add();
        public abstract void Insert();
        public abstract void Move();
        public abstract void Remove();
        public abstract void Set();
    }
}

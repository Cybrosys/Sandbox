using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public class Factory
    {
        public static T Get<T>(string id)
            where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}

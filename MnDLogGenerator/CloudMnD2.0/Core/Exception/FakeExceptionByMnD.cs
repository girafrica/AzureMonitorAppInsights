using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMnD2._0.Core.Exception
{
    class FakeExceptionByMnD : System.Exception
    {
        public FakeExceptionByMnD() : base("It's a fake exception by TestApp fake data generator. Don't worry :)")
        {
        }
    }
}

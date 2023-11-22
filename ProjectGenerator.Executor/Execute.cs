using ProjectGenerator.Executor.BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator.Executor
{
    public class Execute
    {
        public Execute() { }

        public void Generate(Configuration configuration)
        {
            var backEndCode = new BackEndCode(configuration);
            backEndCode.Create();
        }
    }
}

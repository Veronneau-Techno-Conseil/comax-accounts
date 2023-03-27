using CommunAxiom.Accounts.BusinessLayer.Generators;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer
{
    public static class ValueGenFactory
    {
        private static Dictionary<string, IValueGen> _generators = new Dictionary<string, IValueGen>();

        static ValueGenFactory() 
        {
            _generators.Add("RandomPWGenerator", new RandomPWGenerator());
            _generators.Add("RandomUNGenerator", new RandomUNGenerator());
        }

        public static string Generate(string generator, string param)
        {
            if (!_generators.TryGetValue(generator, out var gen))
                return null;

            return gen.Generate(param);
        }
    }
}

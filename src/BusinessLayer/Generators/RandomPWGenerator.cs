using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Generators
{
    public class RandomPWGenerator : IValueGen
    {
        static char[] autorized = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM,.éÉàÀ^êÊäÂÄçÇ[]{}-=+*".ToCharArray();
        public string Generate(string parameter)
        {
            var count = int.Parse(parameter);
           
            List<char> chars= new List<char>();
            Random r = new Random();
            
            for(int i = 0; i < count; i++)
            {
                var index = r.Next(0, autorized.Length - 1);
                chars.Add(autorized[index]);
            }

            return string.Concat(chars);
        }
    }
}

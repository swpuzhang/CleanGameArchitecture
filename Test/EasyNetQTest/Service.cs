using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyNetQTest
{
    public interface IService :IDisposable
    {

        void Write();
    }
    public class Service : IService
    {
        public void Dispose()
        {
            
            Console.WriteLine("Dispose");
        }

        public void Write()
        {
            Console.WriteLine("Write");
        }
    }
}

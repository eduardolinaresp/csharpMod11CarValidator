using System;

namespace csharp
{
    class Program
    {
        static void Main(string[] args)
        {
           
           PatenteValidator.filename = "antiguo.csv";
           PatenteValidator.filename = "nuevo.csv";
        
            Console.WriteLine("Formato patente nueva:   AAAA00-1");
            Console.WriteLine("Formato patente antigua: AA0000-1");
            
            Console.WriteLine("Ingresar Patente ");

            string line = Console.ReadLine();

            if (PatenteValidator.ValidarPatente(line))
            {
                Console.WriteLine("OK");
            }else
            {
                Console.WriteLine("NO-OK");
            }
           
        }
    }
}

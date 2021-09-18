using MoleculeSoftware.Packages.State; 
using System;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MoleculeState stateEngine = new MoleculeState();
            stateEngine.Init();

            Console.WriteLine(stateEngine.DatabaseOperation("STORE#>KEY_NICE#>VALUE"));
            Console.WriteLine(stateEngine.DatabaseOperation("RETRIEVE#>KEY_NICE"));
            Console.WriteLine(stateEngine.DatabaseOperation("DELETE#>KEY_NICE"));
            Console.WriteLine(stateEngine.DatabaseOperation("STORE#>_USERID#>155"));
            Console.WriteLine(stateEngine.DatabaseOperation("RETRIEVE#>_USERID")); 
            Console.ReadLine(); 
        }
    }
}

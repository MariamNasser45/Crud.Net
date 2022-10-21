                                //generate name of this permision

namespace Crud.Net.Constant
{
    public static class Permisions
    {
        public static List<string> GeneratePermisionsList(string module) // (string module) : name of module
        {
            return new List<string>()
            {
                // to collect group of string togther there are many ways in c# (string bilder, concatonationn,..)
               // now using stringenterpolation

                $"Permisions.{module}.view",       // {} using to write c# code
                $"Permisions.{module}.Create",    // thes permisions(create , view ,...) changed according to required  
                $"Permisions.{module}.Edit",     // permission.module.name of permision
                $"Permisions.{module}.Delete",

            };
        }
    }
}

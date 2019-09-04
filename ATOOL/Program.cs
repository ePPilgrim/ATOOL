using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

namespace ATOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 0){
                if(args[0] == "set_relation"){
                    if(args.Length != 5)
                    {
                        Console.WriteLine("Wrong number of arguments:");
                        Console.WriteLine("arg[0] - command;");
                        Console.WriteLine("arg[1] - file name that contains all function names that are in APL;");
                        Console.WriteLine("arg[2] - all child objects (including functions) that are called from the corresponding functions from arg[1];");
                        Console.WriteLine("arg[3] - modules IDs for all function in arg[1];");
                        Console.WriteLine("arg[4] - output file name, where the relations between function call as well as their modules IDs are saved.");
                    }
                    else{
                        var funcRelations = new ModulesDependency();
                        funcRelations.SetRelationInFile(args[1], args[2], args[3], args[4]);
                    }

                } else if(args[0] == "csr"){
                    var csr = new ConvertIntoCSRMatrix(args[1]);
                    csr.SetColumnMap(args[2]);
                    csr.CreateCSRMatrix(args[3], args[4], args[5]);

                } else{
                   Console.WriteLine("Only two commands are expected:");
                   Console.WriteLine("1. set_relation - create table of relation between functions. That is what list of functions are called from the some single function.");
                   Console.WriteLine("   This talbe let to know all modules IDs that are depends on the any given function in APL.");
                   Console.WriteLine("   This step is used to create this table and save in the file, that would be used in the next command.");
                   Console.WriteLine("2. add_samples_in_csr_matrix - create samples from the input data and append it to the sparce matrix in csr format.");
                }

            }
            var csr1 = new ConvertIntoCSRMatrix("func.json");
            csr1.SetColumnMap("columns");
            csr1.CreateCSRMatrix("cr", "crlist", "csr_matrix");
/*             var fn1 = new FakeNode(){str = "Hellow JSON1111111111!!!", list = new List<int>{1,2,3}};
            var fn2 = new FakeNode(){str = "Hellow JSON2222222222!!!", list = new List<int>{11,22,33,44}};  
            string str = "[sssss,ddddd,]";
            str = str.TrimStart('[');
            str = str.TrimEnd(']');
            IList<string> nnn =  str.Split(',');
            //int rr = 8; 

            var tR = new Dictionary<int,int>();
            tR.Add(1,22222);
            Console.WriteLine($"{tR[1]}");    */   
        }
    }
}

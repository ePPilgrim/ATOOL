using System;
using System.Collections.Generic;
using System.IO;

namespace ATOOL
{
    class Program
    {
            public class FakeNode : IComparable
            {
                int ii =0;
                public string str = null;

            int IComparable.CompareTo(object obj)
            {
                return this.GetHashCode().CompareTo(obj.GetHashCode());
            }
        }
        static void Main(string[] args)
        {
            using(StreamReader sr1 = new StreamReader("input.txt")) 
            using (StreamReader sr2 = new StreamReader("input2.txt")){
                string str1 = null;
                string str2 = null;
                while((str1 = sr1.ReadLine())!=null && (str2 = sr2.ReadLine())!=null){
                    Console.WriteLine(str1 + " - " + str2);
                }
            }

            IList<int> list = new List<int>{111,234,3455,789};
            using(FileStream fs = File.Create("input1.txt")){
                foreach(var val in list){
                    var bytes = BitConverter.GetBytes(val);
                    fs.Write(bytes,0,bytes.Length);
                }
            }

            using(FileStream fs = File.OpenRead("input1.txt")){
                byte[] buff = {0,0,0,0};
                while(fs.Read(buff,0,4) != 0){
                    Console.WriteLine(BitConverter.ToInt32(buff,0));
                }
            }

            var set = new HashSet<FakeNode>();
            var v1 = new FakeNode {str = "1Hellow Stringgggggggggggg!!!!!!!"};
            var v2 = new FakeNode {str = "2Hellow Stringgggggggggggg!!!!!!!"};
            var v3 = new FakeNode {str = "3Hellow Stringgggggggggggg!!!!!!!"};
            var v4 = new FakeNode {str = "1Hellow Stringgggggggggggg!!!!!!!"};
            set.Add(v1);
            set.Add(v2);
            set.Add(v3);
            set.Add(v4);
            set.Add(v2);
            set.Add(v2);
            set.Add(v3);
            
            foreach(var val in set){
                val.str = "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGG";
                Console.WriteLine(val.str);
                
            }

            string str11 = "Heeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee ew";
            string str22 = "Heeeeeeeeeeeeeeeeeeeeeeeefeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee ew";
            Console.WriteLine("\n\n\n");
            Console.WriteLine(str11);
            Console.WriteLine(str22);
            Console.WriteLine(v1.str);
        }
    }
}

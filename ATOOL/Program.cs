using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

namespace ATOOL
{
    class Program
    {
            public class FakeNode : IComparable
            {
                public string str = null;

                public List<int> list;

            int IComparable.CompareTo(object obj)
            {
                return this.GetHashCode().CompareTo(obj.GetHashCode());
            }
        }
        static void Main(string[] args)
        {
            var fn1 = new FakeNode(){str = "Hellow JSON1111111111!!!", list = new List<int>{1,2,3}};
            var fn2 = new FakeNode(){str = "Hellow JSON2222222222!!!", list = new List<int>{11,22,33,44}};           
        }
    }
}

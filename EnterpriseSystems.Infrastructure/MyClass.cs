using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseSystems.Infrastructure
{
    public class MyClass
    {
        public int Sum(int x1, int x2)
        {
            return x1 + x2;
        }
        public int Sum(List<int> ints)
        {
            return ints.Sum();
        }

        public bool IsPrimaryColor(string color)
        {
            color = color.ToLower();
            return (color == "red" || color == "blue" || color == "yellow");
        }
    }
}

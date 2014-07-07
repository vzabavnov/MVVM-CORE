using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVM.Core.Tests
{
    public static class Helper
    {
        public static int ToInt32(this string s)
        {
            switch(s)
            {
                case "First":
                    return 1;
                case "Second":
                    return 2;
                case "Third":
                    return 3;
                case "Forth":
                    return 4;
                case "Fifth":
                    return 5;
                default:
                    throw new Exception();
            }
        }

        public static string ToText(this int n)
        {
            switch(n)
            {
                case 1:
                    return "First";
                case 2:
                    return "Second";
                case 3:
                    return "Third";
                case 4:
                    return "Forth";
                case 5:
                    return "Fifth";
                default:
                    throw new Exception();
            }
        }
    }
}

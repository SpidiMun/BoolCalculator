using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BoolCalculator
{
    class Table
    {
        string[] values;

        public string[] Values
        {
            get
            {
                return values;
            }
        }

        public Table(Dictionary<string, bool> args)
        {
            values = new string[args.Count];
            int i = 0;
            foreach(var elem in args)
            {
                values[i] = Result(elem.Value);
                i++;
            }
        }

        string Result(bool operand)
        {
            if (operand)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
}

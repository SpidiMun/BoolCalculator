using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BoolCalculator
{
    class Calculation
    {
        Expression expression; // Выражение в ОПН

        string[] operators = { "¬", "∧", "∨", "⇒", "⇔", "|", "↓", "⊕" }; // Доступные операторы

        Dictionary<string, bool> arguments; // Сортированные переменные и их значения (строка)

        List<string> operands; // Сортированные переменные

        int count; // Кол-во переменных

        DataGrid dataGrid;

        string prevOperator;

        public DataGrid Table
        {
            get
            {
                return dataGrid;
            }
        }

        public int Count
        {
            get
            {
                return (int)Math.Pow(2, count);
            }
        }

        public Calculation()
        {
            expression = new Expression();
            arguments = new Dictionary<string, bool>();
            operands = new List<string>();
            dataGrid = new DataGrid();
            prevOperator = string.Empty;
            count = 0;
        }

        public Calculation(string row, DataGrid dg)
        {
            expression = new Expression(row);
            arguments = new Dictionary<string, bool>();
            operands = new List<string>();
            dataGrid = dg;
            dataGrid.Columns.Clear();
            dataGrid.Items.Clear();
            GetArguments();
            prevOperator = string.Empty;
            count = operands.Count;
        }

        void GetArguments()
        {
            operands.Clear();
            bool[] args = new bool[arguments.Count];
            int i = 0;
            foreach(var elem in arguments.Values)
            {
                args[i] = elem;
                i++;
            }
            arguments.Clear();
            foreach(var elem in expression.Variables)
            {
                if (!IsOperator(elem) && !operands.Contains(elem))
                {                    
                    operands.Add(elem);
                }
            }
            operands.Sort();
            i = 0;
            foreach (var elem in operands)
            {
                if (args.Length != 0)
                {
                    arguments.Add(elem, args[i]);
                }             
                else
                {
                    arguments.Add(elem, false);
                }
                i++;
            }
        }

        bool IsOperator(string row)
        {
            if (operators.Contains(row))
            {
                return true;
            }
            else
            {
                return false;
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

        string Operator(string row)
        {
            string op = string.Empty;
            foreach (var symbol in row)
            {
                op += symbol;
                if (operators.Contains(op))
                {
                    return op;
                }
                op = string.Empty;
            }
            return string.Empty;
        }

        bool DoOperation(string operation, bool elem1, bool elem2)
        {
            switch (operation)
            {
                case "∧":
                    return elem1 && elem2;

                case "∨":
                    return elem1 || elem2;

                case "⇒":
                    return !elem1 || elem2;

                case "⇔":
                    return (!elem1 || elem2) && (elem1 || !elem2);

                case "|":
                    return !(elem1 && elem2);

                case "↓":
                    return !(elem1 || elem2);

                case "⊕":
                    return elem1 ^ elem2;
            }
            return true;
        }

        bool Supplement(bool elem)
        {
            return !elem;
        }

        int GetPriority(string operation)
        {
            switch (operation)
            {
                case "(":
                    return (int)Priority.LeftBracket;
                case ")":
                    return (int)Priority.RigthBracket;
                case "∧":
                    return (int)Priority.And;
                case "∨":
                    return (int)Priority.Or;
                case "⇒":
                    return (int)Priority.Implication;
                case "⇔":
                    return (int)Priority.Equivalence;
                case "⊕":
                    return (int)Priority.Xor;
                case "|":
                    return (int)Priority.Shieffer;
                case "↓":
                    return (int)Priority.Pier;
                case "¬":
                    return (int)Priority.Supplement;
                default:
                    return (int)Priority.None;
            }
        }

        public void Calc(int i)
        {
            if(i <= count)
            {
                bool[] args = { false, true };
                foreach(var arg in args)
                {
                    arguments[operands[i - 1]] = arg;
                    Calc(i + 1);
                    if (i == count)
                    {
                        Calculate();
                    }
                }               
            }
        }

        string ProcessData()
        {
            string row = string.Empty;
            foreach (var elem in arguments)
            {
                row += Result(elem.Value);
            }
            return row;
        }

        public void Calculate()
        {
            Stack<string> stringOperands;
            Stack<bool> boolOperands;
            try
            {
                stringOperands = new Stack<string>();
                boolOperands = new Stack<bool>();
                string result;
                foreach (var operand in expression.Variables)
                {                  
                    if (!IsOperator(operand))
                    {
                        stringOperands.Push(operand);
                        boolOperands.Push(arguments[operand]);
                    }
                    else
                    {
                        if (operand == "¬")
                        {
                            string elem;
                            string temp = stringOperands.Pop();
                            boolOperands.Pop();
                            if (!operands.Contains(temp))
                            {
                                elem = "¬(" + temp + ")";
                                operands.Add(elem);

                            }
                            else
                            {
                                elem = "¬" + temp;
                                operands.Add(elem);
                            }                                                 
                            bool exprResult = Supplement(arguments[temp]);
                            stringOperands.Push(elem);
                            boolOperands.Push(exprResult);
                            arguments.Add(elem, exprResult);
                        }
                        else
                        {
                            string elem2 = stringOperands.Pop(),
                                elem1 = stringOperands.Pop(),
                                elem = string.Empty;                            
                            boolOperands.Pop();
                            boolOperands.Pop();
                            bool exprResult = DoOperation(operand, arguments[elem1], arguments[elem2]);

                            if (!operands.Contains(elem1) && GetPriority(operand) < GetPriority(Operator(elem1)) && !elem1.Contains(operand)) // || GetPriority(operand) > GetPriority(prevOperator) && elem1.Contains(operand) / || elem1[0] == '¬'
                            {
                                elem1 = "(" + elem1 + ")";
                                operands.Add(elem1);
                            }
                            if (!operands.Contains(elem2) && GetPriority(operand) < GetPriority(Operator(elem2)) && !elem2.Contains(operand)) // || GetPriority(operand) > GetPriority(prevOperator) && elem2.Contains(operand) / || elem2[0] == '¬'
                            {
                                elem2 = "(" + elem2 + ")";
                                operands.Add(elem2);
                            }                        
                            elem = elem1 + operand + elem2;
                            stringOperands.Push(elem);
                            boolOperands.Push(exprResult);
                            arguments.Add(elem, exprResult);
                            prevOperator = operand;
                        }
                    }
                }
                result = stringOperands.Pop();
                if (result != null)
                {
                    GetColumns();
                    dataGrid.Items.Add(ProcessData());
                    GetArguments();
                    return;
                }
                else
                {
                    dataGrid.Columns.Clear();
                    dataGrid.Items.Clear();
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = "Ошибка!",
                        Binding = new Binding()
                    });
                    dataGrid.Items.Add("Невозможно решить выражение!");
                }
            }
            catch
            {
                dataGrid.Columns.Clear();
                dataGrid.Items.Clear();
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Ошибка!",
                    Binding = new Binding()
                });
                dataGrid.Items.Add("Неверны введены поля для записи выражения!");
            }
        }

        void GetColumns()
        {
            if (dataGrid.Columns.Count != arguments.Count)
            {
                int counter = 0;
                foreach (var elem in arguments)
                {
                    DataGridTextColumn column = new DataGridTextColumn
                    {
                        Header = elem.Key,
                        Binding = new Binding(string.Format("[{0}]", counter))
                    };
                    if (!dataGrid.Columns.Contains(column))
                    {
                        dataGrid.Columns.Add(column);
                        
                    }
                    counter++;
                }
            }           
        }
    }
}

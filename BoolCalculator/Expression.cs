using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoolCalculator
{
    enum Priority
    {
        None = -1,
        LeftBracket = 10,
        RigthBracket,
        And = 2,
        Or,
        Implication,
        Equivalence,
        Xor,
        Shieffer,
        Pier = 7,
        Supplement = 1
    }

    class Expression
    {
        List<string> variables;

        string[] operators = { "(", ")", "¬", "∧", "∨", "⇒", "⇔", "|", "↓", "⊕" };

        public int Args
        {
            get
            {
                int counter = 0;
                foreach(var elem in variables)
                {
                    if (!IsOperator(elem))
                    {
                        counter++;
                    }
                }
                return 0;
            }
        }

        public List<string> Variables
        {
            get
            {
                return variables;
            }
        }

        public Expression()
        {
            variables = new List<string>();
        }

        public Expression(string row)
        {
            variables = new List<string>();
            Read(row);
        }

        public Expression(List<string> list)
        {
            variables = list;
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

        public void Read(string rowExpression)
        {
            Stack<string> operationStack = new Stack<string>();
            string operand = string.Empty;
            string temp = string.Empty;
            try
            {
                for (int i = 0; i < rowExpression.Length; i++)
                {
                    temp = string.Empty;
                    temp += rowExpression[i];
                    if (IsOperator(temp) || temp == " ")
                    {
                        if (operand != string.Empty)
                        {
                            variables.Add(operand);
                        }
                        if (temp == " ")
                        {
                            
                        }
                        else
                        {
                            if (temp == "(")
                            {
                                operationStack.Push(temp);
                            }
                            else
                            {
                                if (temp == ")")
                                {
                                    temp = operationStack.Pop();
                                    while (temp != "(")
                                    {
                                        variables.Add(temp);
                                        temp = operationStack.Pop();
                                    }
                                }
                                else
                                {
                                    if (operationStack.Count > 0)
                                    {                                 
                                        
                                        if (GetPriority(temp) >= GetPriority(operationStack.Peek()))
                                        {
                                            string op = operationStack.Pop();
                                            /*if (operationStack.Peek() == "¬")
                                            {
                                                operationStack.Pop();
                                            }*/
                                            variables.Add(op); // operationStack.Peek()
                                        }
                                    }
                                    operationStack.Push(temp);
                                }
                            }
                        }
                        operand = string.Empty;
                    }
                    else
                    {                       
                        operand += temp;
                        if (i == rowExpression.Length - 1)
                        {
                            variables.Add(operand);
                        }
                    }
                }
                while (operationStack.Count > 0)
                {
                    variables.Add(operationStack.Pop());
                }
            }
            catch
            {
                return;
            }
        }

        public string Write()
        {
            string row = string.Empty;
            foreach (var elem in variables)
            {
                row += elem + " ";
            }
            return row;
        }
    }
}

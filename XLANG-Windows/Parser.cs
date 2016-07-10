using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLANG_Windows
{
    class StringPointer
    {
        int position = -1;
        string txt;
        public object ReadNumber()
        {

            StringBuilder mbuilder = new StringBuilder();
            while (Next())
            {
                if(!char.IsDigit(Current))
                {
                    Prev();
                    return int.Parse(mbuilder.ToString());
                }

                mbuilder.Append(Current);
            }
            Prev();
            return int.Parse(mbuilder.ToString());

        }
        public StringPointer(string txt)
        {
            this.txt = txt;
        }
        public char Current
        {
            get
            {
                return txt[position];
            }
        }
        public void ReadWhitespace()
        {
            while(Next())
            {
                if(!char.IsWhiteSpace(Current))
                {
                    break;
                }
            }
            Prev();
        }
        public string ExpectIdentifier()
        {
            StringBuilder bodyBuilder = new StringBuilder(); //The one thing you'll never be able to do.
            while (Next())
            {
                if(!char.IsLetterOrDigit(Current))
                {
                    Prev();
                    return bodyBuilder.ToString();
                }
                bodyBuilder.Append(Current);
            }
            Prev();
            return bodyBuilder.ToString();
        }
        public bool Expect(char mander)
        {
            if(Next())
            {
                return Current == mander;
            }
            return false;
        }
        public bool Next()
        {
            position++;
            return position < txt.Length;
        }
        public void Prev()
        {
            position--;
        }
    }
    public class XObject
    {
        public XType objType; //Type of object
    }
    public class XType:XObject
    {
        static Dictionary<string, XType> types = new Dictionary<string, XType>();
        public Dictionary<string, XType> Fields = new Dictionary<string, XType>();
        public Dictionary<string, XFunction> Functions = new Dictionary<string, XFunction>();
        public XType(string name)
        {
            if (name != null)
            {
                Name = name;
                types[name] = this;
            }
        }
        public string Name;
    }
    
    public abstract class Variable
    {
        public string Name;
        public string Type;
        public Variable(string name, string type)
        {
            
            Name = name;
            Type = type;
        }
    }
    public class LocalVariable:Variable
    {
        public LocalVariable(string name, string type):base(name,type)
        {

        }
    }
    public class Scope
    {
        public Dictionary<string, Variable> locals = new Dictionary<string, Variable>();
        public Scope parent;
        public Scope()
        {
            
        }
        public Scope(Scope parent)
        {
            this.parent = parent;
        }
    }
    public class XFunctionExternal:XFunction
    {

    }
    public class XFunction:XObject
    {
        public Scope Scope = new Scope();
        public List<Expression> Operations = new List<Expression>();

    }
    public class Expression:XObject
    {
    }
    public class VariableReferenceExpression:Expression
    {
        public Variable variable;
        public VariableReferenceExpression(Variable variable)
        {
            this.variable = variable;
        }
    }
    
    public class CompilationException:Exception
    {
        public CompilationException(string msg):base(msg)
        {

        }
    }
    public class BinaryExpression:Expression
    {
        public char op;
        public Expression left;
        public Expression right;
        
    }
    public class ConstantExpression:Expression
    {
        object val;
        public ConstantExpression(object val)
        {
            this.val = val;
        }
    }
    public class Parser
    {
        StringPointer ptr;
        public void Error(string msg)
        {
            throw new CompilationException(msg);
        }
        public Expression Expression(Expression prev)
        {
            while(ptr.Next())
            {
                switch(ptr.Current)
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '=':
                        {
                            BinaryExpression exp = new BinaryExpression();
                            exp.left = prev;
                            exp.op = ptr.Current;
                            ptr.ReadWhitespace();
                            exp.right = Expression(exp);
                            ptr.ReadWhitespace();
                            Expression next = Expression(exp);
                            
                            return next == null ? exp : next;
                        }
                    case ';':
                        ptr.Prev();
                        return null;
                    default:
                        if(char.IsDigit(ptr.Current))
                        {
                            ptr.Prev();
                            Expression exp =  new ConstantExpression(ptr.ReadNumber());
                            ptr.ReadWhitespace();
                            Expression next = Expression(exp);
                            
                            return next == null ? exp : next;
                        }
                        Error("Unexpected token "+ptr.Current);
                        return null;
                }
            }
            Error("Unexpected End-of-File (EOF).");
            return null;
        }
        public XFunction FunctionBody(XFunction function)
        {
            var scope = function.Scope;
            while (ptr.Next())
            {
                ptr.Prev();
                string id = ptr.ExpectIdentifier();
                ptr.ReadWhitespace();
                ptr.Next();
                Expression exp = null;
                if(char.IsLetter(ptr.Current))
                {
                    //Variable declaration (local scope)
                    ptr.Prev();
                    string varName = ptr.ExpectIdentifier();
                    var local = new LocalVariable(varName, id);
                    if(scope.locals.ContainsKey(varName))
                    {
                        Error("Invalid redeclaration of " + varName + ".");
                    }
                    scope.locals.Add(varName, local);
                    exp = new VariableReferenceExpression(local);
                }
                ptr.ReadWhitespace();
                ptr.Next();
                if(ptr.Current == ';')
                {
                    //End of expression
                    ptr.ReadWhitespace();
                    
                }else
                {
                    //Parse expression
                    ptr.Prev();
                    exp = Expression(exp);
                    function.Operations.Add(exp);
                    ptr.Next();
                    ptr.ReadWhitespace();
                }
            }
            return function;
        }
        public XFunction Main()
        {
            ptr.ReadWhitespace();
            
            return FunctionBody(new XFunction());
        }
        public Parser(string txt)
        {
            //Parse
            ptr = new StringPointer(txt);
            Main();
        }
    }
}

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


namespace iengine
{
    public class ExpressionResearch
    {
        readonly private string _characters;
        readonly private string _initial;
        private string _associative;
        private List<ExpressionResearch> _child = new List<ExpressionResearch>();

        
        public ExpressionResearch()
        {

        }

        public ExpressionResearch(string sentence)   // constructor
        {

            _initial = sentence;
            sentence = sentence.Trim(); 
            if (sentence.Contains("<=>") || sentence.Contains("=>")
                || sentence.Contains("&") || sentence.Contains("~")
                || sentence.Contains("V"))
            {
                SentenceParser(sentence);
            }
            else
            {
                _characters = sentence;
            }
        }

        public void SentenceParser(string sentence)
        {
            int counterBracket = 0;
            int indexOperator = -1;
            
            bool activate = true;
            bool activate2 = true;
            
            sentence.Trim();
            
            for (int i = sentence.Length - 1; i >= 0; i--)
            {
                char c = sentence.ElementAt(i);        // checking different symbol conditions
                if (c == '(')
                {
                    counterBracket++;
                }
                else if (c == ')')
                {
                    counterBracket--;
                }
                else if ((c == '&') && counterBracket == 0 && activate && activate2)
                {
                    indexOperator = i;

                    activate = false;
                    activate2 = false;
                }
                else if (c == '~' && counterBracket == 0 && indexOperator < 0 && activate && activate2)
                {
                    indexOperator = i;
                }
                else if ((c == '<') && counterBracket == 0)
                {
                    indexOperator = i;

                    activate = false;
                    activate2 = false;
                }
                else if ((c == '=' && c + 1 == '>') && counterBracket == 0 && activate2)
                {
                    indexOperator = i;

                    activate = false;
                    activate2 = false;
                }

                else if (c == 'V' && counterBracket == 0 && activate && activate2)
                {
                    indexOperator = i;

                    activate = false;
                    activate2 = false;
                }


            }
            if (indexOperator < 0)
            {
                sentence = sentence.Trim();
                if (sentence.ElementAt(0) == '(' && sentence.ElementAt(sentence.Length - 1) == ')')
                    SentenceParser(sentence.Substring(1, sentence.Length - 2));
            }


            else
            {
                if (sentence.ElementAt(indexOperator) == '<')   // if expression is biconditional
                {

                    string one = sentence.Substring(0, indexOperator);
                    string two = sentence.Substring(indexOperator + 3);

                    one = one.Trim();
                    two = two.Trim();

                    ExpressionResearch childrenOne = new ExpressionResearch(one);
                    ExpressionResearch childrenTwo = new ExpressionResearch(two);

                    _child.Add(childrenOne);
                    _child.Add(childrenTwo);
                    _associative = "<=>";

                }
                else if (sentence.ElementAt(indexOperator) == '~')  // if expression is Negation
                {
                    string one = sentence.Substring(indexOperator + 1);

                    one = one.Trim();

                    ExpressionResearch child = new ExpressionResearch(one);

                    _child.Add(child);
                    _associative = "~";
                }
                else if (sentence.ElementAt(indexOperator) == '&')  // if expression is AND
                {
                    string one = sentence.Substring(0, indexOperator);
                    string two = sentence.Substring(indexOperator + 1);

                    one = one.Trim();
                    two = two.Trim();

                    ExpressionResearch childrenOne = new ExpressionResearch(one);
                    ExpressionResearch childrenTwo = new ExpressionResearch(two);

                    _child.Add(childrenOne);
                    _child.Add(childrenTwo);
                    _associative = "&";

                }
                else if (sentence.ElementAt(indexOperator) == '=')   // if expression is Equal
                {
                    string one = sentence.Substring(0, indexOperator);
                    string two = sentence.Substring(indexOperator + 2);

                    one = one.Trim();
                    two = two.Trim();

                    ExpressionResearch childrenOne = new ExpressionResearch(one);
                    ExpressionResearch childrenTwo = new ExpressionResearch(two);

                    _child.Add(childrenOne);
                    _child.Add(childrenTwo);
                    _associative = "=>";
                }

                else if (sentence.ElementAt(indexOperator) == 'V')     // if expression is OR
                {
                    string one = sentence.Substring(0, indexOperator);
                    string two = sentence.Substring(indexOperator + 2);

                    one = one.Trim();
                    two = two.Trim();

                    ExpressionResearch childrenOne = new ExpressionResearch(one);
                    ExpressionResearch childrenTwo = new ExpressionResearch(two);

                    _child.Add(childrenOne);
                    _child.Add(childrenTwo);
                    _associative = "V";

                }

            }
        }
        public List<ExpressionResearch> Children { get => _child; set => _child = value; } // setting Children property


        public string Symbol { get => _characters; } // readonly symbol property

        public string Connective { get => _associative; set => _associative = value; } // setting Connective property

        public string StringInit { get => _initial; } // readonly original string property

        public void ExecuteInfo()  // Prints the info gathered
        {
            Console.WriteLine("\nInitial String: " + _initial);
            if (_characters is null)
            {
                if (_child.Count > 1)
                {
                    Console.WriteLine("left-side: " + _child[0].StringInit
                                    + " |  Connective: " + _associative 
                                    + " |  right-side: " + _child[1].StringInit + "\n");
                    foreach (ExpressionResearch child in _child)
                        child.ExecuteInfo();
                }
                else if (_child.Count == 1)
                {
                    _child[0].ExecuteInfo();
                }

            }
            else
            {
                Console.WriteLine("Character: " + _characters);
            }
        }

    }
}

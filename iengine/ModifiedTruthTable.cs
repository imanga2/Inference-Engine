using System;
using System.Collections.Generic;
using System.Text;

namespace iengine
{
    class ModifiedTruthTable
    {
        readonly private NewKnowledgeBase _kb;
        readonly private string _q;
        private int _modelCount;

        /*
         * Constructor for Modified Truth Table Algorithm
         */
        public ModifiedTruthTable(NewKnowledgeBase kb, string query)
        {
            _kb = kb;  // local variable of knowledge base 
            _q = query;  //  local variable of query
        }

        /**
        * IS_true function with query and model 
        * @var string query
        * @var Dictionary<string, bool> model
        */
        private bool IS_TRUE(string query, Dictionary<string, bool> model)
        {
            if (model.ContainsKey(query))
            {
                return model[query];
            }
            else
            {
                return false;
            }
        }

    /**
    * IS_true function with Expression and model
    * @var ExpressionResearch expression
    * @var Dictionary<string, bool> model
    */
        private bool IS_TRUE(ExpressionResearch expression, Dictionary<string, bool> model)
        {
            bool decision = true;
            if (expression.Symbol != null)
            {
                decision = IS_TRUE(expression.Symbol, model);
            }
            else
            {
                if (expression.Connective == "&")
                {
                    foreach (ExpressionResearch child in expression.Children)
                    {
                        decision = decision && IS_TRUE(child, model);
                    }
                }
                if (expression.Connective == "<=>")
                {
                    if (IS_TRUE(expression.Children[0], model) == IS_TRUE(expression.Children[1], model))
                    {
                        decision = true;
                    }
                    else
                    {
                        decision = false;
                    }
                }
                if (expression.Connective == "V")
                {
                    decision = false;
                    foreach (ExpressionResearch child in expression.Children)
                    {
                        decision = decision || IS_TRUE(child, model);
                    }
                }
                if (expression.Connective == "=>")
                {
                    if (IS_TRUE(expression.Children[0], model) && !IS_TRUE(expression.Children[1], model))
                    {
                        decision = false;
                    }
                    else
                    {
                        decision = true;
                    }
                }
            }
            return decision;
        }
        private bool IS_TRUE(List<ExpressionResearch> kb, Dictionary<string, bool> model)
        {
            bool finalDecision = true;
            foreach (ExpressionResearch exp in kb)
            {
                finalDecision = finalDecision && IS_TRUE(exp, model);
            }
            return finalDecision;
        }

       /**
       * Calculate model count
       */
        private bool TT_CHECK_ALL(NewKnowledgeBase kb, string query, List<string> symbols, Dictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (IS_TRUE(kb.Expressions, model))
                {
                    if (IS_TRUE(query, model))
                    {
                        _modelCount++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                string str = symbols[0];
                symbols.RemoveAt(0);
                return TT_CHECK_ALL(kb, query, new List<string>(symbols), 
                       Extend(str, true, model)) && TT_CHECK_ALL(kb, query, new List<string>(symbols), 
                       Extend(str, false, model));
            }
        }

     /*
     * Main Algorithm for Modified Truth Table
     * @return boolean
     */
        private bool TT_ENTAILS(NewKnowledgeBase kb, string query)
        {
            List<string> Symbol = GenerateSymbolList(kb.Expressions, query);
            Dictionary<string, bool> model = new Dictionary<string, bool>();
            return TT_CHECK_ALL(kb, query, Symbol, model);
        }

        public void MTTSolution()
        {
            GenerateSymbolList(_kb.Expressions, _q);
            bool decision = TT_ENTAILS(_kb, _q);
            Console.WriteLine(decision ? "YES: " + _modelCount : "NO"); // Prints model count if its Yes
        }

        private List<string> GenerateSymbolList(List<ExpressionResearch> kb, string query)
        {

            List<string> character_List = new List<string>
            {
                query
            };
            foreach (ExpressionResearch exp in kb)
            {
                if (exp.Symbol != null)
                {
                    if (!character_List.Contains(exp.Symbol))
                        character_List.Add(exp.Symbol);
                    }
                else
                {
                    List<string> genList = new List<string>();

                    genList = GenerateSymbolList(exp.Children, null);
                    foreach (string s in genList)
                    {
                        if (!character_List.Contains(s))
                            character_List.Add(s);
                    }
                }
            } 
           return character_List;
       }

        private Dictionary<string, bool> Extend(string key, bool val, Dictionary<string, bool> model)
        {
            Dictionary<string, bool> newModel = new Dictionary<string, bool>(model);
            if (key != null)
            {
                newModel.Add(key, val);
            }

            return newModel;
        }
    }
}

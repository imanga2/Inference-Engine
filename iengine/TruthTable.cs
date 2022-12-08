using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iengine
{
    class TruthTable
    {
        private int _modelCount = 0;
        readonly private KnowledgeBase _kb;
        readonly private string _q;

        /*
         * Constructor for Truth Table Algorithm
         */
        public TruthTable(KnowledgeBase kb, string query)
        {
            _kb = kb;  // local variable of knowledge base
            _q = query;  //  local variable of query
        }

        /**
         * PL_true function with query and model 
         * @var string query
         * @var Dictionary<string, bool> model
         */
        public bool PL_true(string query, Dictionary<string, bool> model)
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
         * PL_true function with knowledge base and model
         * @var KnowledgeBase kb
         * @var Dictionary<string, bool> model
         */
        public bool PL_true(KnowledgeBase kb, Dictionary<string, bool> model)
        {
            bool result = true;
            foreach (Clause c in kb.Clauses)
            {
                bool clauseResult = true;
                if (c.Premise == null)
                {
                    clauseResult = PL_true(c.Conclusion, model);
                }
                else
                {
                    bool true_Premise = true;
                    foreach (string symbol in c.Premise)
                    {
                        true_Premise &=  PL_true(symbol, model);

                    }
                    clauseResult = !(true_Premise && !PL_true(c.Conclusion, model));
                }
                result &= clauseResult;
            }
            return result;

        }

        /**
         * Calculate model count
         */
        private bool TT_Check_All(KnowledgeBase kb, string alpha, 
            List<string> symbols, Dictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PL_true(kb, model))
                {
                    if (PL_true(alpha, model))
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
                string P = symbols[0];

                symbols.RemoveAt(0);
                return TT_Check_All(kb, alpha, new List<string>(symbols), 
                       Extend(P, true, model)) && TT_Check_All(kb, alpha, new List<string>(symbols), 
                       Extend(P, false, model));
            }
        }

        /*
        * Main Algorithm for Truth Table
        * @return boolean
        */
        public bool TT_Entails(KnowledgeBase kb, string alpha)
        {
            List<string> Symbol = kb.GetCharacters();
            Dictionary<string, bool> model = new Dictionary<string, bool>();
            return TT_Check_All(kb, alpha, Symbol, model);
        }

       /*
       * Prints "YES" or "NO"
       * Prints model count when YES
       */
        public void TTSolution() 
        {
            bool isTrue = TT_Entails(_kb, _q);
            Console.WriteLine(isTrue ? "YES: " + _modelCount : "NO"); // Prints model count if its Yes
        }

        /**
         *Initialise model dictionary with each symbol value of true 
         */
        private Dictionary<string, bool> Extend(string key, bool value, Dictionary<string, bool> model)
        {
            Dictionary<String, bool> newModel = new Dictionary<string, bool>(model)
            {
                { key, value }
            };
            return newModel;
        }
    }
}

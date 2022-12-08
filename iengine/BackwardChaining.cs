using System;
using System.Collections.Generic;
using System.Text;

namespace iengine
{
    class BackwardChaining
    {
       readonly private KnowledgeBase _kB;
       readonly private string _q;
       readonly private List<string> _facts;
       readonly private List<string> _checked = new List<string>();

        /*
        * Constructor for Backward Chaining Algorithm
        */
        public BackwardChaining(KnowledgeBase kb, string query)
        {
            _kB = kb;              // local variable of knowledge base
            _q = query;              //  local variable of query
            _facts = kb.GetFacts();   //  local variable of facts
        }

       /*
       * Main Algorithm for Backward Chaining
       * @var string query
       * @return boolean
       */
        public bool PL_BC_Entails(string query)
        {
            Stack<string> agenda = InitialiseAgenda(query);
            List<Clause> containsQuery = new List<Clause>();

            string searching;
            while (agenda.Count != 0)
            {
                searching = agenda.Pop();
                searching = searching.Trim();

                _checked.Add(searching);

                if (!_facts.Contains(searching))
                {

                    foreach (Clause c in _kB.Clauses)
                    {
                        if (c.Conclusion.Contains(searching))
                            containsQuery.Add(c);
                    }

                    if (containsQuery.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (Clause c in containsQuery)
                        {
                            foreach (string s in c.Premise)
                            {

                                if (!_checked.Contains(s))
                                    agenda.Push(s);
                            }
                        }
                    }

                }
            }
            return true;
        }

        /*
       * Prints "YES or "NO"
       * Prints list of symbols when "YES"
       */
        public void BCSolution()
        {
            bool result = PL_BC_Entails(_q);
            _checked.Reverse();

            string path = String.Join(",", _checked.ToArray()); // get comma separated path value

            Console.WriteLine(result ? "YES: " + path.TrimEnd(',') : "NO");  // Prints path if its Yes
        }

        /*
         * Initializes an agenda stack to contain the initial query
         */
        private Stack<string> InitialiseAgenda(string query)
        {
            Stack<string> agenda = new Stack<string>();

            agenda.Push(query);
            return agenda;
        }
    }
}

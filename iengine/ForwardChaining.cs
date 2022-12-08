using System;
using System.Collections.Generic;
using System.Text;

namespace iengine
{
    class ForwardChaining
    {
       readonly private KnowledgeBase _kB;
       readonly private string _q;
       readonly private Dictionary<string, bool> _inferred = new Dictionary<string, bool>();

        /*
         * Constructor for Forward Chaining Algorithm
         */ 
        public ForwardChaining(KnowledgeBase kb, string Query) 
        {
            _kB = kb;   // local variable of knowledge base
            _q = Query;       //  local variable of query
        }

        /*
        * Main Algorithm for Forward Chaining
        * @return boolean
        */
        public bool PL_FC_Entails()
        {
            Dictionary<Clause, int> count = InitialiseCount();
            Dictionary<string, bool> inferred = InitialiseInferred();
            Queue<string> agenda = InitialiseAgenda();

            while (agenda.Count != 0)
            {
                string symbol = agenda.Dequeue();
                if (symbol == _q)
                {
                    _inferred[symbol] = true;
                    return true;
                }

                if (inferred[symbol] == false)
                {
                    _inferred[symbol] = true;
                    foreach (Clause c in _kB.Clauses)
                    {
                        if (c.Premise != null)
                        {
                            if (c.Premise.Contains(symbol))
                            {
                                count[c]--;
                                if (count[c] == 0)
                                    agenda.Enqueue(c.Conclusion);
                            }
                        }

                    }
                }
            }
            return false;
        }

        /*
        * Prints "YES or "NO"
        * Prints list of symbols when YES
        */
        public void FCSolution()  // Function run the Forward Chaining algorithm 
        {

            bool result = PL_FC_Entails();
            string path = "";
            if (result)
            {
                foreach (KeyValuePair<string, bool> entry in _inferred)   
                {
                    if (entry.Value)
                    {
                        path += entry.Key + ",";  // get comma separated path value
                    }
                }
            }
           
            Console.WriteLine(result ? "YES: " + path.TrimEnd(',') : "NO");  // Prints path if its Yes

        }

        /*
         * Determines whether a clause in the knowledge base has no Premise. 
         * If this is case adds clause to agenda. 
         */
        private Queue<string> InitialiseAgenda()
        {
            Queue<string> agenda = new Queue<string>();
            foreach (Clause c in _kB.Clauses)
            {
                if (c.Premise == null)
                    agenda.Enqueue(c.Conclusion);

            }
            return agenda;
        }

        /*
         * Initializes count for a clause with the number
         * of items in its Premise and stores it in a dictionary.
         */
        private Dictionary<Clause, int> InitialiseCount()
        {
            Dictionary<Clause, int> count = new Dictionary<Clause, int>();

            foreach (Clause c in _kB.Clauses)
            {
                if (c.Premise != null)
                    count[c] = c.Premise.Count;
            }
            return count;
        }

        /*
         * Initializes all symbols to be false in a dictionary
         */
        private Dictionary<string, bool> InitialiseInferred()
        {
            Dictionary<string, bool> inferred = new Dictionary<string, bool>();
            List<string> symbols = _kB.GetCharacters();
            foreach (string symbol in symbols)
            {
                inferred[symbol] = false;
            }
            return inferred;
        }
    }
}

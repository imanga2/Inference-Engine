using System;
using System.Collections.Generic;
using System.Text;

namespace iengine
{
    class KnowledgeBase
    {
        readonly private List<Clause> _clauses;

        public List<Clause> Clauses { get => _clauses; }

        public KnowledgeBase(List<Clause> clauses)
        {
            _clauses = clauses;
        }

        public KnowledgeBase()
        {
            throw new System.NotImplementedException();
        }


        /*
         * a Function that returns a list of known facts from the
         * knowledge base
         */
        public List<string> GetFacts()
        {
            List<string> characters = new List<string>();

            foreach (Clause c in _clauses)
                if (c.Premise is null) characters.Add(c.Conclusion);
    
            return characters;
        }

        /*
         * Function for extracting the symbols that are contained 
         * within the knowledge base. 
         */
        public List<string> GetCharacters()
        {
            List<string> characters = new List<string>();
            foreach (Clause c in _clauses)
            {
                //if there is a Premise in clause
                if (c.Premise != null)
                {
                    foreach (string s in c.Premise)
                    {
                        //checks is symbol list already has the symbol
                        if (!characters.Contains(s))
                            characters.Add(s);
                    }
                }
                // checks if symbol list already contains the Consequent
                if (!characters.Contains(c.Conclusion))
                    characters.Add(c.Conclusion);
            }
            return characters;
        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}

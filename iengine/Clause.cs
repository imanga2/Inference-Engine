using System;
using System.Collections.Generic;
using System.Text;

namespace iengine
{
    class Clause
    {
        readonly private List<string> _premise;
        readonly private string _conclusion;

        public Clause(List<string> premise, string conclusion)
        {
            _premise = premise; // local variable of premise
            _conclusion = conclusion; // local variable of conclusion
        }
        public Clause(string conclusion)
        {
            _conclusion = conclusion;  // local variable of conclusion
        }

        public List<string> Premise { get => _premise;}  // premise get property

        public string Conclusion { get => _conclusion; } // conclusion get property

    }
}

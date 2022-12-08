using System;
using System.Collections.Generic;
using System.Text;

namespace iengine
{
    public class NewKnowledgeBase
    {
        readonly private List<ExpressionResearch> _expressions;

        public List<ExpressionResearch> Expressions   
        {
            get => _expressions;  // readonly property
        }
        public NewKnowledgeBase(List<ExpressionResearch> expressions)
        {
            _expressions = expressions;  // local variable of expression
        }

    }
}

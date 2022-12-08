using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iengine
{
    public class DPLL 
    {
        /*
        * The DPLL is essentially a backtracking algorithm, 
        * and that's the main idea behind the recursive calls.
        */
        public DPLL()
        {

        }

        public ExpressionResearch CNFconvertor(ExpressionResearch expression)  // Converts to CNF
        {
            if (expression.Symbol != null)
                return expression;
            
            else if (expression.Connective == "<=>")    // bi-condition check
            {
               
                ExpressionResearch firstChild = new ExpressionResearch(expression.Children[0].StringInit);
                ExpressionResearch secondChild = new ExpressionResearch(expression.Children[1].StringInit);

                List<ExpressionResearch> children = new List<ExpressionResearch>
                {
                    firstChild,
                    secondChild
                };

                ExpressionResearch expNew = new ExpressionResearch
                {
                    Connective = "&"
                };

                expNew.Children = children;
                
                Console.WriteLine("Bi-Implication(<=>) is being removed: \n");
                
                expNew.ExecuteInfo();
                
                return expNew;
            }
            else if (expression.Connective == "=>")
            {
                ExpressionResearch expNew = new ExpressionResearch
                {
                    Connective = "V"
                };
                ExpressionResearch negatefirstChild = new ExpressionResearch
                {
                    Connective = "~"
                };

                ExpressionResearch firstChild = CNFconvertor(expression.Children[0]);
                ExpressionResearch secondChild = CNFconvertor(expression.Children[1]);
                
               
                
                negatefirstChild.Children.Add(firstChild);
                
                expNew.Children.Add(negatefirstChild);
                expNew.Children.Add(secondChild);
                                
                expNew.ExecuteInfo();
                return expNew;
            }
            else if (expression.Connective == "~")
            {
                if (expression.Children[0].Symbol != null)
                {
                    return expression;
                }
                else
                {
                    if (expression.Children[0].Connective == "V")
                    {
                        ExpressionResearch secondChild = new ExpressionResearch
                        {
                            Connective = "~"
                        };
                        ExpressionResearch firstChild = new ExpressionResearch
                        {
                            Connective = "~"
                        };
                        ExpressionResearch expNew = new ExpressionResearch
                        {
                            Connective = "&"
                        };
                        
                        
                        firstChild.Children.Add(expression.Children[0].Children[0]);
                        secondChild.Children.Add(expression.Children[0].Children[1]);
                        
                        expNew.Children.Add(firstChild);
                        expNew.Children.Add(secondChild);
                        
                        return expNew;

                    }
                    else if (expression.Children[0].Connective == "&")
                    {
                        ExpressionResearch secondChild = new ExpressionResearch
                        {
                            Connective = "~"
                        };
                       
                        ExpressionResearch firstChild = new ExpressionResearch
                        {
                            Connective = "~"
                        };
                       
                        ExpressionResearch expNew = new ExpressionResearch
                        {
                            Connective = "V"
                        };
                       
                       
                        
                        firstChild.Children.Add(expression.Children[0].Children[0]);
                        secondChild.Children.Add(expression.Children[0].Children[1]);
                        
                        expNew.Children.Add(firstChild);
                        expNew.Children.Add(secondChild);
                        
                        return expNew;

                    }
                }
            }
            return expression;

        }

        public void DPLLSolution() // Function run the DPLL
        {
            ExpressionResearch exp = new ExpressionResearch("(a&b)<=>c");
            exp = CNFconvertor(exp);
            exp.ExecuteInfo();
        }
    }
}


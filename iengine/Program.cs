using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;


namespace iengine
{
    class Program 
    {
        static string _query;
        static KnowledgeBase _kb;
        static NewKnowledgeBase _nkb;


        /**
         * Main function that reads command line arguments.
         * Calls algorithm based on method name passed in command line arguments.
         */
        static void Main(string[] args)
        {
            if (args.ElementAtOrDefault(0) == null)
            {
                Console.WriteLine("Please provide method name");
                 return;
            }
                
            if (args.ElementAtOrDefault(1) == null)        
            {
                    Console.WriteLine("Please provide text filename");
                    return;      
            }
            

            string filename = args[1];
            string method = args[0].ToLower();
            bool canReadFile = ProblemReader(filename,method);

            if (canReadFile)
            {

                //Determines which algorithm to use for solving problem
                switch (method)
                {
                    case "fc":
                        ForwardChaining fc = new ForwardChaining(_kb, _query);
                        fc.FCSolution();
                        break;

                    case "bc":
                        BackwardChaining bc = new BackwardChaining(_kb, _query);
                        bc.BCSolution();
                        break;

                    case "tt":
                        TruthTable tt = new TruthTable(_kb, _query);
                        tt.TTSolution();
                        break;
                    case "mtt":
                        ModifiedTruthTable mtt = new ModifiedTruthTable(_nkb, _query);
                        mtt.MTTSolution();
                        break;
                    case "dpll":
                        DPLL dpll = new DPLL();
                        dpll.DPLLSolution();
                        break;
                    default:
                        Console.WriteLine("No inference method called " + args[0]);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Cannot read file.Try Again...");
            }
        }

        /**
         * Reads the file for knowledgebase and query
         * Initialises clause object for each premise and conclusion.
         * Builds knowledgebase of clauses.
         */
        public static bool ProblemReader(string filename, string solver)
        {
            List<string> text = new List<string>();
            string[] knowledge = new string[] { };

            //tries to read problem file, if it can't returns false
            try
            {
                StreamReader reader = File.OpenText(filename);
                for (int i = 0; i < 4; i++)
                {
                    text.Add(reader.ReadLine());
                }
                reader.Close();
            }
            catch
            {
                Console.WriteLine("Not able to read file");
                return false;
            }

            //knowledgebase follows the keyword "TELL" 
            if (text[0] == "TELL")
            {
                knowledge = text[1].Split(';').ToArray();
            }

            //query follows the keyword "ASK" 
            if (text[2] == "ASK")
            {
                _query = text[3];
            }


            List<Clause> clauses = new List<Clause>();
           
            // If it is FC or BC, or TT
            if (solver == "fc" || solver == "bc" || solver == "tt")
            {
                foreach (string s in knowledge)
                {
                    if (s.Contains("=>"))
                    {
                        List<string> antecedentSymbols = new List<string>();
                        int index = s.IndexOf("=>");
                        string antecedent = s.Substring(0, index); //get the premise
                        string consequent = s.Substring(index + 2); //get the conclusion
                        consequent = consequent.Trim();


                        string[] splitAntecedent = new string[] { "" };
                        if (antecedent.Contains("&"))
                        {
                            splitAntecedent = antecedent.Split('&');  // spitting by & 
                        }
                        else
                        {
                            splitAntecedent[0] = antecedent;
                        }

                        foreach (string symbol in splitAntecedent)
                        {
                            string ts = symbol.Trim();
                            antecedentSymbols.Add(ts);
                        }

                        //initialise clause object for each premise and conclusion
                        clauses.Add(new Clause(antecedentSymbols, consequent));
                    }
                    else
                    {
                        string consequent = s.Trim();
                        if (consequent.Length > 0) clauses.Add(new Clause(consequent));
                    }
                }

                //Add the clauses to knowledge base 
                _kb = new KnowledgeBase(clauses);

                return true;
            }

            if(solver == "mtt" || solver == "dpll")    // For Modified Truth Table
            {
                List<ExpressionResearch> kb = new List<ExpressionResearch>();

                foreach (string s in knowledge)
                {
                    ExpressionResearch exp = new ExpressionResearch(s);
                    kb.Add(exp);
                }

                _nkb = new NewKnowledgeBase(kb);
                _query = text[3];
                return true;
            }
            else
            {
                Console.WriteLine("Error !!");
                return true;
            }
        }
     }
}


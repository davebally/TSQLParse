using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace TSQLParse
{
    class ProcessFunctions
    {
        public void OnSchemaObjectName(SchemaObjectName currentFragment)
        {
            string sout= "";
            if (currentFragment.SchemaIdentifier != null)
            {
                sout = currentFragment.SchemaIdentifier.Value;
                sout += ".";
               

            }
            sout += currentFragment.BaseIdentifier.Value;
            Console.WriteLine("use of {0} found", sout);



        }

        public void OnDropTableStatement(DropTableStatement currentFragment)
        {
            string sout ="DROP TABLE Found : ";
            foreach(var drp in currentFragment.Objects)
            {
                sout += drp.BaseIdentifier.Value + " ";

            }
            Console.WriteLine(sout);

        }



    }
    class Program
    {

        static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void ParseStmt(string stmt)
        {
            
            using (Stream ms = GenerateStreamFromString(stmt))
            {
                StreamReader sr = new StreamReader(ms);
                IList<ParseError> parseErrors;
                TSql120Parser p = new TSql120Parser(true);
                TSqlFragment frg = p.Parse(sr, out parseErrors);

                ProcessFunctions funcs = new ProcessFunctions();

                try
                {
                    var x = new TSQLParse.TSqlFragmentProcess.TSqlFragmentProcess(frg, funcs);
                }

                catch
                {

                }

            }


        }

        static void Main(string[] args)
        {
            ParseStmt("Select * from MyTable");
            ParseStmt("Drop Table MyTable");
        }
    }
}





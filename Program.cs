using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxJoins
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<string>> list = new List<List<string>>() {
                new List<string>() { "tbl_0", "col0"},
                new List<string>() { "tbl_0", "col1"},
                new List<string>() { "tbl_0", "col2"},
                new List<string>() { "tbl_3", "col0"},
                new List<string>() { "tbl_4", "col1"},
                new List<string>() { "tbl_4", "col6"},
                new List<string>() { "tbl_6", "col6"},
                new List<string>() { "tbl_7", "col1"},
                new List<string>() { "tbl_8", "col8"},
                new List<string>() { "tbl_9", "col9"}
                };

            var columns = list.Select(x => x[1]).ToList();

            list = list.Select(x => x).Where(x => columns.Count(x => x.Contains(x[1])) > 1).ToList();

            //create a dictionary that has a table as the key & columns their own as the Value
            Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>();
            Dictionary<string, HashSet<string>> answer = new Dictionary<string, HashSet<string>>();
            HashSet<string> uniqueJoins = new HashSet<string>();


            foreach (var a in list)
            {
                if (!dictionary.ContainsKey(a[0]))
                {
                    dictionary.Add(a[0], new HashSet<string>() { a[1] });
                    answer.Add(a[0], new HashSet<string>());
                }
                else
                {
                    dictionary[a[0]].Add(a[1]);
                }
            }

            //iterate through the dictionary & compare against are tables to find column overlaps
            foreach (var x in dictionary)
            {
                foreach (var y in dictionary)
                {
                    //Don't compare table to itself
                    if (x.Value != y.Value)
                    {
                        //iterate through the HashSet of Values
                        foreach (var z in y.Value)
                        {
                            //if table x contains any columns that table y owns then inherate the rest of the colums
                            if (x.Value.Contains(z))
                            {
                                //UnionWith to prevent duplicates
                                x.Value.UnionWith(y.Value);
                                y.Value.UnionWith(x.Value);

                                answer[y.Key].Add(x.Key);
                                answer[x.Key].Add(y.Key);

                                //no need to loop through Values of table y since one match has been found
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var x in answer)
            {
                Console.WriteLine();
                Console.WriteLine(x.Key);
                x.Value.OrderBy(x => x);
                Console.WriteLine("Max Join for " + x.Key + " : " + x.Value.Count());

                foreach (var y in x.Value)
                {
                    uniqueJoins.Add(y);
                    Console.WriteLine(y);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Max unique joins: " + uniqueJoins.Count());

            Console.ReadLine();
        }
    }
}

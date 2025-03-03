﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TechJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create two Dictionary vars to hold info for menu and data

            // Top-level menu options
            Dictionary<string, string> actionChoices = new Dictionary<string, string>();
            actionChoices.Add("search", "Search");
            actionChoices.Add("list", "List");

            // Column options
            Dictionary<string, string> columnChoices = new Dictionary<string, string>();
            columnChoices.Add("core competency", "Skill");
            columnChoices.Add("employer", "Employer");
            columnChoices.Add("location", "Location");
            columnChoices.Add("position type", "Position Type");
            columnChoices.Add("all", "All");

            Console.WriteLine("Welcome to LaunchCode's TechJobs App!");

            // Allow user to search/list until they manually quit with ctrl+c //build a exit option from main menu
            while (true)
            {

                string actionChoice = GetUserSelection("View Jobs", actionChoices);

                if (actionChoice.Equals("list"))
                {
                    string columnChoice = GetUserSelection("List", columnChoices);

                    if (columnChoice.Equals("all"))
                    {
                        PrintJobs(JobData.FindAll());
                    }
                    else
                    {
                        List<string> results = (new List<string>(JobData.FindAll(columnChoice)));
                        results.Sort();

                        string title = $"\n*** All {columnChoices[columnChoice]} Values ***";
                        title = formatScripts(title, title.Length + 8);

                        Console.WriteLine( title );
                        Console.WriteLine($" {formatScripts("", title.Length + 2, '-')} ");

                        foreach (string item in results)
                        {                            
                            Console.WriteLine($"| {formatScripts(item, title.Length)} |");
                            Console.WriteLine($" {formatScripts("", title.Length + 2, '-')} ");
                        }

                        Console.WriteLine();
                    }
                }
                else // choice is "search"
                {
                    // How does the user want to search (e.g. by skill or employer)
                    string columnChoice = GetUserSelection("Search", columnChoices);

                    // What is their search term?
                    Console.WriteLine("\nSearch term: ");
                    string searchTerm = Console.ReadLine();

                    List< Dictionary<string, string> > searchResults;

                    // Fetch results
                    if (columnChoice.Equals("all"))
                    {
                        Console.WriteLine("Search all fields not yet implemented.");
                        searchResults = JobData.FindByValue(searchTerm);
                        PrintJobs(searchResults);
                    }
                    else
                    {
                        searchResults = JobData.FindByColumnAndValue(columnChoice, searchTerm);
                        PrintJobs(searchResults);
                    }
                }
            }
        }

        /*
         * Returns the key of the selected item from the choices Dictionary
         */
        private static string GetUserSelection(string choiceHeader, Dictionary<string, string> choices)
        {
            int choiceIdx;
            bool isValidChoice = false;
            string[] choiceKeys = new List<string>(choices.Keys).ToArray();
            //Console.Clear();


            do
            {
                Console.WriteLine("\n" + choiceHeader + " by:");

                for (int j = 0; j < choiceKeys.Length; j++)
                {
                    Console.WriteLine(j + " - " + choices[choiceKeys[j]]);
                }

                string input = Console.ReadLine();
                if (!int.TryParse(input, out choiceIdx)) {
                    Console.Clear();
                    continue;
                }

                if (choiceIdx < 0 || choiceIdx >= choiceKeys.Length)
                {
                    Console.WriteLine("Invalid choices. Try again.");
                }
                else
                {
                    isValidChoice = true;
                }

            } while (!isValidChoice);

            return choiceKeys[choiceIdx];
        }

        private static void PrintJobs(List<Dictionary<string, string>> someJobs)
        {
            
            if (someJobs.Count == 0) {
                string label = "Not Result Found!";

                Console.WriteLine($"{formatScripts("", label.Length + 4, '-')}");
                Console.WriteLine($"| {formatScripts(label)} |");
                Console.WriteLine($"{formatScripts("", label.Length + 4, '-')}");
            }

            Console.WriteLine();
            for (int i = 0; i < someJobs.Count; i++) {

                Console.WriteLine($"*{formatScripts("", 63, '-')}*");

                foreach (KeyValuePair<string, string> job in someJobs[i]) {
                    Console.WriteLine($"| {formatScripts(job.Key)}: {formatScripts(job.Value, 43)}|");
                }
                Console.WriteLine($"*{formatScripts("", 63, '-')}*\n");

            }

        }

        private static string formatScripts(string text, int len = 17, char ch = ' ') {

            if (text.Length >= len)
                return text;

            StringBuilder temp = new StringBuilder(len);
            temp.Append(text);

            for (int i = len - text.Length; i > 0; i--) {
                temp.Append(ch);
            }

            return temp.ToString();
        }
    }
}

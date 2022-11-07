using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Text_Processor_Web_API_Rest.Models;
using Text_Processor_Web_API_Rest.ViewModel;

namespace Text_Processor_Web_API_Rest.Controllers
{
    public class TextController : Controller
    {
        // GET: TextController
        public IActionResult Index(TextViewModel model)
        {
            PopulateDropdownList(model);
            return View(model);
        }

        private static void PopulateDropdownList(TextViewModel model)
        {
            model.SortingOptionsList = SortingOptions.GetAll();
        }

        // GET: TextController/OrderOptions
        public IActionResult OrderOptions(TextViewModel model)
        {
            PopulateDropdownList(model);

            //Deliver the list of Order Options
            return View();
        }

        /// <summary>
        /// Recover all words of the string and sort them based on the received sort option
        /// There are 3 types of sorting (AlphabeticAsc, AlphabeticDesc, LenghtAsc)
        /// Deliver a list of ordered words as a result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: TextController/OrderText
        [HttpPost]
        public IActionResult OrderText(TextViewModel model)
        {
            PopulateDropdownList(model);

            if(CheckSortingConditions(model))
            {
                var sortedList = SortText(model);
                model.SortedText = String.Join(" ", sortedList.ToArray());
            }

            return View(nameof(Index), model);
        }

        private bool CheckSortingConditions(TextViewModel model)
        {
            if (model.SelectedSortingOption == "noSorting" || string.IsNullOrEmpty(model.TextToSort))
            {
                model.SortedText = "Please select a sorting option and/or write text to sort";
                return false;
            }
            return true;
        }

        public List<string> SortText(TextViewModel model)
        {
            if (!CheckSortingConditions(model))
                return new List<string>();

            List<string> listString = model.TextToSort.Split(' ').ToList();
            List<string> sortedList;
            if (model.SelectedSortingOption == SortingOptions.SortingOptionType.AlphabeticAsc.ToString())
            {
                sortedList = listString.OrderBy(x => x).ToList();
            }
            else if (model.SelectedSortingOption == SortingOptions.SortingOptionType.AlphabeticDesc.ToString())
            {
                sortedList = listString.OrderByDescending(x => x).ToList();
            }
            else if (model.SelectedSortingOption == SortingOptions.SortingOptionType.LengthAsc.ToString())
            {
                sortedList = listString.OrderBy(x => x.Length).ToList();
            }
            else
            {
                sortedList = listString;
            }

            return sortedList;
        }

        /// <summary>
        /// Returning a json complex POCO object called TextStatistics with text statistics
        /// of hyphens quantity, word quantity, spaces quantity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: TextController/AnalyzeText
        [HttpPost]
        public IActionResult AnalyzeText(TextViewModel model)
        {
            if(model != null)
            {
                var wordCount = GetWordCount(model.SortedText);
                var hyphensCount = GetCharCount(model, '-');
                var spacesCount = GetCharCount(model, ' ');

                var analyzedText = new AnalyzeText
                {
                    words = wordCount,
                    hyphens = hyphensCount,
                    spaces = spacesCount
                };
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(analyzedText, options);

                Console.WriteLine(jsonString);
                model.AnalyzedText = jsonString;
            }

            return View(nameof(Index), model);
        }

        private static int GetCharCount(TextViewModel model, char ch)
        {
            return model.SortedText.Count(c => c == ch);
        }

        public int GetWordCount(string txtToCount)
        {
            if (string.IsNullOrEmpty(txtToCount))
                return 0;

            string pattern = "\\w+";
            Regex regex = new Regex(pattern);

            int countedWords = regex.Matches(txtToCount).Count;

            return countedWords;
        }
    }
}

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
            model.SortingOptionsList = SortingOptions.GetAll();

            //Deliver the list of Order Options
            return View();
        }

        /// <summary>
        /// Recover all words of the string and sort them based on the received sort option
        /// there are 3 types of sorting (AlphabeticAsc, AlphabeticDesc, LenghtAsc)
        /// Deliver a list of ordered words as a result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: TextController/OrderText
        [HttpPost]
        public IActionResult OrderText(TextViewModel model)
        {
            model.SortingOptionsList = SortingOptions.GetAll();

            var selectedOption = model.SelectedSortingOption;
            List<string> ListString = model.TextToSort.Split(' ').ToList();
            var sortedList = new List<string>();
            sortedList = SortText(selectedOption, ListString);

            model.SortedText = String.Join(" ", sortedList.ToArray());

            return View(nameof(Index), model);
        }

        private static List<string> SortText(string selectedOption, List<string> ListString)
        {
            List<string> sortedList;
            if (selectedOption == SortingOptions.SortingOptionType.AlphabeticAsc.ToString())
            {
                sortedList = ListString.OrderBy(x => x).ToList();
            }
            else if (selectedOption == SortingOptions.SortingOptionType.AlphabeticDesc.ToString())
            {
                sortedList = ListString.OrderByDescending(x => x).ToList();
            }
            else if (selectedOption == SortingOptions.SortingOptionType.LengthAsc.ToString())
            {
                sortedList = ListString.OrderBy(x => x.Length).ToList();
            }
            else
            {
                sortedList = ListString;
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

            return View(nameof(Index), model);
        }

        private static int GetCharCount(TextViewModel model, char ch)
        {
            return model.SortedText.Count(c => c == ch);
        }

        public int GetWordCount(string txtToCount)
        {
            string pattern = "\\w+";
            Regex regex = new Regex(pattern);

            int CountedWords = regex.Matches(txtToCount).Count;

            return CountedWords;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Text_Processor_Web_API_Rest.Models;
using Text_Processor_Web_API_Rest.ViewModel;

namespace Text_Processor_Web_API_Rest.Controllers
{
    public class TextController : Controller
    {
        // GET: TextController
        public IActionResult Index(TextVM model)
        {
            PopulateDropdownList(model);
            return View(model);
        }

        private static void PopulateDropdownList(TextVM model)
        {
            model.SortingOptionsList = SortingOptions.GetAll();
        }

        // GET: TextController/OrderOptions
        public IActionResult OrderOptions(TextVM model)
        {
            model.SortingOptionsList = SortingOptions.GetAll();

            //Deliver the list of Order Options
            return View();
        }

        // POST: TextController/OrderText
        [HttpPost]
        public IActionResult OrderText(TextVM model)
        {
            model.SortingOptionsList = SortingOptions.GetAll();

            var selectedOption = model.SelectedSortingOption;
            List<string> ListString = model.TextToSort.Split(' ').ToList();
            var sortedList = new List<string>();
            sortedList = SortText(selectedOption, ListString);

            model.SortedText = String.Join(" ", sortedList.ToArray());
            //Should recover all words of the string and sort them based on the received sort option
            //there are 3 types of sorting (AlphabeticAsc, AlphabeticDesc, LenghtAsc)
            //Should deliver a list of ordered words as a result
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

        // POST: TextController/AnalyzeText
        [HttpPost]
        public IActionResult AnalyzeText(TextVM model)
        {
            //Returning a json complex POCO object called TextStatistics with text statistics
            //of hyphens quantity, word quantity, spaces quantity
            return View(nameof(Index), model);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Text_Processor_Web_API_Rest_Rest.Controllers
{
    public class TextController : Controller
    {
        // GET: TextController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TextController/OrderOptions
        public ActionResult OrderOptions()
        {
            //Deliver the list of Order Options
            return View();
        }

        // GET: TextController/OrderedText
        public ActionResult OrderedText()
        {
            //Should recover all words of the string and sort them based on the received sort option
            //there are 3 types of sorting (AlphabeticAsc, AlphabeticDesc, LenghtAsc)
            //Should deliver a list of ordered words as a result
            return View();
        }

        // GET: TextController/Statistics
        public ActionResult Statistics(string textToAnalyze)
        {
            //Returning a json complex POCO object called TextStatistics with text statistics
            //of hyphens quantity, word quantity, spaces quantity
            return View();
        }
    }
}

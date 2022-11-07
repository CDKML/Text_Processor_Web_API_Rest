using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Text_Processor_Web_API_Rest.ViewModel
{
    public class TextViewModel
    {
        public List<SelectListItem> SortingOptionsList { get; set; }
        public string SelectedSortingOption { get; set; }

        public string TextToSort { get; set; }
        public string SortedText { get; set; }
        public string AnalyzedText { get; set; }
    }
}

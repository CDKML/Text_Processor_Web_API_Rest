using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Text_Processor_Web_API_Rest.Models
{
    public static class SortingOptions
    {
        public enum SortingOptionType
        {
            AlphabeticAsc,
            AlphabeticDesc,
            LengthAsc
        }

        public static List<SelectListItem> GetAll()
        {
             return new SelectList(Enum.GetValues(typeof(SortingOptionType))).ToList();
        }
    }
}

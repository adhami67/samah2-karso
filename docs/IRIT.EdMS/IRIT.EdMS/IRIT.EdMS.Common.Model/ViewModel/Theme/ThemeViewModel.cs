using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IRIT.EdMS.Common.Model.ViewModel.Theme
{
    public class ThemeViewModel
    {
        public ThemeViewModel()
        {
            _themeList = new List<SelectListItem>
                {
                    new SelectListItem {Value = "Metro", Text = "مترو روشن"},
                    new SelectListItem {Value = "MetroBlack", Text = "مترو تاریک"},
                    new SelectListItem {Value = "Black", Text = "مشکی"},
                    new SelectListItem {Value = "BlueOpal", Text = "آبی روشن"},
                    new SelectListItem {Value = "Bootstrap", Text = "بوت استرپ"},
                    new SelectListItem {Value = "Default", Text = "کندو"},
                    new SelectListItem {Value = "Flat", Text = "مسطح"},
                    new SelectListItem {Value = "HighContrast", Text = "کنتراست بالا"},
                    new SelectListItem {Value = "Moonlight", Text = "مهتاب"},
                    new SelectListItem {Value = "Silver", Text = "نقره ای"},
                    new SelectListItem {Value = "Uniform", Text = "یونیفرم"}
                };
        }

        [Display(Name = @"نمایش افکت(برای مرورگر اینترنت اکسپلورر 8 قابل نمایش نیست)")]
        public bool StartEffect { get; set; }

        [Display(Name = @"پوسته(اسکین)")]
        public string Theme { get; set; }

        private readonly List<SelectListItem> _themeList;
        public List<SelectListItem> ThemeList
        {
            get { return _themeList; }
        }
    }
}

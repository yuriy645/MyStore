using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.StoreEntities
{
    public class BindingModel
    {
        public ColoredProduct ColoredProduct { get; set; } //через это поле указывается путь к различным полям классов в View для привязки модели
        public bool SelectedShowAll { get; set; }

        public List<SelectListItem> OptionSectionList { get; set; }
        public string SelectedSection { get; set; }

        public List<SelectListItem> OptionCategoryList { get; set; }
        public string SelectedCategory { get; set; }

        
        public List<ColoredProduct> ColoredProductsList { get; set; }
        public bool SelectedShowProduct { get; set; }


        public List<SelectListItem> OptionColorsList { get; set; }
        public string SelectedColor { get; set; }


        
    }
}

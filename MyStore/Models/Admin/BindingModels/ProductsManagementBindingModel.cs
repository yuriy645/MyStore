using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.StoreEntities
{
    public class ProductsManagementBindingModel
    {
        public ColoredProduct ColoredProduct { get; set; } //через это поле указывается путь к различным полям классов в View для привязки модели
        public List<ColoredProduct> ColoredProductsList { get; set; }

        [Display(Name = "Показывать все")]
        [UIHint("Boolean")]
        public bool SelectedShowAll { get; set; }

        public List<SelectListItem> OptionSectionList { get; set; }

        [Display(Name = "Раздел")]
        public string SelectedSection { get; set; }

        [Display(Name = "Раздел")]
        public string SelectedSectionCreateProductForm { get; set; }

        public List<SelectListItem> OptionCategoryList { get; set; }

        [Display(Name = "Категория")]
        public string SelectedCategory { get; set; }

        [Display(Name = "Категория")]
        public string SelectedCategoryCreateProductForm { get; set; }

        public string Message { get; set; }

        [Display(Name = "Цвет")]
        public string SelectedColor { get; set; }

        [Display(Name = "Цвет")]
        public string SelectedColorCreateProductForm { get; set; }

        public List<SelectListItem> OptionColorList { get; set; }
        
    }
}

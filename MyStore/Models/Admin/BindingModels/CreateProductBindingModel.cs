using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Admin.BindingModels
{
    public class CreateProductBindingModel
    {
        [UIHint("HiddenInput")]
        public int NewColorId { get; set; }
        [UIHint("HiddenInput")]
        public int NewCategoryId { get; set; }
        [UIHint("HiddenInput")]
        public int NewSectionId { get; set; }


        
        [Display(Name = "Код товара: ")]
        public string ProductCode { get; set; }

        [Display(Name = "Название: ")]
        public string ProductName { get; set; }

        [Display(Name = "Цена: ")]
        public decimal? Price { get; set; }

        [Display(Name = "Показывать на сайте: ")]
        [UIHint("Boolean")]
        public bool ShowProduct { get; set; }

        [Display(Name = "Остаток на складе: ")]
        public int Stock { get; set; }


        public ColoredProduct ColoredProduct { get; set; }

        public List<CharacteristicValue> InputCharacteristicValues { get; set; }

        [UIHint("HiddenInput")]
        public List<CharacteristicValue> AllCharacteristicValues { get; set; }

        //[UIHint("HiddenInput")]
        //public List<int> CharacteristicValuesIds { get; set; }




        public List<CategoryCharacteristic> CategoryCharacteristicsList { get; set; }

        public List<ColoredProduct> ColoredProductsList { get; set; }

        [Display(Name = "Показывать все")]
        [UIHint("Boolean")]
        public bool SelectedShowAll { get; set; }

        public List<SelectListItem> OptionSectionList { get; set; }

        [Display(Name = "Раздел")]
        public string SelectedSection { get; set; }

        [NotAll]
        //[Display(Name = "Раздел")]
        public string SelectedSectionCreateProductForm { get; set; }

        

        public List<SelectListItem> OptionCategoryList { get; set; }

        [Display(Name = "Категория")]
        public string SelectedCategory { get; set; }

        [NotAll]
        //[Display(Name = "Категория")]
        public string SelectedCategoryCreateProductForm { get; set; }

        public string Message { get; set; }

        [Display(Name = "Цвет")]
        public string SelectedColor { get; set; }

        [Dash]
        //[Display(Name = "Цвет")]
        public string SelectedColorCreateProductForm { get; set; }

        public List<SelectListItem> OptionColorList { get; set; }




        public List<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public List<SelectListItem>[] OptionCharacteristicValuesList { get; set; }
        public List<string> Messages { get; set; }
        public List<SelectListItem> OptionColorsList { get; set; }
        public List<Image> Images { get; set; }


        [Display(Name = "Выберите изображения")]
        [UIHint("Files")]
        [ImageFileValidator]
        public List<IFormFile> Files { get; set; }

        public string FileName { get; set; }
    }
}

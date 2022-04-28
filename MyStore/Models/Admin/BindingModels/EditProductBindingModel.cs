using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.StoreEntities
{
    public class EditProductBindingModel
    {
        public List<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public List<SelectListItem>[] OptionCharacteristicValuesList { get; set; }
        public List<string> Messages { get; set; }
        public string InputMessage { get; set; }
        public ColoredProduct ColoredProduct { get; set; } //через это поле указывается путь к различным полям классов в View для привязки модели
        public List<SelectListItem> OptionColorsList { get; set; }
        public string SelectedColor { get; set; }

        public List<Image> Images { get; set; }

        [Display(Name = "Выберите изображения")]
        [UIHint("Files")]
        [ImageFileValidator]
        public IFormFile File { get; set; }

        [Display(Name = "Выберите изображения")]
        [UIHint("Files")]
        [ImageFileValidator]
        public List<IFormFile> Files { get; set; }



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


        public string FileName { get; set; }

        [UIHint("HiddenInput")]
        public int ProductId { get; set; }
        [UIHint("HiddenInput")]
        public int ColorId { get; set; }





    }
}

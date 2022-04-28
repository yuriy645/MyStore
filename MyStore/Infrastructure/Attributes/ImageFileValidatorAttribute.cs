
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class ImageFileValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //object value- объект, кот валидируем
        //ValidationContext validationContext - контекст обработки запроса
        {
            if (value == null) return new ValidationResult(" ");

            if (!(value is List<IFormFile>)) return new ValidationResult("This attribute can only be used on an List<IFormFile>");

            byte[] fileBytes;
            var filesList = (List<IFormFile>)value; // то, что пришло из формы приводим к результату запроса файла
            bool[] rezults = new bool[filesList.Count];

            for (int i = 0; i < filesList.Count; i++)
            {
                using (BinaryReader b = new BinaryReader(filesList[i].OpenReadStream())) // .OpenReadStream() выдаёт поток байт файла из зароса
                {
                    fileBytes = b.ReadBytes((int)filesList[i].Length);// на экземпляре с потоком  вызываем метод получения байтового массива
                                                                // .ReadBytes(..) Считывает указанное количество байтов из текущего потока в байтовый массив и перемещает текущую позицию на это количество байтов вперед. 
                }

                //using (var stream = file.OpenReadStream())//используя поток, загружаем данные
                //{
                //    fileBytes = new byte[stream.Length];// создали массив байт 

                //    for (int i = 0; i < stream.Length; i++)
                //    {
                //        fileBytes[i] = (byte)stream.ReadByte();//записываем эти данные в массив
                //    }
                //}

                // Делаем дополнительные проверки
                var ext = Path.GetExtension(filesList[i].FileName); // проверка расширения
                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":

                        //If the first three bytes don't match the expected, fail the check
                        if (fileBytes[0] != 255 || fileBytes[1] != 216 || fileBytes[2] != 255)

                            return new ValidationResult("Image appears not to be in jpg format. Please try another.");

                        //If the fourth byte doesn't match one of the four expected values, fail the check
                        else if (fileBytes[3] != 219 &&
                        fileBytes[3] != 224 &&
                        fileBytes[3] != 238 &&
                        fileBytes[3] != 225)
                            return new ValidationResult("Image appears not to be in jpg format. Please try another.");
                        
                        else rezults[i] = true; break;
                            //All expected bytes match
                           // return ValidationResult.Success;

                    case ".gif":
                        //If bytes 1-4 and byte 6 aren't as expected, fail the check
                        if (fileBytes[0] != 71 ||
                        fileBytes[1] != 73 ||
                        fileBytes[2] != 70 ||
                        fileBytes[3] != 56 ||
                        fileBytes[5] != 97)
                            return new ValidationResult("Image appears not to be in gif format. Please try another.");
                        //If the fifth byte doesn't match one of the expected values, fail the check
                        else if (fileBytes[4] != 55 && fileBytes[4] != 57)
                            return new ValidationResult("Image appears not to be in gif format. Please try another.");
                        else
                            rezults[i] = true; break;
                            //return ValidationResult.Success;

                    case ".png":
                        if (fileBytes[0] != 137 ||
                        fileBytes[1] != 80 ||
                        fileBytes[2] != 78 ||
                        fileBytes[3] != 71 ||
                        fileBytes[4] != 13 ||
                        fileBytes[5] != 10 ||
                        fileBytes[6] != 26 ||
                        fileBytes[7] != 10)
                            return new ValidationResult("Image appears not to be in png format. Please try another.");
                        else
                         rezults[i] = true; break;
                                //ValidationResult.Success;
                    default:
                        return new ValidationResult($"Расширение {ext} не поддерживается. Допускаются расширения gif, png или jpg.");
                }
                //We shouldn't reach this line – add logging for the error
                //throw new InvalidOperationException("Last line reached in validating the ImageFile");// если вообще всё не подошло 
            }

            int count = 0;
            for (int i = 0; i < rezults.Length; i++)
            {
                if (rezults[i] == true)
                    count++;
            }
            
            if (count == rezults.Length)
                return ValidationResult.Success;
            else
                return new ValidationResult($"Extension is not supported. Please use gif, png, or jpg.");
        }
    }
}

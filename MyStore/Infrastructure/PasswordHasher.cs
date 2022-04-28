using MyStore.Models.Home.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class PasswordHasher
    {
        internal static string HashPassword(string password) //получаем строку -пароль из форм регистрации и входа
        {
            var byted = Encoding.UTF8.GetBytes(password); //пароль разбираем на байты, получаем массив байтов. Пока ничего не зашифровано
                                                          // получаем наш пароль в новой форме представления - из строки в байтовое представление в формате UTF8
            var sha1 = SHA1CryptoServiceProvider.Create(); //создаём объект, кот будет шифровать. SHA1CryptoServiceProvider- далеко не самый стойкий, но тут подойдет
            var hashedBytes = sha1.ComputeHash(byted);     //шифровальщик, вычисли хэш для э набора байтов
            return Encoding.UTF8.GetString(hashedBytes);   //из зашифрованного набора байт получаем строку. Она и будет храниться в БД
        }

        internal static bool IsCorrectPassword(SignInBindingModel signInBindingModel, string password)
        {
            var passwordHash = HashPassword(password); //сверяет хэш тог, что введено пользователем
            return passwordHash == signInBindingModel.Pass;   //с тем, что хранится в БД
        }
    }
}

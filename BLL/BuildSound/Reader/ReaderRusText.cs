using System;
using System.Collections.Generic;
using BLL.Entities.Params.BuildSoundParam;
using BLL.Infastructure;
using BLL.Entities;
using System.Text.RegularExpressions;
using DAL.UnitOfWork;
using System.IO;

namespace BLL.BuildSound.Reader
{
    public class ReaderRusText : IReaderText
    {
        private IUnitOfWork _db;
        public ReaderRusText(IUnitOfWork db)
        {
            _db = db;
        }

        public ResultDetails<IEnumerable<string>> Read(ReaderParams param)
        {
            bool textHaveLetNum = ValidateText(param.Text);
            if (!textHaveLetNum)
                return new ResultDetails<IEnumerable<string>>(false, "text should have minimum 1 letter or number", "Text", null);

            int countLetters = _db.Letters.Count(p => p.User.UserName.Equals(param.UserName));
            if(countLetters != 46)
                return new ResultDetails<IEnumerable<string>>(false, "user not have all recordered russian letters(31 lettters)", "", null);


            ICollection<string> srcs = new List<string>();
            string text = param.Text.ToLower();
            string sitePath = AppDomain.CurrentDomain.BaseDirectory;
                   
            //мы помещаем пустую строку в текст, чтобы на последней итерации цикла
            //мы могли проиницилизоровать переменную currentLetter 
            text = text.Insert(text.Length, " ");

            for (int i = 0; i < text.Length - 1; i++)
            {
                string letterSrc = String.Empty;
                string currentLetter = text[i].ToString();
                string nextLetter = text[i + 1].ToString();

                if (nextLetter == "ь")
                {
                    //если следущая буква ь знак, то добовляем src предыдущей буквы с ь знаком
                    letterSrc = $@"{sitePath}Letters\{param.UserName}\{param.Language.ToString()}\{currentLetter}ь.wav";
                    //инкрементируем счётчик, чтобы пропустить ь знак
                    i++;
                }
                else
                {
                    if (IsPunctuation(currentLetter))
                        letterSrc = currentLetter;
                    else
                        letterSrc = $@"{sitePath}Letters\{param.UserName}\{param.Language.ToString()}\{currentLetter}.wav";
                }
         
                srcs.Add(letterSrc);
            }

            return new ResultDetails<IEnumerable<string>>(true, "", "", srcs);
        }

        private bool IsPunctuation(string symbol)
        {
            char[] punctuatuon = new char[] { '.', ',', ' ', '?', '-', '!', '\'' };
            int index = symbol.IndexOfAny(punctuatuon);
            return index != -1;
        }

        private bool ValidateText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return false;

            string pattern = @"[а-яА-Я0-9]+";
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }
    }
}

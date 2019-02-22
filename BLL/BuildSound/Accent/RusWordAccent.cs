using DAL.Entities;
using DAL.UnitOfWork;
using System;
using System.Linq;
using System.Text;

namespace BLL.BuildSound.Accent
{
    public class RusWordAccent : IWordAccent
    {
        private IUnitOfWork _db;
        public RusWordAccent(IUnitOfWork db)
        {
            _db = db;
        }

        public string InsertAccents(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text can not be null");

            string[] punctuations = new string[] { " ", ",", ".", "!", "?", ";", ":", "-" };
            char[] glasn = new char[] { 'а', 'и', 'у', 'е', 'ы', 'о', 'э', 'я', 'ю' };
            StringBuilder changedText = new StringBuilder(text.ToLower());

            //проверка на пунктуацию последнего символа, если его нет, то добовляем ".", чтобы найти последнее слово в тексте
            bool isPun = punctuations.Contains(changedText[changedText.Length - 1].ToString());
            if (!isPun)
                changedText.Insert(changedText.Length, ".");

           
            int count = 0; //счётчик букв в слове
            for (int i = 0; i < changedText.Length; i++)
            {
                if (punctuations.Contains(changedText[i].ToString()) && count > 0)
                {
                    string word = changedText.ToString().Substring(i - count, count);
                    WordAccent wordAccent = _db.WordAccents.FindWithExpressionsTree(p => p.Word.Equals(word)).FirstOrDefault();

                    if (wordAccent != null)
                    {
                        changedText.Remove(i - count, count); //удалили слово
                        changedText.Insert(i - count, wordAccent.Accent); //вставили этоже слово с ударением 
                    }

                    else //если в словаре нет искомого слова, то ставим ударение на 1 гласную букву
                    {
                        int glIndex = word.IndexOfAny(glasn);
                        if (glIndex != -1)
                        {
                            word = word.Insert(glIndex, "'");
                            changedText.Remove(i - count, count);
                            changedText.Insert(i - count, word);
                        }
                    }
                    i++; //инкрементируем i, потомучто i указывает на предыдущую букву, т.к мы добавили к слову ударение
                    count = 0;
                }

                else
                {
                    count++;
                }
            }

            return changedText.ToString();
        }
    }
}

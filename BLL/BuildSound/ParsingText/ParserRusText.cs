using System;
using System.Text;

namespace BLL.BuildSound.ParsingText
{
    public class ParserRusText : IParserText
    {
        public string ClearExcessPunctuation(string text)
        {
            //TODO: доделать удаление лишних пунткционных знаков
            throw new NotImplementedException();
        }

        public string NumbersParse(string text)
        {
            //TODO: доделать синтез цифр в текст
            if (text == null)
                throw new ArgumentNullException("text", "text can not be null");

            throw new NotImplementedException();
        }

        public string PhoneticParse(string text, char accentSymbol)
        {
            if (text == null)
                throw new ArgumentNullException("text", "text can not be null");

            StringBuilder analyzedText = new StringBuilder(text.ToLower());


            for (int i = 0; i < analyzedText.Length; i++)
            {

                bool isSogl = false;
                int indexPrevSymbol = i - 1;
                if (i > 0)
                {   
                    //Если пред. символ это ударение, то индекс пред. буквы indexPrevSymbol - 1
                    if (i > 1 && analyzedText[i - 1].ToString() == accentSymbol.ToString())
                        indexPrevSymbol--;
                    
                    isSogl = IsSogl(analyzedText[indexPrevSymbol]);
                }


                //если пред. буква согласная и текущая буква гласная, то применяем правила
                if (isSogl)
                {

                    string changedLetter = String.Empty;
                    switch (analyzedText[i])
                    {
                        case 'я':
                            {
                                //добавляем после согласной буквы ь знак, чтобы показать что она мягкая
                                InsertSoftSymbol(analyzedText, i, accentSymbol);

                                //если гласная буква безударная, то изменяем ее на э, иначе на а
                                changedLetter = analyzedText[i] != accentSymbol ? "э" : "а";

                                //индекс инкрементируется т.к мы добавили ь знак, если бы этого не было, то текущий индекс стоял на ь знаке , а не на следующей букве 
                                i++;
                            }
                            break;

                        case 'ю':
                            {
                                InsertSoftSymbol(analyzedText, i, accentSymbol);
                                changedLetter = "у";
                                i++;
                            }
                            break;

                        case 'е':
                            {
                                InsertSoftSymbol(analyzedText, i, accentSymbol);
                                changedLetter = analyzedText[i] != accentSymbol ? "и" : "э";
                                i++;
                            }
                            break;

                        case 'ё':
                            {
                                changedLetter = "о";
                            }
                            break;

                        case 'э':
                            {
                                changedLetter = analyzedText[i - indexPrevSymbol] != accentSymbol ? "и" : String.Empty;
                            }
                            break;

                        case 'о':
                            {
                                changedLetter = analyzedText[i - indexPrevSymbol] != accentSymbol ? "а" : String.Empty;
                            }
                            break;

                        case 'и':
                            {
                                InsertSoftSymbol(analyzedText, i, accentSymbol);
                                i++;
                            }
                            break;
                    }

                    if (!String.IsNullOrEmpty(changedLetter))
                    {
                        string currentLetter = analyzedText[i].ToString();
                        analyzedText.Replace(currentLetter, changedLetter, i, 1);
                    }
                }
            }

            return analyzedText.ToString();
        }

        private void InsertSoftSymbol(StringBuilder analyzedText, int currentIndex, char accentSymbol)
        {
            //индекс предыдущей буквы
            int indexPrevLetter = currentIndex - 1;

            //если предыдущий символ ударение, то  индекс пред. буквы indexPrevLetter - 1
            if (currentIndex >= 1 && analyzedText[indexPrevLetter].ToString() == accentSymbol.ToString())
                indexPrevLetter--;

            //если текущая буква не является только твёрдой
            if (!IsLetterOnlySolid(analyzedText[indexPrevLetter])) 
               analyzedText.Insert(indexPrevLetter + 1, "ь");
        }

        private bool IsSogl(char letter)
        {
            char[] sogl = new char[] { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' };
            int index = letter.ToString().IndexOfAny(sogl);
            return index != -1;
        }

        private bool IsLetterOnlySolid(char letter)
        {
            char[] solidLetters = new char[] { 'ц', 'ж', 'ш' };
            int index = letter.ToString().IndexOfAny(solidLetters);
            return index != -1;
        }
    }
}

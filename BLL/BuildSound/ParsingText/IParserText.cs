namespace BLL.BuildSound.ParsingText
{
    public interface IParserText
    {
        string PhoneticParse(string text, char accentSymbol);
        string NumbersParse(string text);
        string ClearExcessPunctuation(string text);
    }
}

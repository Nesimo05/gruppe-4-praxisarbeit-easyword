public class Word
{
    public string GermanWord { get; set; }
    public string EnglishWord { get; set; }
    public bool Learned { get; set; }

    public Word(string germanWord, string englishWord)
    {
        GermanWord = germanWord;
        EnglishWord = englishWord;
        Learned = false;
    }
}

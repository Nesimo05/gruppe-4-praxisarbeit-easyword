using System;
using System.Collections.Generic;
using System.Linq;

public class WordList
{
    private List<Word> words;
    private Random random = new Random();

    public WordList()
    {
        words = new List<Word>();
    }

    public void AddWord(Word word)
    {
        words.Add(word);
    }

    public void RemoveWord(Word word) {
        words.Remove(word);
    }

    public Word GetRandomUnlearnedWord()
    {
        var unlearnedWords = words.Where(w => !w.Learned).ToList();
        if (unlearnedWords.Count == 0)
            return null;

        int index = random.Next(unlearnedWords.Count);
        return unlearnedWords[index];
    }

    public void MarkAsLearned(Word word)
    {
        var foundWord = words.FirstOrDefault(w => w.GermanWord == word.GermanWord && w.EnglishWord == word.EnglishWord);
        if (foundWord != null)
        {
            foundWord.Learned = true;
        }
    }

    internal void Reset()
    {
        words.Clear();
    }

    internal bool AllWordsLearned()
    {

        if (words.Count == 0) return false;
        throw new NotImplementedException();
    }

    internal bool IsEmpty()
    {
        if (words != null && words.Count > 0)
        {
            return false; // Die Datei ist nicht leer
        }
        else
        {
            return true; // Die Datei ist leer
        }
    }

    internal Word GetRandomWord()
    {
        throw new NotImplementedException();
    }
}

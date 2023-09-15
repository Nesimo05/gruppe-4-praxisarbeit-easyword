using System;
using System.IO;

namespace EasyWord4
{
    public class CsvWordImporter
    {
        private readonly string _filePath;
        private readonly WordList _wordList;

        public CsvWordImporter(string filePath, WordList wordList)
        {
            _filePath = filePath;
            _wordList = wordList;
        }

        public void Import()
        {
            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        if (values.Length != 2)
                        {
                            throw new Exception("Ungültiges Format in der CSV-Datei. Jede Zeile sollte genau zwei Werte enthalten.");
                        }

                        var germanWord = values[0];
                        var englishWord = values[1];

                        _wordList.AddWord(new Word(germanWord, englishWord));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Importieren der CSV-Datei: {ex.Message}");
            }
        }
    }
}

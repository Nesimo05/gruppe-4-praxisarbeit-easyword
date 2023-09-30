using EasyWord4;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyWord
{
    public partial class EasyWordWindow : Window
    {
        private WordList wordList;

        // ... (restlicher Code bleibt unverändert)

        private void InitializeWordList()
        {
            wordList = new WordList();

            // Erstellen Sie einen OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Setzen Sie Filter für Dateitypen
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";

            // Zeigen Sie den OpenFileDialog an, indem Sie die ShowDialog-Methode aufrufen
            Nullable<bool> result = dlg.ShowDialog();

            // Holen Sie sich den ausgewählten Dateinamen und zeigen Sie ihn in einer MessageBox an
            if (result == true)
            {
                string filename = dlg.FileName;
                var csvImporter = new CsvWordImporter(filename, wordList);

                // Fragen Sie den Benutzer nach Importoptionen
                MessageBoxResult importChoice = MessageBox.Show("Sollen die Wörter zu den vorhandenen hinzugefügt werden?",
                                                                "Importoptionen",
                                                                MessageBoxButton.YesNoCancel);

                if (importChoice == MessageBoxResult.Yes)
                {
                    csvImporter.Import(); // Füge zu den vorhandenen Wörtern hinzu
                }
                else if (importChoice == MessageBoxResult.No)
                {
                    wordList.Clear(); // Überschreibe die vorhandenen Wörter
                    csvImporter.Import();
                }
                // Falls der Benutzer Abbrechen wählt, passiert nichts
            }
        }

        // ... (restlicher Code bleibt unverändert)

        private void SaveDataOnExit()
        {
            // Restlicher Code bleibt unverändert

            // Speichern der vorhandenen Wortlisten und des Lernstatus
            string wordListJson = JsonConvert.SerializeObject(wordList);
            File.WriteAllText("wordList.json", wordListJson);

            // Speichern der Benutzereinstellungen
            SaveUserSettings();

            // Speichern der Groß-/Kleinschreibungsoption
            SaveCaseOption();
        }

        private void SaveUserSettings()
        {
            // Implementiere deine Logik hier
        }

        private void SaveCaseOption()
        {
            // Implementiere deine Logik hier
        }

        // ... (restlicher Code bleibt unverändert)

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDataOnExit();
        }
    }
}

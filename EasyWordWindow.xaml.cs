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
    /// <summary>
    /// Interaktionslogik für EasyWordWindow.xaml
    /// </summary>
    public partial class EasyWordWindow : Window
    {
        private WordList wordList;
        private Word currentWord;
        private UserSettings userSettings; // Benutzereinstellungen
        private CaseOption caseOption; // Groß-/Kleinschreibungsoption
        private AppInfo appInfo; // Softwareinformationen

        public EasyWordWindow()
        {
            InitializeComponent();
            InitializeWordList();
            DisplayRandomWord();
            LoadUserSettings(); // Laden der Benutzereinstellungen beim Start
            LoadCaseOption(); // Laden der Groß-/Kleinschreibungsoption beim Start
            InitializeAppInfo(); // Initialisieren der Softwareinformationen
        }

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
                csvImporter.Import();
            }
        }


        private void LoadUserSettings()
        {
            try
            {
                if (File.Exists("userSettings.json"))
                {
                    string json = File.ReadAllText("userSettings.json");
                    userSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                }
                else
                {
                    userSettings = new UserSettings();
                }

                // Aktualisieren der Benutzeroberfläche basierend auf den geladenen Einstellungen
                IgnoreCaseCheckBox.IsChecked = userSettings.IgnoreCase;
                LanguageComboBox.SelectedValue = userSettings.Language;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Benutzereinstellungen: " + ex.Message);
            }
        }

        private void SaveUserSettings()
        {
            try
            {
                // Aktualisieren der Benutzereinstellungen basierend auf der Benutzeroberfläche
                userSettings.IgnoreCase = IgnoreCaseCheckBox.IsChecked ?? false;
                userSettings.Language = LanguageComboBox.SelectedValue.ToString();

                string json = JsonConvert.SerializeObject(userSettings);
                File.WriteAllText("userSettings.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern der Benutzereinstellungen: " + ex.Message);
            }
        }

        private void LoadCaseOption()
        {
            try
            {
                if (File.Exists("caseOption.json"))
                {
                    string json = File.ReadAllText("caseOption.json");
                    caseOption = JsonConvert.DeserializeObject<CaseOption>(json);
                }
                else
                {
                    caseOption = new CaseOption();
                }

                // Aktualisieren der Benutzeroberfläche basierend auf den geladenen Einstellungen
                IgnoreCaseCheckBox.IsChecked = caseOption.IgnoreCase;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Groß-/Kleinschreibungsoption: " + ex.Message);
            }
        }

        private void SaveCaseOption()
        {
            try
            {
                // Aktualisieren der Groß-/Kleinschreibungsoption basierend auf der Benutzeroberfläche
                caseOption.IgnoreCase = IgnoreCaseCheckBox.IsChecked ?? false;

                string json = JsonConvert.SerializeObject(caseOption);
                File.WriteAllText("caseOption.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern der Groß-/Kleinschreibungsoption: " + ex.Message);
            }
        }

        private void InitializeAppInfo()
        {
            appInfo = new AppInfo();
            DisplayAppInfo();
        }

        private void DisplayAppInfo()
        {
            AppInfoLabel.Text = $"{appInfo.SoftwareName} Version {appInfo.Version} entwickelt von {appInfo.Developer}";
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            // Stellen Sie sicher, dass das aktuelle Wort und die WordList initialisiert sind
            if (currentWord == null || wordList == null)
            {
                MessageBox.Show("Die Wortliste ist nicht korrekt initialisiert.");
                return;
            }

            // Holen Sie die eingegebene Übersetzung aus dem TextBox-Steuerelement
            string userTranslation = EnglishTranslationTextBox.Text.Trim();

            if (string.IsNullOrEmpty(userTranslation))
            {
                MessageBox.Show("Bitte geben Sie eine Übersetzung ein.");
                return;
            }

            // Überprüfen Sie die Übersetzung des Benutzers mit der richtigen Übersetzung
            bool isTranslationCorrect = string.Equals(userTranslation, currentWord.EnglishWord, StringComparison.OrdinalIgnoreCase);

            // Aktualisieren Sie das Feedback basierend auf der Überprüfung
            if (isTranslationCorrect)
            {
                FeedbackLabel.Foreground = Brushes.Green;
                FeedbackLabel.Text = "Richtig! Weitermachen.";
                // Markieren Sie das Wort als gelernt und entfernen Sie es aus der WordList
                currentWord.Learned = true;
                wordList.RemoveWord(currentWord);
            }
            else
            {
                FeedbackLabel.Foreground = Brushes.Red;
                FeedbackLabel.Text = "Falsch! Versuchen Sie es erneut.";
            }

            // Löschen Sie den Text im TextBox-Steuerelement
            EnglishTranslationTextBox.Clear();

            // Prüfen Sie, ob alle Wörter gelernt wurden, und starten Sie gegebenenfalls erneut
            if (wordList.AllWordsLearned())
            {
                MessageBox.Show("Herzlichen Glückwunsch! Sie haben alle Wörter gelernt.");
                // Hier können Sie die Wortliste neu mischen oder zurücksetzen, um erneut zu beginnen
                wordList.Reset();
            }
            else
            {
                DisplayRandomWord(); // Zeigen Sie das nächste zufällige Wort an
            }
        }

        private void DisplayRandomWord()
        {
            // Überprüfen Sie, ob noch Wörter in der Liste vorhanden sind
            if (wordList.IsEmpty())
            {
                MessageBox.Show("Es gibt keine Wörter mehr in der Liste.");
                return;
            }

            // Holen Sie ein zufälliges Wort aus der Liste
            currentWord = wordList.GetRandomWord();

            // Anzeigen des deutschen Wortes zur Übersetzung
            GermanWordLabel.Text = currentWord.GermanWord;

            // Löschen Sie das vorherige Feedback
            FeedbackLabel.Text = string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Aktualisieren und speichern Sie die Benutzereinstellungen
            SaveUserSettings();
            MessageBox.Show("Benutzereinstellungen wurden erfolgreich gespeichert.");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Laden und aktualisieren Sie die Benutzereinstellungen
            LoadUserSettings();
            MessageBox.Show("Benutzereinstellungen wurden erfolgreich geladen.");
        }


        private void IgnoreCaseCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Aktualisieren Sie die Groß-/Kleinschreibungsoption basierend auf der CheckBox
            if (IgnoreCaseCheckBox.IsChecked.HasValue)
            {
                bool ignoreCase = IgnoreCaseCheckBox.IsChecked.Value;
                // Speichern Sie die Groß-/Kleinschreibungsoption
                caseOption.IgnoreCase = ignoreCase;
                SaveCaseOption();
            }
        }

    }
}



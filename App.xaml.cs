using System;
using System.Windows;
using EasyWord;
using EasyWord4;

namespace EasyWord4
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            // Stellen Sie sicher, dass die EasyWordWindow-Instanz korrekt initialisiert wird.
            EasyWordWindow mainWindow = new EasyWordWindow();

            // Überprüfen, ob mainWindow oder ein Mitglied davon null ist.
            if (mainWindow != null)
            {
                App app = new App();
                app.Run(mainWindow);
            }
            else
            {
                Console.WriteLine("EasyWordWindow instance is null.");
            }
        }
    }
}

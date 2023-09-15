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
            App app = new App();
            EasyWordWindow mainWindow = new EasyWordWindow();
            app.Run(mainWindow);
        }
    }
}

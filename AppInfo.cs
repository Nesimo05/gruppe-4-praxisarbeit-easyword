using System;

public class AppInfo
{
    public string SoftwareName { get; set; }
    public string Version { get; set; }
    public string Developer { get; set; }

    public AppInfo()
    {
        SoftwareName = "EasyWord";
        Version = "1.0"; // Standardwerte für die Softwareinformationen
        Developer = "Anthony David Nesim Sebastian ";
    }
}
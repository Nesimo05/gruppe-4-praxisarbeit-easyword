using System;

public class UserSettings
{
    public bool IgnoreCase { get; set; }
    public string Language { get; set; }

    public UserSettings()
    {
        IgnoreCase = false;
        Language = "English"; // Standardwerte für die Einstellungen
    }
}

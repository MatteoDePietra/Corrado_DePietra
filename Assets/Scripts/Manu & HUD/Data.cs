[System.Serializable]
public class Data
{
    internal float masterVolume;
    internal float musicVolume;
    internal float record;
    internal Data (MainMenu mainMenu, float record)
    {
        this.masterVolume = mainMenu.masterVolume;
        this.musicVolume = mainMenu.musicVolume;
        this.record = record;
    }

    internal Data()
    {
        masterVolume = 1;
        musicVolume = 1;
        record = 0;
    }
}
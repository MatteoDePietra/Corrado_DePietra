[System.Serializable]
public class Data
{
    internal float masterVolume;
    internal float musicVolume;
    internal int lastLevel;
    internal int Coin;
    internal float recordTime;

    internal Data (MainMenu mainMenu)
    {
        masterVolume = mainMenu.masterVolume;
        musicVolume = mainMenu.musicVolume;
    }
}
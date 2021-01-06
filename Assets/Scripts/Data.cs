using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public float masterVolume;
    public float musicVolume;
    public int lastLevel;
    public int Coin;
    public float recordTime;

    public Data (MainMenu mainMenu)
    {
        masterVolume = mainMenu.masterVolume;
        musicVolume = mainMenu.musicVolume;
    }
}
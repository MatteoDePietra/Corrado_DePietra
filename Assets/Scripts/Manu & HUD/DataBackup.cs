using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBackup : MonoBehaviour
{
    internal static float masterVolume;
    internal static float musicVolume;
    internal static float record;
    private static MainMenu mainMenu;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        mainMenu = GameObject.Find("Menu").GetComponent<MainMenu>();
        
        Data data = SaveSystem.Load();

        masterVolume = data.masterVolume; 
        musicVolume = data.musicVolume;
        record = data.record;
    }

    internal static void Save()
    {
        SaveSystem.Save(mainMenu, record);
    }
}

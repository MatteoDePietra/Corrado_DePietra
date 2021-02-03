using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Animator crossFade = null;

    [SerializeField]
    private CanvasGroup canvasGroup = null;
    private float transitionTimeCF = 1.5f;
    public static int playerChoosen;
    private bool activeMainMenu;
    private bool activeCharacterChooser;
    private bool activeOptionMenu;
    private bool activePauseMenu;
    private bool activeGameOverMenu;
    private bool activeWinMenu;
    internal float masterVolume;
    internal float musicVolume;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private AudioManager audioManager;
    public static MainMenu instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            GameObject gameObject = GameObject.Find("AudioManager");
            audioManager = gameObject.GetComponent<AudioManager>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            activeMainMenu = true;
            canvasGroup.alpha = 1f;
        }
        else
        {
            activeMainMenu = false;
            canvasGroup.alpha = 0f;
        }

        activeOptionMenu = false;
        activeCharacterChooser = false;
        activePauseMenu = false;
        activeGameOverMenu = false;
        activeWinMenu = false;
    }
    private void FollowTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = desiredPosition;
    }
    private void LateUpdate()
    {
        FollowTarget();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && (activeMainMenu))
            BackMenu();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            activeMainMenu = true;
        else
            activeMainMenu = false;

        if ((activeMainMenu) && (!activeOptionMenu) && (!activeCharacterChooser))
        {
            instance.transform.GetChild(1).gameObject.SetActive(true);
            instance.transform.GetChild(2).gameObject.SetActive(false);
            instance.transform.GetChild(5).gameObject.SetActive(false);
        }
        else if ((activeMainMenu) && (activeOptionMenu) && (!activeCharacterChooser))
        {
            instance.transform.GetChild(1).gameObject.SetActive(false);
            instance.transform.GetChild(2).gameObject.SetActive(true);
            instance.transform.GetChild(5).gameObject.SetActive(false);
        }
        else if ((activeMainMenu) && (!activeOptionMenu) && (activeCharacterChooser))
        {
            instance.transform.GetChild(1).gameObject.SetActive(false);
            instance.transform.GetChild(2).gameObject.SetActive(false);
            instance.transform.GetChild(5).gameObject.SetActive(true);
        }
        else if (!activeMainMenu)
        {
            instance.transform.GetChild(1).gameObject.SetActive(false);
            instance.transform.GetChild(2).gameObject.SetActive(false); 
            instance.transform.GetChild(5).gameObject.SetActive(false);
        }
        
        if (activePauseMenu)
        {
            instance.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (!activePauseMenu)
        {
            instance.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (activeGameOverMenu)
        {
            instance.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (!activeGameOverMenu)
        {
            instance.transform.GetChild(4).gameObject.SetActive(false);
        }

        if (activeWinMenu)
        {
            instance.transform.GetChild(6).gameObject.SetActive(true);
        }
        else if (!activeWinMenu)
        {
            instance.transform.GetChild(6).gameObject.SetActive(false);
        }
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (!activeGameOverMenu)
            {
                if ((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 1))
                {
                    PauseGame();
                }
                else if ((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 0))
                {                
                    ResumeGame();
                }
            }
        }
    }
    public void SetMasterVolume (float volume)
    {
        masterVolume = volume;
        audioManager.SetVolumeSound(masterVolume, "Master");
    }
    public void SetMusicVolume (float volume)
    {
        musicVolume = volume;
        audioManager.SetVolumeSound(musicVolume, "Music");
    }
    public void PlayGame()
    {
        activeCharacterChooser = true;
    }
    public void Botton1()
    {
        playerChoosen = 0;
        audioManager.StopSound("Music");
        StartCoroutine(LoadLevel(1,playerChoosen));
    }
    public void Botton2()
    {
        playerChoosen = 1;
        audioManager.StopSound("Music");
        StartCoroutine(LoadLevel(1,playerChoosen));
    }
    public void Botton3()
    {
        playerChoosen = 2;
        audioManager.StopSound("Music");
        StartCoroutine(LoadLevel(1,playerChoosen));
    }
    public void QuitGame()
    {
        audioManager.StopSound("Music");
        StartCoroutine(Quit());
    }
    public void GameOver()
    {
        canvasGroup.alpha = 0.8f;
        if (!activeGameOverMenu)
            activeGameOverMenu = true;
    }
    public void RetryGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, playerChoosen));
        if (activeGameOverMenu)
            activeGameOverMenu = false;
    }
    public void PauseGame()
    {
        canvasGroup.alpha = 0.5f;
        Time.timeScale = 0f;
        if (!activePauseMenu)
            activePauseMenu = true;
    }
    public void ResumeGame()
    {
        canvasGroup.alpha = 0f;
        Time.timeScale = 1f;
        if (activePauseMenu)
            activePauseMenu = false;
    }
    public void OptionMenu()
    {
        activeOptionMenu = true;
    }
    public void BackMenu()
    {
        audioManager.StopSound("Music");

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartCoroutine(LoadLevel(0,playerChoosen));
        }

        Time.timeScale = 1f;

        if (activePauseMenu)
            activePauseMenu = false;
        if (activeOptionMenu)
            activeOptionMenu = false;
        if (activeGameOverMenu)
            activeGameOverMenu = false;
        if (activeWinMenu)
            activeWinMenu = false;
        if (activeCharacterChooser)
            activeCharacterChooser = false;
    }
    public void WinMenu()
    {
        canvasGroup.alpha = 0.6f;
        if (!activeWinMenu)
            activeWinMenu = true;
    }
    private IEnumerator LoadLevel(int levelIndex, int characterChoosen)
    {
        crossFade.SetTrigger("Start");

        Time.timeScale = 1f;

        yield return new WaitForSeconds(transitionTimeCF);

        {
            Data data = SaveSystem.Load();

            masterVolume = data.masterVolume; 
            masterSlider.value = masterVolume;
            musicVolume = data.musicVolume;
            musicSlider.value = musicVolume;
        }

        SceneManager.LoadScene(levelIndex);

        audioManager.PlaySound("Music");

        if (levelIndex != 0)
        {
            canvasGroup.alpha = 0f;
            activeMainMenu = false;
            if (levelIndex == 1)
            {
                yield return new WaitForSeconds(.01f);
                GameObject c = GameObject.Find("Player");
                if (characterChoosen == 0)
                {
                    c.transform.GetChild(0).gameObject.SetActive(true);
                    c.transform.GetChild(1).gameObject.SetActive(false);
                    c.transform.GetChild(2).gameObject.SetActive(false);
                }
                else if (characterChoosen == 1)
                {
                    c.transform.GetChild(0).gameObject.SetActive(false);
                    c.transform.GetChild(1).gameObject.SetActive(true);
                    c.transform.GetChild(2).gameObject.SetActive(false);
                }
                else if (characterChoosen == 2)
                {
                    c.transform.GetChild(0).gameObject.SetActive(false);
                    c.transform.GetChild(1).gameObject.SetActive(false);
                    c.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
        else 
        {
            canvasGroup.alpha = 1f;
            activeMainMenu = true;
        }
    }
    private IEnumerator Quit()
    {
        if (activePauseMenu)
            ResumeGame();
        
        crossFade.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTimeCF);

        SaveSystem.Save(this);

        Application.Quit();
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private Timer timer;

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
    private bool activeWinRecordMenu;
    private bool activeWinNoRecordMenu;
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
        activeWinRecordMenu = false;
        activeWinNoRecordMenu = false;        
    }
    private void Start()
    {
        masterSlider.value = DataBackup.masterVolume;
        musicSlider.value = DataBackup.musicVolume;
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

        if (activeWinRecordMenu)
        {
            instance.transform.GetChild(6).gameObject.SetActive(true);
        }
        else if (!activeWinRecordMenu)
        {
            instance.transform.GetChild(6).gameObject.SetActive(false);
        }
        if (activeWinNoRecordMenu)
        {
            instance.transform.GetChild(7).gameObject.SetActive(true);
        }
        else if (!activeWinNoRecordMenu)
        {
            instance.transform.GetChild(7).gameObject.SetActive(false);
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
        CoinCounter.CounterReset();
        EnemyCounter.CounterReset();
        DiamondCounter.CounterReset();
    }
    public void Botton1()
    {
        playerChoosen = 0;
        StartCoroutine(LoadLevel(1,playerChoosen));
    }
    public void Botton2()
    {
        playerChoosen = 1;
        StartCoroutine(LoadLevel(1,playerChoosen));
    }
    public void Botton3()
    {
        playerChoosen = 2;
        StartCoroutine(LoadLevel(1,playerChoosen));
    }
    public void QuitGame()
    {
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
        if (activeWinRecordMenu)
            activeWinRecordMenu = false;
        if (activeWinNoRecordMenu)
            activeWinNoRecordMenu = false;
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
        if (activeWinRecordMenu)
            activeWinRecordMenu = false;
        if (activeWinNoRecordMenu)
            activeWinNoRecordMenu = false;
        if (activeCharacterChooser)
            activeCharacterChooser = false;
    }
    public void WinMenu(bool flag)
    {
        canvasGroup.alpha = 0.6f;
        if (flag)
        {
            if (!activeWinRecordMenu)
                activeWinRecordMenu = true;
        }
        else
        {
            if (!activeWinNoRecordMenu)
                activeWinNoRecordMenu = true;
        }
    }
    private IEnumerator LoadLevel(int levelIndex, int characterChoosen)
    {
        crossFade.SetTrigger("Start");

        Time.timeScale = 1f;

        yield return new WaitForSeconds(transitionTimeCF);

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

        DataBackup.Save();

        Application.Quit();
    }
}
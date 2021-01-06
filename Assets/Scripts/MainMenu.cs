using System.Collections;
using System.Collections.Generic;
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
    private bool activeMainMenu;
    private bool activeOptionMenu;
    private bool activePauseMenu;
    private bool activeGameOverMenu;
    public float masterVolume;
    public float musicVolume;
    public AudioManager audioManager = null;
    public static MainMenu instance;
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
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

        activePauseMenu = false;
        activeGameOverMenu = false;
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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            activeMainMenu = true;
        }
        else
        {
            activeMainMenu = false;
        }

        if ((activeMainMenu) && (!activeOptionMenu))
        {
            instance.transform.GetChild(1).gameObject.SetActive(true);
            instance.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if ((activeMainMenu) && (activeOptionMenu))
        {
            instance.transform.GetChild(1).gameObject.SetActive(false);
            instance.transform.GetChild(2).gameObject.SetActive(true);            
        }
        else if (!activeMainMenu)
        {
            instance.transform.GetChild(1).gameObject.SetActive(false);
            instance.transform.GetChild(2).gameObject.SetActive(false);            
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
        StartCoroutine(LoadLevel(1));
    }
    public void QuitGame()
    {
        StartCoroutine(Quit());
    }
    public void GameOver()
    {
        if (!activeGameOverMenu)
            activeGameOverMenu = true;
        canvasGroup.alpha = 0.5f;
    }
    public void RetryGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        if (activeGameOverMenu)
        {
            activeGameOverMenu = false;
        }
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
            StartCoroutine(LoadLevel(0));
        }

        Time.timeScale = 1f;

        if (activePauseMenu)
            activePauseMenu = false;
        if (activeOptionMenu)
            activeOptionMenu = false;
        if (activeGameOverMenu)
            activeGameOverMenu = false;
    }
    private IEnumerator LoadLevel(int levelIndex)
    {

        crossFade.SetTrigger("Start");

        Time.timeScale = 1f;

        yield return new WaitForSeconds(transitionTimeCF);

        {
            Data data = SaveSystem.Load();
            masterVolume = data.masterVolume;
            musicVolume = data.musicVolume;
        }

        SceneManager.LoadScene(levelIndex);
        
        if (levelIndex != 0)
        {
            canvasGroup.alpha = 0f;
            activeMainMenu = false;
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
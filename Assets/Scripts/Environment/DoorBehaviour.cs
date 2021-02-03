using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private MainMenu mainMenu;
    private AudioManager audioManager;
    private PlayerMovement playerMovement;
    private Rigidbody2D body;
    private void Start()
    {
        playerMovement = GameObject.Find("Player").transform.GetChild(MainMenu.playerChoosen).GetComponent<PlayerMovement>();;
        body = GameObject.Find("Player").transform.GetChild(MainMenu.playerChoosen).GetComponent<Rigidbody2D>();;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            GameObject gameObject = GameObject.Find("AudioManager");
            audioManager = gameObject.GetComponent<AudioManager>();
        }

        mainMenu = MainMenu.instance;
        if (mainMenu == null)
        {
            GameObject gameObject = GameObject.Find("Menu");
            mainMenu = gameObject.GetComponent<MainMenu>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("vittoriaaaaaaa");
            StartCoroutine(Win());
        }
    }
    private IEnumerator Win()
    {
        yield return new WaitForSeconds(.3f);
        playerMovement.moveSpeed = new Vector2 (0f, 0f);
        body.velocity = new Vector2(0, 0);
        body.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 0f;
        mainMenu.WinMenu();
        Debug.Log("vado");
        Debug.Log("non vado");
        audioManager.SetVolumeSound(.2f, "Music");
        CoinCounter.CounterReset();
        EnemyCounter.CounterReset();
    }
}
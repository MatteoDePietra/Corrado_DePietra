using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private MainMenu mainMenu;
    private AudioManager audioManager;
    private PlayerMovement playerMovement;
    private Rigidbody2D body;
    private bool beaten;
    private void Start()
    {
        beaten = false;
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
            if ((CoinCounter.coinCount >= CoinCounter.minimum) && (EnemyCounter.enemyCount >= EnemyCounter.minimum) && (DiamondCounter.diamondCount >= DiamondCounter.minimum))
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
        if ((TimerRecord.record - Timer.TimerControl +.03f) > 0)
        {
            TimerRecord.record = Timer.TimerControl;
            DataBackup.record = TimerRecord.record;
            beaten = true;
        }
        else 
            beaten = false;
        mainMenu.WinMenu(beaten);
        audioManager.SetVolumeSound(.2f, "Music");
        CoinCounter.CounterReset();
        EnemyCounter.CounterReset();
        DiamondCounter.CounterReset();
    }
}
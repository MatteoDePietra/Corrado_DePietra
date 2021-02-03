using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    private Text text;
    private static int enemyCount = 0;
    void Start()
    {
        text = GetComponent<Text> ();
    }
    void Update()
    {
        text.text = enemyCount.ToString();
    }
    internal static void AddEnemy() {
        enemyCount += 1;
    } 
    internal static void CounterReset() {
        enemyCount = 0;
    }
}
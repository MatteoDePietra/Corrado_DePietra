using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    private Text text;
    private static int coinCount = 0;

    void Start()
    {
        text = GetComponent<Text> ();
    }
    void Update()
    {
        text.text = coinCount.ToString();
    }
    internal static void AddCoin() {
        coinCount += 1;
    } 

    internal static void CoinReset() {
        coinCount = 0;
    }
}

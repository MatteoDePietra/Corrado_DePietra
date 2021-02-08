using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    private Text text;
    internal static int coinCount = 0;
    [SerializeField]
    internal static int minimum = 10;
    void Start()
    {
        text = GetComponent<Text> ();
    }
    void Update()
    {
        if (coinCount>=minimum)
            text.color = Color.green;
        else 
            text.color = Color.red;
        text.text = coinCount + "/" + minimum.ToString();
    }
    internal static void AddCoin(int quantity) {
        coinCount += quantity;
    } 
    internal static void CounterReset() {
        coinCount = 0;
    }
}
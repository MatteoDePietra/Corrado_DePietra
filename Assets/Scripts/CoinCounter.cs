using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    public static int coinCount = 0;

    public static int CoinCount { get => coinCount; set => coinCount = value;}
    public static void AddCoin() {
        coinCount += 1;
    } 

    public static void CoinReset() {
        coinCount = 0;
    }

    void Start()
    {
        text = GetComponent<Text> ();
    }
    void Update()
    {
        text.text = coinCount.ToString();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DiamondCounter : MonoBehaviour
{
    private Text text;
    internal static int diamondCount = 0;
    [SerializeField]
    internal static int minimum = 1;
    void Start()
    {
        text = GetComponent<Text> ();
    }
    void Update()
    {
        if (diamondCount>=minimum)
            text.color = Color.green;
        else 
            text.color = Color.red;
        text.text = diamondCount + "/" + minimum.ToString();
    }
    internal static void AddCoin() {
        diamondCount += 1;
    } 
    internal static void CounterReset() {
        diamondCount = 0;
    }
}
 using UnityEngine;
 using UnityEngine.UI;
 public class Timer : MonoBehaviour 
 {
    internal string timerString;
    private float startTime;
    internal static float TimerControl;
    private void Start () 
    {
        startTime = Time.time;
    }
    private void FixedUpdate()
    {
        TimerControl = Time.time - startTime;
        string mins = ((int)TimerControl/59).ToString("00");
        string segs = (TimerControl % 59).ToString("00");
        string milisegs = ((TimerControl * 99)%99).ToString("00");
            
        timerString = string.Format ("{00}:{01}:{02}", mins, segs, milisegs);
            
        GetComponent<Text>().text = timerString.ToString ();
    }
 }
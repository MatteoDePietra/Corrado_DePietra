 using UnityEngine;
 using UnityEngine.UI;
 public class Timer : MonoBehaviour 
 {
    private float StartTime;
    private void Start () 
    {
        StartTime = Time.time;
    }
    private void FixedUpdate()
    {
        float TimerControl = Time.time - StartTime;
        string mins = ((int)TimerControl/59).ToString("00");
        string segs = (TimerControl % 59).ToString("00");
        string milisegs = ((TimerControl * 99)%99).ToString ("00");
            
        string TimerString = string.Format ("{00}:{01}:{02}", mins, segs, milisegs);
            
        GetComponent<Text>().text = TimerString.ToString ();
    }
 }
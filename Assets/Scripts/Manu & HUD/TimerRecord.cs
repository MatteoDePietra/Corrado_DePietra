 using UnityEngine;
 using UnityEngine.UI;

public class TimerRecord : MonoBehaviour
{
    internal static float record;
    internal string recordString;
    private bool noRecord = false;
    private void Start () 
    {
        record = DataBackup.record;
        if (record == 0)
            noRecord = true;
    }
    private void FixedUpdate()
    {
        if (noRecord)    
            record = Timer.TimerControl;
        
        if ((record != Timer.TimerControl) && (DataBackup.record > record))
        {
            noRecord = false;
            DataBackup.record = record;
        }
            
        string mins = ((int)record/59).ToString("00");
        string segs = (record % 59).ToString("00");
        string milisegs = ((record * 99)%99).ToString("00");
            
        recordString = string.Format ("{00}:{01}:{02}", mins, segs, milisegs);

        GetComponent<Text>().text = recordString.ToString ();
    }
}
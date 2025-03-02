using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CommunicatorWithBrowser : MonoBehaviour
{
    public bool ReadyToClickBrowser = false;
    public Vector2 BrowserPointer;
    public GameObject BrowserPointerObject;
    public HandInstrument HI;

    public bool DoClickBack;
    public bool DoClickForward;
    public bool DoClickGoogle;
    public bool DoClickReload;

    public string Command;
    public bool DoSendCommand;

    public string URL;
    public bool DoOpenURL;

    public void DoClickBrowserByWorldCords(Vector3 Cords, bool byHand)
    {
        if (byHand) BrowserPointerObject.transform.position = HI.GetAnnotation(8, 0).transform.position;
        else BrowserPointerObject.transform.position = Cords;
        DoClickBrowser(new Vector2((360 + BrowserPointerObject.transform.localPosition.x) / 720, (-BrowserPointerObject.transform.localPosition.y) / 350));
    }
    public void DoClickBrowser(Vector2 point)
    {
        BrowserPointer = point;
        ReadyToClickBrowser = true;
    }

    public void DoClickBackMethod() { DoClickBack = true; }
    public void DoClickForwardMethod() { DoClickForward = true; }
    public void DoClickGoogleMethod() { DoClickGoogle = true; }
    public void DoClickReloadMethod() { DoClickReload = true; }
    public void DoOpenURLMethod(TextMeshProUGUI TextInput)
    {
        URL = TextInput.text;
        DoOpenURL = true;
    }
    public void DoSendCommandMethod(string _Command) 
    { 
        Command = _Command;
        DoSendCommand = true;
        Debug.Log("Key input worked on a Communicator:" + _Command);
    }
}

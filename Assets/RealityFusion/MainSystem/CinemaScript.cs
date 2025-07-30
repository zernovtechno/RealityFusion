using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandTracking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RealityFusion.API;
public class CinemaScript : MonoBehaviour
{
    [SerializeField] EasyAPI _EasyAPI;
    [SerializeField] private RawImage TheaterScreen; // Image for screen-transmission from pc
    [SerializeField] public RawImage ScreenCastScreen;

    [SerializeField] private GameObject Theater;
    //[SerializeField] private GameObject PlayerObject;
    //[SerializeField] private GameObject InterfaceObject;

    [SerializeField] private GameObject ScreenCast;

    [SerializeField] private GameObject ScreenCaster;

    [SerializeField] private Mediapipe.Unity.Screen CameraScript;
    [SerializeField] private MJPEGStreamDecoder MJPEGSD;

    //private Vector3 CameraPosition = new Vector3(4.5f, -40, -65);
    //private Vector3 InterfacePosition = new Vector3(4.5f, -40, -65);
    //private Vector3 DefaultPosition = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    public void Update()
    {
        if (!ScreenCast.activeSelf && !_EasyAPI.GetLeftScreen().gameObject.activeSelf)
        {
            TheaterScreen.texture = CameraScript.texture;
        }
    }

    public void TurnCinema()
    {
        if (Theater.activeSelf)
        {
            Theater.SetActive(false);
            Theater.transform.rotation = _EasyAPI.GetPlayerObject().transform.rotation;
            MJPEGSD.OutTexture = ScreenCastScreen;
            ScreenCaster.SetActive(true);
            //PlayerObject.transform.position = DefaultPosition;
            //InterfaceObject.transform.position = DefaultPosition;
            _EasyAPI.GetLeftScreen().gameObject.SetActive(true);
            _EasyAPI.GetRightScreen().gameObject.SetActive(true);
        }
        else
        {
            Theater.SetActive(true);
            MJPEGSD.OutTexture = TheaterScreen;
            ScreenCaster.SetActive(false);
            //PlayerObject.transform.position = CameraPosition;
            //InterfaceObject.transform.position = InterfacePosition;
            _EasyAPI.GetLeftScreen().gameObject.SetActive(false);
            _EasyAPI.GetRightScreen().gameObject.SetActive(false);
        }
    }

    public void goLeft()
    {
        //CameraPosition.x = CameraPosition.x - 10;
        //InterfacePosition.x = InterfacePosition.x - 10;
        //PlayerObject.transform.position = CameraPosition;
        //InterfaceObject.transform.position = InterfacePosition;
    }

    public void goRight()
    {
        /*CameraPosition.x = CameraPosition.x + 10;
        InterfacePosition.x = InterfacePosition.x + 10;
        PlayerObject.transform.position = CameraPosition;
        InterfaceObject.transform.position = InterfacePosition;*/
    }

    public void goDown()
    {
        /*CameraPosition.z = CameraPosition.z + 20;
        CameraPosition.y = CameraPosition.y - 12;
        InterfacePosition.z = InterfacePosition.z + 20;
        InterfacePosition.y = InterfacePosition.y - 12;
        PlayerObject.transform.position = CameraPosition;
        InterfaceObject.transform.position = InterfacePosition;*/
    }

    public void goUp()
    {
        /*CameraPosition.z = CameraPosition.z - 20;
        CameraPosition.y = CameraPosition.y + 12;
        InterfacePosition.z = InterfacePosition.z - 20;
        InterfacePosition.y = InterfacePosition.y + 12;
        PlayerObject.transform.position = CameraPosition;
        InterfaceObject.transform.position = InterfacePosition;*/
    }
}

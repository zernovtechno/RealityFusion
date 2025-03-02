using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandTracking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public AudioSource ClickSound;
    public GameObject AnnotationLayer;
    public WindowPositionController WPC;
    public CommunicatorWithBrowser CWB;

    private bool Cooldown;

    void Start()
    {
        InvokeRepeating("Update_Every_2000ms", 2, 2f);
    }
    void Update_Every_2000ms ()
    {
        if (Cooldown) Cooldown = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.CompareTag("Interface") && AnnotationLayer.activeSelf)
        {
            Debug.Log($"Trigger with {other.name}.");
        }
        if (other.CompareTag("ThumbFinger"))
        {
            WPC.TriggerWithThumb = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (AnnotationLayer.activeSelf && !Cooldown)
        {
            if (other.CompareTag("Interface")) // If there interface on a way
            {
                //if (!other.gameObject.GetComponentInParent<Canvas>().CompareTag("SafePane")) _MainTrackingModule.LastContactedMenu = other.gameObject.GetComponentInParent<Canvas>();
                ExecuteEvents.Execute(other.gameObject.GetComponent<Button>().gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler); // Do clicking on a button (If its a button) by emulating mouse event
                ClickSound.Play(); // Clicking sound
                Cooldown = true; // Restart cooldown
            }
            if (other.CompareTag("ThumbFinger"))
            {
                WPC.TriggerWithThumb = false; // Off position changer
            }
            if (other.CompareTag("Browser")) // If there is a browser...
            {
                CWB.DoClickBrowserByWorldCords(other.transform.position, true); // Click
                Cooldown = true; // Restart cooldown
            }
        }
    }
}

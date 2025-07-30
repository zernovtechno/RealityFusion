using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandTracking;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using RealityFusion.API;

namespace RealityFusion.Interaction.HandTracking
{
    public class Interactor : MonoBehaviour
    {
        public EasyAPI _EasyAPI;
        public AudioSource ClickSound;
        public GameObject AnnotationLayer;
        public WindowPositionController WPC;
        public CommunicatorWithBrowser CWB;

        private bool Cooldown;

        public float maxDistance = 500f;

        void Start()
        {
            InvokeRepeating("Update_Every_2000ms", 2, 2f);
            //InvokeRepeating("ThrowRaycastEvery200ms", 2, 0.2f);
        }

        void ThrowRaycastEvery200ms()
        {
            Vector3 startPoint = _EasyAPI.GetPlayerObject().transform.position;
            Vector3 endPoint = this.transform.position;
            // Вычисляем направление луча
            Vector3 direction = (endPoint - startPoint).normalized;

            // Создаем луч, начинающийся с startPoint и направленный в вычисленном направлении
            Ray ray = new Ray(startPoint, direction);

            // Получаем все попадания луча
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);

            // Бросаем луч
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("StartThatPane")) // If there is a raycast...
                {
                    if (!hit.collider.GetComponentInChildren<Canvas>(true).gameObject.activeSelf) hit.collider.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
                }
                if (hit.collider.CompareTag("Interface")) // If there interface on a way
                {
                    ExecuteEvents.Execute(hit.collider.gameObject.GetComponent<UnityEngine.UI.Button>().gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler); // Do clicking on a button (If its a button) by emulating mouse event
                    ClickSound.Play();
                    break;
                }
                if (hit.collider.CompareTag("Browser")) // If there is a browser...
                {
                    CWB.DoClickBrowserByWorldCords(hit.point, false);
                }
            }
        }
        void Update_Every_2000ms()
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Mediapipe.Unity.Sample.HandTracking
{
    public class CenterPointModule : MonoBehaviour
    {
        [SerializeField] GameObject centerPointCanvas;
        [SerializeField] GameObject centerPoint;
        [SerializeField] GameObject PlayerObject;
        [SerializeField] AudioSource ClickSound;
        [SerializeField] CommunicatorWithBrowser CWB;
        [SerializeField] UnityEngine.UI.Image Circle;

        private Vector3 CenterPoint = new Vector3(0, 0, 0);
        private Vector3 CenterPointOld = new Vector3(0, 0, 0);


        public float horizontalSpeed = 4.0f;
        public float verticalSpeed = 4.0f;

        private float h, v;

        private bool DoUpdateTimer = true;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("Update_Every_10ms", 2, 0.01f);
        }
        void UpdateTimer()
        {
            DoUpdateTimer = true;
            Debug.Log("Clicking timer was updated");
        }

        private void Update()
        {
            if (centerPointCanvas.activeSelf)
            {
                h = horizontalSpeed * Input.GetAxis("Vertical");
                v = verticalSpeed * Input.GetAxis("Horizontal");
                h += horizontalSpeed * Input.GetAxis("Mouse Y");
                v += verticalSpeed * Input.GetAxis("Mouse X");
                if (centerPoint.transform.localPosition.x + v < 1200 && centerPoint.transform.localPosition.x + v > -1200 && centerPoint.transform.localPosition.y + h < 1200 && centerPoint.transform.localPosition.y + h > -1200)
                {
                    centerPoint.transform.localPosition = new Vector3(centerPoint.transform.localPosition.x + v, centerPoint.transform.localPosition.y + h, centerPoint.transform.localPosition.z);
                }

                centerPointCanvas.transform.localPosition = new Vector3(centerPointCanvas.transform.localPosition.x + v, centerPointCanvas.transform.localPosition.y + h, centerPointCanvas.transform.localPosition.z);

            }
        }

        private void DoClick()
        {
            Ray ray = new Ray(CenterPoint, PlayerObject.transform.position - CenterPoint); // Prepare raycast from point to the camera

            float maxDistance = Vector3.Distance(CenterPoint, PlayerObject.transform.position); // Calculate distance to camera
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance); // Throw raycast from point to the camera
            Debug.DrawRay(CenterPoint, PlayerObject.transform.position - CenterPoint);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("StartThatPane")) // If there is a raycast...
                {
                    if (!hit.collider.GetComponentInChildren<Canvas>(true).gameObject.activeSelf) hit.collider.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
                }
                if (hit.collider.CompareTag("Interface")) // If there interface on a way
                {
                    //if (!hit.collider.gameObject.GetComponentInParent<Canvas>().CompareTag("SafePane")) LastContactedMeun = hit.collider.gameObject.GetComponentInParent<Canvas>();
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

        // Update is called once per frame
        void Update_Every_10ms()
        {
            CenterPoint = centerPoint.transform.position;
            if (centerPointCanvas.activeSelf)
            {

                if (Input.GetAxis("Submit") > 0 || Input.GetAxis("Cancel") > 0 || Input.GetAxis("Fire1") > 0 || Input.GetAxis("Jump") > 0 || Input.GetAxis("Fire2") > 0 || Input.GetAxis("Fire3") > 0)
                {
                    DoClick();
                }
                if (Vector3.Distance(CenterPoint, CenterPointOld) < 1 && DoUpdateTimer)
                {
                    if (Circle.fillAmount >= 1)
                    {
                        Circle.fillAmount = 0;
                        DoUpdateTimer= false;
                        InvokeRepeating("UpdateTimer", 2, 0);
                        DoClick();
                    }
                    else Circle.fillAmount += 0.005f;
                }
                else if (Circle.fillAmount - 0.02f >= 0) Circle.fillAmount -= 0.02f;
                CenterPointOld = CenterPoint;
            }
        }

    }
}

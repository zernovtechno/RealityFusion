/*
                                ,,    ,,                                                          ,,                        
`7MM """Mq.                    `7MM    db    mm                   `7MM"""YMM db                        
  MM   `MM.                     MM          MM                     MM    `7                                                 
  MM   ,M9   .gP"Ya   ,6"Yb.    MM  `7MM  mmMMmm  `7M'   `MF'      MM   d   `7MM  `7MM  ,pP"Ybd `7MM   ,pW"Wq.  `7MMpMMMb.  
  MMmmdM9  , M'   Yb 8)   MM    MM    MM    MM      VA   ,V        MM""MM     MM    MM  8I   `"   MM  6W'   `Wb   MM    MM  
  MM  YM.   8M""""""  ,pm9MM    MM    MM    MM       VA ,V         MM   Y     MM    MM  `YMMMa.   MM  8M     M8   MM    MM  
  MM   `Mb. YM.    , 8M   MM    MM    MM    MM        VVV          MM         MM    MM  L.   I8   MM  YA.   ,A9   MM    MM  
.JMML. .JMM. `Mbmmd' `Moo9^Yo..JMML..JMML.  `Mbmo     ,V         .JMML.       `Mbod"YML.M9mmmP' .JMML. `Ybmd9'  .JMML  JMML.
                                                     ,V                                                                     
                                                  OOb"                                                           Zernov 2025
*/

// ��� Center Point Module - ������ ������� ������, ������������� �� ��� ������� � ������ ������� ��� � ����������� �����.
//
// Center Point Module ���������� ������������ ����������, ����������� ����� �����, � ����� ������.

using RealityFusion.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RealityFusion.Interaction
{
    public class CenterPointModule : MonoBehaviour
    {
        [SerializeField] EasyAPI _EasyAPI; // ��������
        [SerializeField] UnityEngine.UI.Image Circle; // ����, ������� ����� ����������� �� ���� �������
        [SerializeField] float UpdateSpeedInSec = 0.01f; // ������� ����������� ��������.

        public float maxDistance = 500f; // ������������ ��������� ������������ ���� �� �����

        private Vector3 CenterPointPos = new Vector3(0, 0, 0); // ���������� ���������� ��� ��������� �������� (���������� ���������)
        private Vector3 CenterPointOldPos = new Vector3(0, 0, 0); // ���������� ���������� ��� ��������� �������� (������� ���������)

        private Vector3 startPoint; // ��������� ����� (������ �����)
        private Vector3 endPoint; // ������������� ����� (���� ��� �����)
        private Vector3 direction; // ����������� (�����������)
        private Ray ray; // ��� ��� ������
        private RaycastHit[] hits; // ������ ������������ ����

        private bool DoUpdateTimer = true; // ������ ��� �������
        private bool DoUpdateTimerForJoystick = true; // ������ ��� ������� � ����������

        UnityEngine.Color CenterPointColor; // ���� ��� ��������� � ������������� ����������� ����� (��������)

        void Start()
        {
            InvokeRepeating("Update_Every_10ms", 2, UpdateSpeedInSec); // ������ ������ "�����������" ������ Update
        }

        public void UpdateTimer() // �������� ������ �����
        {
            DoUpdateTimer = true;
            Debug.Log("Clicking timer updated");
        }

        public void UpdateTimerForJoystick() // �������� ������ ���������
        {
            DoUpdateTimerForJoystick = true;
            Debug.Log("Joystick timer updated");
        }

        private void ThrowRayCast() // ������� ��� ��� ����� � ������� ����������
        {
            startPoint = _EasyAPI.GetPlayerObject().transform.position; // ��������� ����� (������� ������)
            endPoint = CenterPointPos; // ������������� ����� ��� ����������� �����������
            direction = (endPoint - startPoint).normalized; // ��������� ����������� ����
            ray = new Ray(startPoint, direction); // ������� ���, ������������ � startPoint � ������������ � ����������� �����������
            hits = Physics.RaycastAll(ray, maxDistance); // �������� ��� ��������� ����

            //Debug.DrawLine(startPoint, startPoint + direction * maxDistance, UnityEngine.Color.red); // ����� ��� ��������� ����
        }

        private void CheckWithInterface() // ��������, ���� �� �� ���� ���������
        {
            if (!(!_EasyAPI.GetTrackingType() && !_EasyAPI.IsHandDetected())) // �������� �� ������� ���� � �����, �� ��������� ������������ ����
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Interface") || hit.collider.CompareTag("Browser")) // ���� �� ���� ���-�� ����
                    {
                        InvokeRepeating("FadeIn", 0, 0.05f); // ���������� �����
                        return;
                    }
                }
            }
           InvokeRepeating("FadeOut", 0, 0.1f); // ���������� ��� - ����� ���� ������
        }

        private void DoClick() // �������!
        {
            foreach (RaycastHit hit in hits) // ���������� ��� ������������ � ������������
            {
                if (hit.collider.CompareTag("StartThatPane")) // ���� ���� ������, ������� ����� �������...
                {
                    if (!hit.collider.GetComponentInChildren<Canvas>(true).gameObject.activeSelf) hit.collider.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
                }
                if (hit.collider.CompareTag("Interface")) // ���� �� ���� ������ ����������...
                {
                    ExecuteEvents.Execute(hit.collider.gameObject.GetComponent<UnityEngine.UI.Button>().gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler); // Do clicking on a button (If its a button) by emulating mouse event
                    _EasyAPI.PlayClickSound(); // ������ ���� ����� ����� API
                    break;
                }
                if (hit.collider.CompareTag("Browser")) // ���� �� ���� �������...
                {
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoClickByWorldCoordinates(hit.point); // ������ � ������� ����� API
                }
            }
        }

        private void PrepareForClick() // ���������������� � ����� (��������� ����)
        {
            CenterPointPos = _EasyAPI.GetCenterPoint().transform.position; // ������������� ������� ��������
            if (Input.GetAxis("Submit") > 0 || Input.GetAxis("Cancel") > 0 || Input.GetAxis("Fire1") > 0 || Input.GetAxis("Jump") > 0 || Input.GetAxis("Fire2") > 0 || Input.GetAxis("Fire3") > 0)
            { // ������ ��� ���������/�������
                if (DoUpdateTimer)
                {
                    CancelInvoke("UpdateTimerForJoystick");
                    DoClick(); // ������� ������ ��������
                    DoUpdateTimer = false;
                    InvokeRepeating("UpdateTimer", 1, 0);
                    DoUpdateTimerForJoystick = false;
                    InvokeRepeating("UpdateTimerForJoystick", 15, 0);
                }
            }
            if (Vector3.Distance(CenterPointPos, CenterPointOldPos) < 1 && DoUpdateTimer && DoUpdateTimerForJoystick && CenterPointColor.a >= 1) // ������ �� ���������
            {
                //Debug.Log(Vector3.Distance(CenterPointPos, CenterPointOldPos));
                if (Circle.fillAmount >= 1) // ���� ���� ��������� ��������...
                {
                    Circle.fillAmount = 0; // ���������� ���
                    DoUpdateTimer = false; // ��������� ������
                    InvokeRepeating("UpdateTimer", 2, 0);
                    DoClick(); // �������
                }
                else Circle.fillAmount += 0.005f; // � ���� ����� ���������� ���������
            }
            else if (Circle.fillAmount - 0.02f >= 0) Circle.fillAmount -= 0.02f; // ���� �������� ����� ��������� ������� �� �������� ��������� ���� � ��������� ���
            CenterPointOldPos = CenterPointPos;
        }

        public void Update_Every_10ms()
        {
            if (!_EasyAPI.GetTrackingType()) _EasyAPI.GetCenterPoint().transform.position = _EasyAPI.GetMainFingerAnnotation().transform.position; // ������������� ������� �������� �� ������� ������������� ������ ����, ���� ������ ������� ��� � ���� � �����
            ThrowRayCast();
            CheckWithInterface();
            PrepareForClick();
        }
        public void FadeIn() // ������� ���������
        {
            CenterPointColor = _EasyAPI.GetCenterPoint().GetComponent<UnityEngine.UI.Image>().color;
            CenterPointColor.a += 0.2f;
            _EasyAPI.GetCenterPoint().GetComponent<UnityEngine.UI.Image>().color = CenterPointColor;
            if (CenterPointColor.a >= 1) CancelInvoke("FadeIn");
        }

        public void FadeOut() // ������� ���������
        {
            CenterPointColor = _EasyAPI.GetCenterPoint().GetComponent<UnityEngine.UI.Image>().color;
            CenterPointColor.a -= 0.2f;
            _EasyAPI.GetCenterPoint().GetComponent<UnityEngine.UI.Image>().color = CenterPointColor;
            if (CenterPointColor.a <= 0)
            {
                CancelInvoke("FadeOut");
                Circle.fillAmount = 0;
            }
        }

    }
}

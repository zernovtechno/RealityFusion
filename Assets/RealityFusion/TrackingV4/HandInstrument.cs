
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

// ��� Hand Instrument - ������� ������, ������������� �� ����� ������ Reality Fusion � ���������� MediaPipe.
//
// Hand Instrument ���������� ���������� ����� ���, ������������� MediaPipe'��, � ����� ���������� �� ������������ ����� �����, ������� ��������������� � �����������.

using Mediapipe.Unity;
using UnityEngine;

namespace RealityFusion.Interaction.HandTracking
{
    public class HandInstrument : MonoBehaviour
    {
        [SerializeField] API.EasyAPI _EasyAPI; // ������� � �������������� ���������
        [SerializeField] MultiHandLandmarkListAnnotation _MultiHandLandmarkListAnnotation; // ���������� ��� �������� ����� ���� � ������� ��������� MediaPipe
        HandLandmarkListAnnotation _HandLandmarkListAnnotation; // ���������� ���������� �������������� �� ��������� ������ ������
        PointListAnnotation PointAnnotations; // ��� ���� ����� ����������

        [HideInInspector] public bool IsSetupDone = false; // ��������� (��������� ����� ��������) �����������?
        [HideInInspector] public bool TrackingIsWorking = false; // ���� ������ � �����?

        public int mainFingerNumber = 8; // �����, ��� ������ �������� ��������������� � ����������� (�� ����������� ����� ������). 8 - ������������

        [SerializeField] GameObject[] HandLandmarks; // ������ �������� 3D-����. �� ������������.

        //Vector3 WhiteHandRotation; // ��������� ��� �������� ���������� ������� 3D-���� (WhiteHand)  �� ������������.

        private void Start()
        {
            //InvokeRepeating("ChangePositionOf3DHand", 1, 0.01f); // ��������� ������ ����������������� ��������� 3D ������ ���� ������ 10 ��. ��������
        }

        public PointAnnotation GetAnnotation(int PointNumber, int HandNumber) // ������ ��� ����������� ������� ��������������� � ���������� �������� ����. ���������� ����������� ����� ������.
        {
            return _MultiHandLandmarkListAnnotation.gameObject.GetComponentsInChildren<HandLandmarkListAnnotation>(true)[HandNumber]
                .gameObject.GetComponentInChildren<PointListAnnotation>(true)[PointNumber]; // ������ �����
        }

        /*private Vector3 CalculateRotation() // ������� �������� ���� ��� ���������� ���������. ������������ ���� �������� ������ ����� �������� ������� WhiteHand.
        {
            WhiteHandRotation = _EasyAPI.GetPlayerObject().transform.localEulerAngles; // ���������� �������� ���������� �� ������� "������".

            if (WhiteHandRotation.y < 180) // ����...
            {
                WhiteHandRotation.y += 180; // ������������ ���
            }
            else if (WhiteHandRotation.y > 180) // ����...
            {
                WhiteHandRotation.y -= 180; // ������������ ���
            }
            else
            {
                WhiteHandRotation.y = 0; // ���� �������� ���������� ������ �� Y = 180, �� ����� 0 (����������� ���������).
            }
            return WhiteHandRotation; // ���������� ������� ���������� (� ����� ������)
        }


        private void ChangePositionOf3DHand() // Change position of 3D hand, and layer with it
        {
            if (IsSetupDone && TrackingIsWorking)
            {

                //HandLandmarks[0].SetActive(true);
                //Debug.Log(HandLandmarks[3].transform.localPosition);
                //HandLandmarks[0].transform.rotation = _EasyAPI.GetPlayerObject().transform.rotation;
                //if (HandLandmarks[1].transform.localPosition.x < HandLandmarks[17].transform.localPosition.x) { HandLandmarks[0].transform.localScale = new Vector3(4, 1, 2); }
                //else { HandLandmarks[0].transform.localScale = new Vector3(-4, 1, 2); }
                
                if (GetAnnotation(1, 0).transform.localPosition.x < GetAnnotation(17, 0).transform.localPosition.x)  // ���� ������� ����� ��������� ����� �������.
                {
                    //HandLandmarks[0].transform.rotation = _EasyAPI.GetPlayerObject().transform.rotation; // ���������� ���������� 3D-���� (WhiteHand) � ���������� ������� "������".
                    //HandLandmarks[0].transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    // Debug.Log(CalculateRotation()); // ������� ������  
                    // HandLandmarks[0].transform.localEulerAngles = CalculateRotation(); // ��������� ������������ ����������4
                    //HandLandmarks[0].transform.localRotation = new Quaternion(0, 1, 0, 0);
                }

                for (int i = 0; i < 21; i++) // ���� �� 21 ��������� ����
                {
                    try
                    {
                        HandLandmarks[i].transform.position = GetAnnotation(i, 0).transform.position; // ������ ����� �� 3D-���� � ������� �������� ���.
                    }
                    catch (Exception) { } // ������� Catch. � 3D-���� �� ��� ������ ��������� ���� ���� ������, ��� ��� �� ���� ���� ����� �����������.
                }
                // ��� ���� ������� �� ����� ���� �����-������ �� ������������. ���� ������� ���������� � ��� ������ ��� ���������� ���������� �������.
                //AnnotationLayer.transform.localPosition = new Vector3(20, 0, -(calibration_startPos + (Vector3.Distance(GetAnnotation(0, 0).transform.position, GetAnnotation(9, 0).transform.position) * calibration_multiplier)));
            }
            else HandLandmarks[0].SetActive(false);
        }*/

        public void ReCreateInteractor(int _FingerNumber = 8) // ����� �� ������, ���� ����������� ����� ����� ������� � ��������. ���� �� �������������.
        {
            mainFingerNumber = _FingerNumber; // ������������ ����������� ����� � �����������
            IsSetupDone = false; // ��������������� ����� (��-����, � �������� V4 �� ���������.)
        }

        private void Update()
        {
            if (!IsSetupDone && !_EasyAPI.GetTrackingType()) // ���� �� ��� �� ����������� ������ � ������, �������� ���
            {
                try
                {
                    // ����� ��������� �������� ������� � ������ ����������� ������.
                    _HandLandmarkListAnnotation = _MultiHandLandmarkListAnnotation.gameObject.GetComponentsInChildren<HandLandmarkListAnnotation>(true)[0];
                    PointAnnotations = _HandLandmarkListAnnotation.gameObject.GetComponentInChildren<PointListAnnotation>(true);

                    // ��� ���� ���������������, ��� ��� ���������� ��� �������� ������ 3. ����������� ���������� �������.

                    /*Interactor FingerPointInterator = PointAnnotations[mainFingerNumber].gameObject.AddComponent<Interactor>();
                    FingerPointInterator.ClickSound = ClickSound;
                    FingerPointInterator.AnnotationLayer = AnnotationLayer;
                    FingerPointInterator.WPC = WPC;
                    FingerPointInterator.CWB = CWB;
                    FingerPointInterator._EasyAPI = _EasyAPI;

                    Rigidbody ColliderRigidBody = PointAnnotations[mainFingerNumber].gameObject.AddComponent<Rigidbody>();
                    ColliderRigidBody.isKinematic = true;
                    ColliderRigidBody.useGravity = false;

                    SphereCollider FingerSphereCollider = PointAnnotations[mainFingerNumber].gameObject.AddComponent<SphereCollider>();
                    FingerSphereCollider.isTrigger = true;
                    FingerSphereCollider.radius = 1f;*/


                    //centerPointCanvas.transform.SetParent(GetAnnotation(mainFingerNumber - 1, 0).transform, false); // ���������� "�����" �� 3D-����, ����� ������������ ��.
                    //centerPointCanvas.SetActive(true); // ���������� ����� (� ��������� �������� ������ CenterPointModule, ������� �������� ����������� ����� ������ �� �����.
                    //centerPointCanvas.transform.position = GetAnnotation(mainFingerNumber - 1, 0).transform.position; // ������ "�����" �� ��������� ������������� ������. 

                    //GetAnnotation(4, 0).gameObject.tag = "ThumbFinger";
                    //GetAnnotation(4, 0).GetComponent<SphereCollider>().radius = 1.5f;
                    IsSetupDone = true; // ���! �� ��������� �������� ����� � ��������� ������� � ���, � ����� ��������� � ��������� "�����".
                }
                catch
                {
                    IsSetupDone = false; // ��, ��� ���. ����� ��������� �����, ��� ��������� ������� ������ Update(). 
                    // ����������: �� ������ ���������� ��������� ������ � ������, �.�. MediaPipe ������������� �� ��� ����� ������� �������� ����.
                    // ��������� ����� Catch �������� ������� ���������, � ����� ����������� ������ ���, ����� ����������� ����������, ���� ������ �� �������� ���� � �����.
                }
            }
            if (IsSetupDone && _MultiHandLandmarkListAnnotation.gameObject.activeSelf) // ���� �������� ����� ���������, ���� �������� ������� � ������ ���� ���������...
            {
                TrackingIsWorking = true; // ��������� ��� ������� ���� ��������� ������� (� �������� ����� API-���)
            }
            else TrackingIsWorking = false; // ��, � ���� ��� - �� ���.

        }
    }
}

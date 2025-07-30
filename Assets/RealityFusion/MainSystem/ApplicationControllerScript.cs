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

// ��� Application Controller Script - �������� ������ ����������, ������������� �� ��������� ����������� �� FPS, � ����� ���� ������ ���� ��������, ���� �� ��� �� ��� ����������.

using UnityEngine;
using Google.XR.Cardboard;
using RealityFusion.API;

public class ApplicationControllerScript : MonoBehaviour
{
    [SerializeField] EasyAPI _EasyAPI; // ��������!
    [SerializeField] GameObject FirstSet; // ���� ��������� ��������� ��������
    void Start()
    {
        Application.targetFrameRate = 60; // ������������ FPS �� 60
        QualitySettings.vSyncCount = 0; // ��������� VSYNC
        UnityEngine.Screen.SetResolution(UnityEngine.Screen.width, UnityEngine.Screen.height, true, 60); // ������������ FPS �� ������
        //_EasyAPI.GetAnnotatableScreen().gameObject.transform.SetLocalPositionAndRotation(new Vector3(0,0,-600), new Quaternion(0,0,0,0));

#if UNITY_EDITOR
        _EasyAPI.GetAnnotationLayer().transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f); // � ������ ������� ������������� ����� ��� Annotation Layer, ����� ���� ������������ ���������
#endif
        if (PlayerPrefs.HasKey("TrackingType")) // ���� ��� �������� ����������
        {
            switch (PlayerPrefs.GetInt("TrackingType")) // ���� ���
            {
                case 0:
                    _EasyAPI.HandTracking(); // ���� ��� �������� - 0, �� �������� ������� ���
                    break;
                case 1:
                    _EasyAPI.CenterTracking(); // ���� ��� �������� - 1, �� �������� ������� ������������ ��������
                    break;
            }
        }
        else
        {
            FirstSet.SetActive(true); // ���� ���� �������� ��� � ������, �� ���� ��������� ��������� �������� �����������
        }
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // ��������� ��������� �� ������ �� ����������
    }

    public void Awake()
    {
        UnityEngine.Screen.sleepTimeout = SleepTimeout.NeverSleep; // ����������� ��������, ����� ����� �� ���������� �� �������
    }
    // Update is called once per frame
    void Update()
    {
        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams(); // ��������� ��� Cardboard XR
        }
        if (Api.IsCloseButtonPressed)
        {
            UnityEngine.Application.Quit(); // ������� �� ����������
        }
    }
    public void CLEAR_SAVED_PARAMS()
    {
        PlayerPrefs.DeleteAll(); // ���������� �������� ����������
    }
    public void STOP_THE_APP()
    {
        Application.Quit(); // ���������� ��������� ����������
    }
}

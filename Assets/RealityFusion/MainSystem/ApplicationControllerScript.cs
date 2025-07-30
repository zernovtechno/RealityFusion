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

// Это Application Controller Script - основной модуль приложения, ответственный за установку ограничений по FPS, и вызов меню выбора типа трекинга, если он ещё не был установлен.

using UnityEngine;
using Google.XR.Cardboard;
using RealityFusion.API;

public class ApplicationControllerScript : MonoBehaviour
{
    [SerializeField] EasyAPI _EasyAPI; // Апишечка!
    [SerializeField] GameObject FirstSet; // Меню первичной настройки трекинга
    void Start()
    {
        Application.targetFrameRate = 60; // Ограничиваем FPS на 60
        QualitySettings.vSyncCount = 0; // Отключаем VSYNC
        UnityEngine.Screen.SetResolution(UnityEngine.Screen.width, UnityEngine.Screen.height, true, 60); // Ограничиваем FPS по экрану
        //_EasyAPI.GetAnnotatableScreen().gameObject.transform.SetLocalPositionAndRotation(new Vector3(0,0,-600), new Quaternion(0,0,0,0));

#if UNITY_EDITOR
        _EasyAPI.GetAnnotationLayer().transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f); // В режиме эдитора устанавливаем Скейл для Annotation Layer, чтобы руки показывались правильно
#endif
        if (PlayerPrefs.HasKey("TrackingType")) // Если тип трекинга установлен
        {
            switch (PlayerPrefs.GetInt("TrackingType")) // Ищем его
            {
                case 0:
                    _EasyAPI.HandTracking(); // Если тип трекинга - 0, то включаем трекинг рук
                    break;
                case 1:
                    _EasyAPI.CenterTracking(); // Если тип трекинга - 1, то включаем трекинг центрального Поинтера
                    break;
            }
        }
        else
        {
            FirstSet.SetActive(true); // Если типа трекинга нет в памяти, то меню первичной настройки трекинга открывается
        }
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // Сохраняем параметры на выходе из приложения
    }

    public void Awake()
    {
        UnityEngine.Screen.sleepTimeout = SleepTimeout.NeverSleep; // Настраиваем параметр, чтобы экран не отключался по времени
    }
    // Update is called once per frame
    void Update()
    {
        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams(); // Настройка для Cardboard XR
        }
        if (Api.IsCloseButtonPressed)
        {
            UnityEngine.Application.Quit(); // Выходим из приложения
        }
    }
    public void CLEAR_SAVED_PARAMS()
    {
        PlayerPrefs.DeleteAll(); // Экстренное удаление параметров
    }
    public void STOP_THE_APP()
    {
        Application.Quit(); // Экстренная остановка приложения
    }
}

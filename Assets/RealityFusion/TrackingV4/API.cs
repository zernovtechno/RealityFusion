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

// Это EasyAPI v4. Поистине великий модуль, который связывает между собой самые разные модули.  
//
// Для доступа к любому узлу Reality Fusion достаточно добавить EasyAPI, а затем получить необходимый модуль через метод Get...().
using UnityEngine;
using ZXing;
using RealityFusion.Interaction.HandTracking;
using RealityFusion.BaseUI;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Unity;
using Vuplex.WebView.Demos;


namespace RealityFusion.API
{
    public class EasyAPI : MonoBehaviour
    {
        [SerializeField] HandInstrument _HandInstrument;
        [SerializeField] AppsControllerScript _AppsControllerScript;
        [SerializeField] KeyboardScript _KeyboardScript;
        [SerializeField] CanvasWorldSpaceDemo _CanvasWorldSpaceDemo;
        [SerializeField] Camera LeftCamera;
        [SerializeField] Camera RightCamera;
        [SerializeField] GameObject LeftScreen;
        [SerializeField] GameObject RightScreen;
        [SerializeField] GameObject PlayerObject;
        [SerializeField] GameObject InterfaceObject;
        [SerializeField] GameObject CenterPoint;
        [SerializeField] GameObject AnnotationLayer;
        [SerializeField] HandLandmarkerRunner Solution;
        [SerializeField] MessagingSystemScript MessagingSystem;
        [SerializeField] AudioSource ClickSound; // Звук клика
        [SerializeField] Mediapipe.Unity.Screen AnnotatableScreen;

        private WebCamTexture CurrentWebCamTexture;
        private Texture CurrentTexture;
        private BarcodeReader reader = new BarcodeReader();
        private Canvas LastContactedMenu;
        public void PutTextures(WebCamTexture _CurrentWebCamTexture, Texture _CurrentTexture)
        {
            CurrentWebCamTexture = _CurrentWebCamTexture;
            CurrentTexture = _CurrentTexture;
        }

        public bool IsHandAssotiationSetupDone ()
        {
            return _HandInstrument.IsSetupDone;
        }
        public bool IsHandDetected()
        {
            return _HandInstrument.TrackingIsWorking;
        }

        public HandInstrument GetHandInstrument()
        {
            return _HandInstrument;
        }

        public CanvasWorldSpaceDemo GetCanvasWorldSpaceDemo()
        {
            return _CanvasWorldSpaceDemo;
        }

        public void SetCanvasWorldSpaceDemo(CanvasWorldSpaceDemo __CanvasWorldSpaceDemo)
        {
            _CanvasWorldSpaceDemo = __CanvasWorldSpaceDemo;
        }

        public KeyboardScript GetKeyboardScript()
        {
            return _KeyboardScript;
        }

        public AppsControllerScript GetAppsControllerScript()
        {
            return _AppsControllerScript;
        }

        public PointAnnotation GetAnnotation(int PointNumber, int HandNumber)
        {
            return _HandInstrument.GetAnnotation(PointNumber, HandNumber);
        }

        public PointAnnotation GetMainFingerAnnotation()
        {
            return GetAnnotation(_HandInstrument.mainFingerNumber, 0);
        }
        public WebCamTexture GetCurrentWebCamTexture()
        {
            return CurrentWebCamTexture;
        }
        public Texture GetCurrentTexture()
        {
            return CurrentTexture;
        }
        public int GetCurrentTextureWidth()
        {
            return GetCurrentWebCamTexture().width;
        }
        public int GetCurrentTextureHeight()
        {
            return GetCurrentWebCamTexture().height;
        }
        public Result GetQRCodeDecodeResult()
        {
            return reader.Decode(CurrentWebCamTexture.GetPixels32(), CurrentWebCamTexture.width, CurrentWebCamTexture.height);
        }
        public ResultPoint[] GetQRCodeDecodeResultPoint()
        {
            return reader.Decode(CurrentWebCamTexture.GetPixels32(), CurrentWebCamTexture.width, CurrentWebCamTexture.height).ResultPoints;
        }
        public string GetQRCodeDecodeString()
        {
            return GetQRCodeDecodeResult().Text;
        }

        public Canvas GetLastContactedMenu()
        {
            return LastContactedMenu;
        }
        public void SetLastContactedMenu(Canvas _Menu)
        {
            LastContactedMenu= _Menu;
        }

        public Camera GetLeftCamera()
        {
            return LeftCamera;
        }

        public Camera GetRightCamera() 
        {
            return RightCamera; 
        }

        public GameObject GetLeftScreen()
        {
            return LeftScreen;
        }

        public GameObject GetRightScreen()
        {
            return RightScreen;
        }

        public GameObject GetPlayerObject()
        {
            return PlayerObject;
        }

        public GameObject GetInterfaceObject()
        {
            return InterfaceObject;
        }

        public MessagingSystemScript GetMessagingSystem()
        {
            return MessagingSystem;
        }

        public HandLandmarkerRunner GetSolution()
        {
            return Solution;
        }

        public Mediapipe.Unity.Screen GetAnnotatableScreen()
        {
            return AnnotatableScreen;
        }
        
        public GameObject GetCenterPoint()
        {
            return CenterPoint;
        }
        public GameObject GetAnnotationLayer()
        {
            return AnnotationLayer;
        }


        public void PlayClickSound() 
        { 
            ClickSound.Play();
        }

        public void HandTracking()
        {
            PlayerPrefs.SetInt("TrackingType", 0);
            PlayerPrefs.Save();
            Solution.gameObject.SetActive(true);
            Solution.Play();
            MessagingSystem.SetMessage("Включается трекинг рук...", Color.green);
        }

        public void StopSolution()
        {
            Solution.gameObject.SetActive(false);
        }

        public void CenterTracking()
        {
            PlayerPrefs.SetInt("TrackingType", 1);
            PlayerPrefs.Save();
            MessagingSystem.SetMessage("Включается трекинг головой...", Color.green);
            CenterPoint.gameObject.transform.localPosition = new Vector3(0, 0, 100);
            CenterPoint.gameObject.SetActive(true);
            InvokeRepeating("StopSolution", 1, 0);
        }

        public bool GetTrackingType()
        {
            switch (PlayerPrefs.GetInt("TrackingType"))
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    return false;
            }
        }

        private string url;

        public void DelayedOpenURL()
        {
            _CanvasWorldSpaceDemo.OpenURL(url);
        }

        public void OpenURL(string _url)
        {
            url = _url;
            InvokeRepeating("DelayedOpenURL", 2, 0);
        }
    }
}

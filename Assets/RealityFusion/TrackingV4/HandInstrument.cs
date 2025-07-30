
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

// Это Hand Instrument - главный модуль, ответственный за связь модуля Reality Fusion и библиотеки MediaPipe.
//
// Hand Instrument занимается получением меток рук, отслеживаемых MediaPipe'ом, а также установкой на указательный палец точки, которая взаимодействует с интерфейсом.

using Mediapipe.Unity;
using UnityEngine;

namespace RealityFusion.Interaction.HandTracking
{
    public class HandInstrument : MonoBehaviour
    {
        [SerializeField] API.EasyAPI _EasyAPI; // Великий и могущественный Интерфейс
        [SerializeField] MultiHandLandmarkListAnnotation _MultiHandLandmarkListAnnotation; // Переменная для хранения меток руки в системе координат MediaPipe
        HandLandmarkListAnnotation _HandLandmarkListAnnotation; // Внутренная переменная использованная во избежание утечки памяти
        PointListAnnotation PointAnnotations; // Ещё одна такая переменная

        [HideInInspector] public bool IsSetupDone = false; // Настройка (установка метки Поинтера) произведена?
        [HideInInspector] public bool TrackingIsWorking = false; // Рука сейчас в кадре?

        public int mainFingerNumber = 8; // Палец, при помощи которого взаимодействуем с интерфейсом (Из стандартной карты ладони). 8 - указательный

        [SerializeField] GameObject[] HandLandmarks; // Массив объектов 3D-руки. НЕ ИСПОЛЬЗУЕТСЯ.

        //Vector3 WhiteHandRotation; // Перменная для хранения ориентации объекта 3D-руки (WhiteHand)  НЕ ИСПОЛЬЗУЕТСЯ.

        private void Start()
        {
            //InvokeRepeating("ChangePositionOf3DHand", 1, 0.01f); // Запускаем повтор автокорректировки положения 3D модели руки каждые 10 мс. УСТАРЕЛО
        }

        public PointAnnotation GetAnnotation(int PointNumber, int HandNumber) // Скрипт для упрощённого доступа НЕПОСРЕДСТВЕННО к аннотациям трекинга руки. Использует стандартную карту ладони.
        {
            return _MultiHandLandmarkListAnnotation.gameObject.GetComponentsInChildren<HandLandmarkListAnnotation>(true)[HandNumber]
                .gameObject.GetComponentInChildren<PointListAnnotation>(true)[PointNumber]; // Чистая магия
        }

        /*private Vector3 CalculateRotation() // Подсчёт поворота руки для правильной отрисовки. Компенсирует угол поворота игрока углом поворота объекта WhiteHand.
        {
            WhiteHandRotation = _EasyAPI.GetPlayerObject().transform.localEulerAngles; // Записываем значения ориентации из объекта "Игрока".

            if (WhiteHandRotation.y < 180) // Если...
            {
                WhiteHandRotation.y += 180; // Компенсируем так
            }
            else if (WhiteHandRotation.y > 180) // Если...
            {
                WhiteHandRotation.y -= 180; // Компенсируем так
            }
            else
            {
                WhiteHandRotation.y = 0; // Если значение ориентации игрока по Y = 180, то выдаём 0 (Стандартное положение).
            }
            return WhiteHandRotation; // Возвращаем готовую ориентацию (в любом случае)
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
                
                if (GetAnnotation(1, 0).transform.localPosition.x < GetAnnotation(17, 0).transform.localPosition.x)  // Если большой палец трекается ЛЕВЕЕ мизинца.
                {
                    //HandLandmarks[0].transform.rotation = _EasyAPI.GetPlayerObject().transform.rotation; // Приравнять ориентацию 3D-руки (WhiteHand) к ориентации объекта "Игрока".
                    //HandLandmarks[0].transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    // Debug.Log(CalculateRotation()); // Отладка метода  
                    // HandLandmarks[0].transform.localEulerAngles = CalculateRotation(); // Установка просчитанной ориентации4
                    //HandLandmarks[0].transform.localRotation = new Quaternion(0, 1, 0, 0);
                }

                for (int i = 0; i < 21; i++) // Цикл из 21 аннотации руки
                {
                    try
                    {
                        HandLandmarks[i].transform.position = GetAnnotation(i, 0).transform.position; // Ставим точки из 3D-руки в позиции трековых рук.
                    }
                    catch (Exception) { } // Штатный Catch. В 3D-руке не для каждой аннотации есть свой аналог, так что не весь цикл будет срабатывать.
                }
                // Код ниже отвечал за сдвиг руки ближе-дальше от пользователя. Стал слишком багованным и был отменён при отключении контактной системы.
                //AnnotationLayer.transform.localPosition = new Vector3(20, 0, -(calibration_startPos + (Vector3.Distance(GetAnnotation(0, 0).transform.position, GetAnnotation(9, 0).transform.position) * calibration_multiplier)));
            }
            else HandLandmarks[0].SetActive(false);
        }*/

        public void ReCreateInteractor(int _FingerNumber = 8) // Метод на случай, если управляющий палец нужно сменить в рантайме. Пока не использовался.
        {
            mainFingerNumber = _FingerNumber; // Приравниваем аргументный палец к актуальному
            IsSetupDone = false; // Перепривязываем точки (По-сути, в трекинге V4 не требуется.)
        }

        private void Update()
        {
            if (!IsSetupDone && !_EasyAPI.GetTrackingType()) // Если мы ещё не настраивали доступ к меткам, проводим его
            {
                try
                {
                    // Магия получения быстрого доступа к меткам трекинговой ладони.
                    _HandLandmarkListAnnotation = _MultiHandLandmarkListAnnotation.gameObject.GetComponentsInChildren<HandLandmarkListAnnotation>(true)[0];
                    PointAnnotations = _HandLandmarkListAnnotation.gameObject.GetComponentInChildren<PointListAnnotation>(true);

                    // Код ниже закомментирован, так как применялся для трекинга версии 3. Использовал контактную систему.

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


                    //centerPointCanvas.transform.SetParent(GetAnnotation(mainFingerNumber - 1, 0).transform, false); // Накидываем "Точку" на 3D-руку, чтобы пользоваться ей.
                    //centerPointCanvas.SetActive(true); // Активируем точку (И автоматом стартует скрипт CenterPointModule, который начинает стандартный метод работы по точке.
                    //centerPointCanvas.transform.position = GetAnnotation(mainFingerNumber - 1, 0).transform.position; // Ставим "Точку" на положение указательного пальца. 

                    //GetAnnotation(4, 0).gameObject.tag = "ThumbFinger";
                    //GetAnnotation(4, 0).GetComponent<SphereCollider>().radius = 1.5f;
                    IsSetupDone = true; // Ура! Мы закончили привязку меток и настройку доступа к ним, а также настроили и запустили "Точку".
                }
                catch
                {
                    IsSetupDone = false; // Ну, или нет. Тогда попробуем позже, при следующем проходе метода Update(). 
                    // Примечание: Не всегда получается настроить доступ к меткам, т.к. MediaPipe конфигурирует их уже ПОСЛЕ первого трекинга руки.
                    // Выпадение этого Catch является ШТАТНОЙ ситуацией, и БУДЕТ происходить КАЖДЫЙ раз, когда запускается приложение, пока камера на отследит руку в кадре.
                }
            }
            if (IsSetupDone && _MultiHandLandmarkListAnnotation.gameObject.activeSelf) // Если привязка меток завершена, слой трекинга активен и объект руки трекается...
            {
                TrackingIsWorking = true; // Указываем что трекинг руки ПОЛНОСТЬЮ запущен (В основном нужно API-шке)
            }
            else TrackingIsWorking = false; // Ну, а если нет - то нет.

        }
    }
}

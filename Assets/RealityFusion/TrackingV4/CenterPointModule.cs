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

// Это Center Point Module - второй главный модуль, ответственный за все нажатия в режиме трекинг рук и центральной точки.
//
// Center Point Module занимается определением интерфейса, заполнением круга клика, и самим кликом.

using RealityFusion.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RealityFusion.Interaction
{
    public class CenterPointModule : MonoBehaviour
    {
        [SerializeField] EasyAPI _EasyAPI; // Апишечка
        [SerializeField] UnityEngine.UI.Image Circle; // Круг, который будет заполняться по мере нажатия
        [SerializeField] float UpdateSpeedInSec = 0.01f; // Частота перемещения поинтера.

        public float maxDistance = 500f; // Максимальная дистанция прокидывания луча от точки

        private Vector3 CenterPointPos = new Vector3(0, 0, 0); // Внутренняя переменная для рассчетов дрожания (актуальное положение)
        private Vector3 CenterPointOldPos = new Vector3(0, 0, 0); // Внутренняя переменная для рассчетов дрожания (прошлое положение)

        private Vector3 startPoint; // Стартовая точка (обычно игрок)
        private Vector3 endPoint; // Промежуточная точка (Рука или центр)
        private Vector3 direction; // Направление (Высчитанное)
        private Ray ray; // Луч для кликов
        private RaycastHit[] hits; // Массив столкновений луча

        private bool DoUpdateTimer = true; // Таймер для нажатий
        private bool DoUpdateTimerForJoystick = true; // Таймер для нажатий с джойстиком

        UnityEngine.Color CenterPointColor; // Цвет для затухания и подсвечивания управляющей точки (Поинтера)

        void Start()
        {
            InvokeRepeating("Update_Every_10ms", 2, UpdateSpeedInSec); // Таймер вызова "обновлятора" вместо Update
        }

        public void UpdateTimer() // Сбросить таймер клика
        {
            DoUpdateTimer = true;
            Debug.Log("Clicking timer updated");
        }

        public void UpdateTimerForJoystick() // Сбросить таймер джойстика
        {
            DoUpdateTimerForJoystick = true;
            Debug.Log("Joystick timer updated");
        }

        private void ThrowRayCast() // Бросить луч для клика в сторону интерфейса
        {
            startPoint = _EasyAPI.GetPlayerObject().transform.position; // Начальная точка (Позиция игрока)
            endPoint = CenterPointPos; // Промежуточная точка для определения направления
            direction = (endPoint - startPoint).normalized; // Вычисляем направление луча
            ray = new Ray(startPoint, direction); // Создаем луч, начинающийся с startPoint и направленный в вычисленном направлении
            hits = Physics.RaycastAll(ray, maxDistance); // Получаем все попадания луча

            //Debug.DrawLine(startPoint, startPoint + direction * maxDistance, UnityEngine.Color.red); // Дебаг для видимости луча
        }

        private void CheckWithInterface() // Проверка, есть ли на пути интерфейс
        {
            if (!(!_EasyAPI.GetTrackingType() && !_EasyAPI.IsHandDetected())) // Проверка на наличие руки в кадре, во избежание исчезновения руки
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Interface") || hit.collider.CompareTag("Browser")) // Если на пути что-то есть
                    {
                        InvokeRepeating("FadeIn", 0, 0.05f); // Показываем точку
                        return;
                    }
                }
            }
           InvokeRepeating("FadeOut", 0, 0.1f); // Интерфейса нет - точку надо скрыть
        }

        private void DoClick() // Кликаем!
        {
            foreach (RaycastHit hit in hits) // Перебираем все столкновения с интерфейсами
            {
                if (hit.collider.CompareTag("StartThatPane")) // Если есть панель, которую нужно открыть...
                {
                    if (!hit.collider.GetComponentInChildren<Canvas>(true).gameObject.activeSelf) hit.collider.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
                }
                if (hit.collider.CompareTag("Interface")) // Если на пути объект интерфейса...
                {
                    ExecuteEvents.Execute(hit.collider.gameObject.GetComponent<UnityEngine.UI.Button>().gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler); // Do clicking on a button (If its a button) by emulating mouse event
                    _EasyAPI.PlayClickSound(); // Играем звук клика через API
                    break;
                }
                if (hit.collider.CompareTag("Browser")) // Если на пути браузер...
                {
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoClickByWorldCoordinates(hit.point); // Тыкаем в браузер через API
                }
            }
        }

        private void PrepareForClick() // Подготавливаемся к клику (Заполняем круг)
        {
            CenterPointPos = _EasyAPI.GetCenterPoint().transform.position; // Устанавливаем позицию Поинтера
            if (Input.GetAxis("Submit") > 0 || Input.GetAxis("Cancel") > 0 || Input.GetAxis("Fire1") > 0 || Input.GetAxis("Jump") > 0 || Input.GetAxis("Fire2") > 0 || Input.GetAxis("Fire3") > 0)
            { // Работа для джойстика/тачпада
                if (DoUpdateTimer)
                {
                    CancelInvoke("UpdateTimerForJoystick");
                    DoClick(); // Иллюзия работы геймпада
                    DoUpdateTimer = false;
                    InvokeRepeating("UpdateTimer", 1, 0);
                    DoUpdateTimerForJoystick = false;
                    InvokeRepeating("UpdateTimerForJoystick", 15, 0);
                }
            }
            if (Vector3.Distance(CenterPointPos, CenterPointOldPos) < 1 && DoUpdateTimer && DoUpdateTimerForJoystick && CenterPointColor.a >= 1) // Защита от дрожжания
            {
                //Debug.Log(Vector3.Distance(CenterPointPos, CenterPointOldPos));
                if (Circle.fillAmount >= 1) // Если круг полностью заполнен...
                {
                    Circle.fillAmount = 0; // Сбрасываем его
                    DoUpdateTimer = false; // Обновляем таймер
                    InvokeRepeating("UpdateTimer", 2, 0);
                    DoClick(); // Кликаем
                }
                else Circle.fillAmount += 0.005f; // В ином лучае продолжаем заполнять
            }
            else if (Circle.fillAmount - 0.02f >= 0) Circle.fillAmount -= 0.02f; // Если дрожание точки превышает минимум то перестаём заполнять круг и уменьшаем его
            CenterPointOldPos = CenterPointPos;
        }

        public void Update_Every_10ms()
        {
            if (!_EasyAPI.GetTrackingType()) _EasyAPI.GetCenterPoint().transform.position = _EasyAPI.GetMainFingerAnnotation().transform.position; // Устанавливаем позицию Поинтера на позицию указательного пальца руки, если сейчас трекинг рук и рука в кадре
            ThrowRayCast();
            CheckWithInterface();
            PrepareForClick();
        }
        public void FadeIn() // Плавное появление
        {
            CenterPointColor = _EasyAPI.GetCenterPoint().GetComponent<UnityEngine.UI.Image>().color;
            CenterPointColor.a += 0.2f;
            _EasyAPI.GetCenterPoint().GetComponent<UnityEngine.UI.Image>().color = CenterPointColor;
            if (CenterPointColor.a >= 1) CancelInvoke("FadeIn");
        }

        public void FadeOut() // Плавное исчезание
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

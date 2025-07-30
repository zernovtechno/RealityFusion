using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityFusion.API;

namespace RealityFusion.BaseUI
{
    public class FollowingPanelScript : MonoBehaviour
    {
        [SerializeField] EasyAPI _EasyAPI;
        [SerializeField] private float rotationSpeed = 5f;  // Скорость поворота (чем меньше, тем плавнее)
        [SerializeField] private float maxAngleDifference = 30f;  // Макс. угол отставания (опционально)
        [SerializeField] private float minAngleToStart = 10f;     // Минимальный угол для начала поворота
        [SerializeField] private float minRotationSpeed = 0.5f;
        private Quaternion targetRotation;
        private Quaternion currentRotation;
        private float angle;
        private float t;
        private bool _isRotating;  // Флаг, что объект в процессе поворота
        private void Update()
        {
            if (_EasyAPI.GetPlayerObject().transform == null)
                return;

            // Текущий поворот follower и целевой поворот target
            targetRotation = _EasyAPI.GetPlayerObject().transform.localRotation;
            currentRotation = transform.localRotation;
            angle = Quaternion.Angle(currentRotation, targetRotation);
            //Debug.Log(angle);
            // Если разница в угле меньше minAngleToStart - не поворачиваем
            if (angle > minAngleToStart)
            {
                _isRotating = true;
            }

            // Если поворот активен, вращаемся до точного совпадения
            if (_isRotating)
            {
                // Динамическая скорость: 
                // - Чем больше угол, тем быстрее поворот (но не ниже minRotationSpeed)
                float dynamicSpeed = Mathf.Lerp(
                    minRotationSpeed,
                    rotationSpeed,
                    Mathf.Clamp01((angle - minAngleToStart) / (maxAngleDifference - minAngleToStart))
                );

                // Плавный поворот с динамической скоростью
                transform.rotation = Quaternion.RotateTowards(
                    currentRotation,
                    targetRotation,
                    dynamicSpeed * Time.deltaTime * 100f // Умножаем на 100, т.к. RotateTowards работает в градусах/сек
                );

                // Если угол стал очень маленьким - финальное выравнивание
                if (angle < 0.1f)
                {
                    transform.rotation = targetRotation;
                    _isRotating = false;
                }
            }
        }
    }
}

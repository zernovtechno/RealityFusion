using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityFusion.API;

namespace RealityFusion.BaseUI
{
    public class FollowingPanelScript : MonoBehaviour
    {
        [SerializeField] EasyAPI _EasyAPI;
        [SerializeField] private float rotationSpeed = 5f;  // �������� �������� (��� ������, ��� �������)
        [SerializeField] private float maxAngleDifference = 30f;  // ����. ���� ���������� (�����������)
        [SerializeField] private float minAngleToStart = 10f;     // ����������� ���� ��� ������ ��������
        [SerializeField] private float minRotationSpeed = 0.5f;
        private Quaternion targetRotation;
        private Quaternion currentRotation;
        private float angle;
        private float t;
        private bool _isRotating;  // ����, ��� ������ � �������� ��������
        private void Update()
        {
            if (_EasyAPI.GetPlayerObject().transform == null)
                return;

            // ������� ������� follower � ������� ������� target
            targetRotation = _EasyAPI.GetPlayerObject().transform.localRotation;
            currentRotation = transform.localRotation;
            angle = Quaternion.Angle(currentRotation, targetRotation);
            //Debug.Log(angle);
            // ���� ������� � ���� ������ minAngleToStart - �� ������������
            if (angle > minAngleToStart)
            {
                _isRotating = true;
            }

            // ���� ������� �������, ��������� �� ������� ����������
            if (_isRotating)
            {
                // ������������ ��������: 
                // - ��� ������ ����, ��� ������� ������� (�� �� ���� minRotationSpeed)
                float dynamicSpeed = Mathf.Lerp(
                    minRotationSpeed,
                    rotationSpeed,
                    Mathf.Clamp01((angle - minAngleToStart) / (maxAngleDifference - minAngleToStart))
                );

                // ������� ������� � ������������ ���������
                transform.rotation = Quaternion.RotateTowards(
                    currentRotation,
                    targetRotation,
                    dynamicSpeed * Time.deltaTime * 100f // �������� �� 100, �.�. RotateTowards �������� � ��������/���
                );

                // ���� ���� ���� ����� ��������� - ��������� ������������
                if (angle < 0.1f)
                {
                    transform.rotation = targetRotation;
                    _isRotating = false;
                }
            }
        }
    }
}

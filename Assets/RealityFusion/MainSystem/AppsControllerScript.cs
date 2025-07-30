using Mediapipe.Unity.Sample.HandTracking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RealityFusion.API;

namespace RealityFusion.BaseUI
{
    public class AppsControllerScript : MonoBehaviour
    {
        [SerializeField] GameObject ObjectsPoint;

        [SerializeField] GameObject Keyboard;
        [SerializeField] GameObject KeyboardPoint;

        [SerializeField] EasyAPI _EasyAPI;
        [SerializeField] KeyboardScript _KeyboardScript;

        public void TurnPrefab(GameObject _prefab)
        {
            Instantiate(_prefab, ObjectsPoint.transform.position, ObjectsPoint.transform.rotation, _EasyAPI.GetInterfaceObject().transform);
        }

        public void TurnObject(GameObject _object)
        {
            _object.transform.position = ObjectsPoint.transform.position;
            _object.transform.rotation = ObjectsPoint.transform.rotation;
            _object.SetActive(!_object.activeSelf);
            _EasyAPI.SetLastContactedMenu(_object.gameObject.GetComponent<Canvas>());
        }

        public void TurnKeyboard()
        {
            Keyboard.transform.position = KeyboardPoint.transform.position;
            Keyboard.transform.rotation = KeyboardPoint.transform.rotation;
            Keyboard.SetActive(!Keyboard.activeSelf);
        }

        public void TurnKeyboard(bool active)
        {
            Keyboard.transform.position = KeyboardPoint.transform.position;
            Keyboard.transform.rotation = KeyboardPoint.transform.rotation;
            Keyboard.SetActive(active);
        }
    }
}

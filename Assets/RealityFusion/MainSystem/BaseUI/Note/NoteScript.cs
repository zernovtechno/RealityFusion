using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityFusion.API;
using TMPro;

namespace RealityFusion.BaseUI
{
    public class NoteScript : MonoBehaviour
    {
        private EasyAPI _EasyAPI;
        [SerializeField] TextMeshProUGUI _Text;

        public void Start()
        {
            _EasyAPI = GetComponentInParent<EasyAPI>();
        }

        public void DestroyApp()
        {
            Destroy(this.transform.parent);
        }

        public void PrepareKeyboard()
        {
            _EasyAPI.GetAppsControllerScript().TurnKeyboard();
            _EasyAPI.GetKeyboardScript().SetMode(_Text);
        }
    }
}
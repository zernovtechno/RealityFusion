using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing.QrCode.Internal;
using UnityEngine.TextCore.Text;
using RealityFusion.API;

namespace RealityFusion.BaseUI
{
    public class KeyboardScript : MonoBehaviour
    {
        [SerializeField] EasyAPI _EasyAPI;
        [SerializeField] Button[] ButtonsList;
        [SerializeField] TextMeshProUGUI TextPane;

        private string KeysListEng = "qwertyuiop[]asdfghjkl;'zxcvbnm,.?";
        private string KeysListRu = "йцукенгшщзхъфывапролджэячсмитьбю";
        private string ActualKeysList = "";
        public TextMeshProUGUI SelectedApp;
        public string mode;

        bool Caps = false;
        bool Shift = false;

        bool ru = true;
        // Start is called before the first frame update
        void Start()
        {
            if (PlayerPrefs.HasKey("RuLanguage")) KeysListRu = PlayerPrefs.GetString("RuLanguage");
            SetCharactersList(KeysListRu);
        }

        public void ChangeCharactersList()
        {
            ru = !ru;
            if (ru) SetCharactersList(KeysListRu);
            else SetCharactersList(KeysListEng);
        }

        public void SetMode(string _mode)
        {
            mode = _mode;
            TextPane.text = "Режим: " + _mode;
        }
        public void SetMode(TextMeshProUGUI _app)
        {
            mode = "App";
            SelectedApp = _app;
            TextPane.text = "Приложение: " + _app.gameObject.name;
        }

        public void SetLanguage(string _CharactersList)
        {
            ru = true;
            KeysListRu = _CharactersList;
            SetCharactersList(_CharactersList);
            PlayerPrefs.SetString("RuLanguage", _CharactersList);
        }

        public void SetCharactersList(string CharactersList)
        {
            string _CharactersList = CharactersList;
            Debug.Log("Characters Set! " + Shift + " " + Caps);
            if (Shift)
                _CharactersList = CharactersList.ToUpper();
            if (Caps)
                _CharactersList = CharactersList.ToUpper();

            for (int i = 0; i < CharactersList.Length; i++)
            {
                ButtonsList[i].GetComponentInChildren<TextMeshProUGUI>().text = _CharactersList.ToCharArray()[i].ToString();
            }
            ActualKeysList = CharactersList;
        }


        public void SendKeyCode(Button _senderObject)
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand(_senderObject.GetComponentInChildren<TextMeshProUGUI>().text);
                    break;
                case "App":
                    SelectedApp.text += _senderObject.GetComponentInChildren<TextMeshProUGUI>().text;
                    break;
            }
            if (Shift)
            {
                Shift = false;
                SetCharactersList(ActualKeysList);
            }
        }


        public void ClickTab()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("Tab");
                    break;
            }
        }

        public void ClickShift()
        {
            Shift = !Shift;
            SetCharactersList(ActualKeysList);
        }
        public void ClickCtrl()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("Control");
                    break;
            }
        }
        public void ClickCapsLock()
        {
            Caps = !Caps;
            SetCharactersList(ActualKeysList);
        }
        public void ClickBackSpace()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("Backspace");
                    break;
                case "App":
                    SelectedApp.text = SelectedApp.text.Remove(SelectedApp.text.Length - 1);
                    break;
            }
        }

        public void ClickEnter()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("Enter");
                    break;
                case "App":
                    SelectedApp.text += "\n";
                    break;
            }
        }

        public void ClickSpace()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand(" ");
                    break;
                case "App":
                    SelectedApp.text += " ";
                    break;
            }
        }




        public void ClickUp()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("ArrowUp");
                    break;
            }
        }

        public void ClickDown()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("ArrowDown");
                    break;
            }
        }
        public void ClickRight()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("ArrowRight");
                    break;
            }
        }
        public void ClickLeft()
        {
            switch (mode)
            {
                case "Browser":
                    _EasyAPI.GetCanvasWorldSpaceDemo().DoSendCommand("ArrowLeft");
                    break;
            }
        }
    }
}
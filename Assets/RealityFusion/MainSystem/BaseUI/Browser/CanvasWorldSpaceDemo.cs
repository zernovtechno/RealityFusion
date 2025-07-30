/*Copyright(c) 2021 Vuplex Inc.All rights reserved.
*
*Licensed under the Vuplex Commercial Software Library License, you may
* not use this file except in compliance with the License. You may obtain
* a copy of the License at
*
* https://vuplex.com/commercial-library-license
*
*Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using Google.Protobuf.WellKnownTypes;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using Mediapipe.Unity.Sample.HandTracking;
using TMPro;
using RealityFusion.API;

namespace Vuplex.WebView.Demos
{

    /// <summary>
    /// Sets up the CanvasWorldSpaceDemo scene, which displays a `CanvasWebViewPrefab`
    /// in a world space canvas.
    /// </summary>
    public class CanvasWorldSpaceDemo : MonoBehaviour
    {

        CanvasWebViewPrefab _canvasWebViewPrefab;
        HardwareKeyboardListener _hardwareKeyboardListener;
        [SerializeField] GameObject BrowserPointerObject;
        [SerializeField] TextMeshProUGUI URL;
        private EasyAPI _EasyAPI;

        async void OnEnable()
        {
            _EasyAPI = GetComponentInParent<EasyAPI>();
            _EasyAPI.SetCanvasWorldSpaceDemo(this);
            // Use a desktop User-Agent to request the desktop versions of websites.
            // https://developer.vuplex.com/webview/Web#SetUserAgent
            Web.SetUserAgent(false);

            // The CanvasWebViewPrefab's `InitialUrl` property is set via the editor, so it
            // automatically loads that URL when it initializes.
            _canvasWebViewPrefab = GetComponentInParent<CanvasWebViewPrefab>();
            _setupKeyboards();

            // Wait for the CanvasWebViewPrefab to initialize, because the CanvasWebViewPrefab.WebView property
            // is null until the prefab has initialized.
            await _canvasWebViewPrefab.WaitUntilInitialized();

            // The CanvasWebViewPrefab has initialized, so now we can use the IWebView APIs
            // using its CanvasWebViewPrefab.WebView property.
            // https://developer.vuplex.com/webview/IWebView
            _canvasWebViewPrefab.WebView.UrlChanged += (sender, eventArgs) => {
                Debug.Log("URL changed: " + eventArgs.Url);
            };
        }

        void _setupKeyboards()
        {

            // Send keys from the hardware (USB or Bluetooth) keyboard to the webview.
            // Use separate `KeyDown()` and `KeyUp()` methods if the webview supports
            // it, otherwise just use `IWebView.HandleKeyboardInput()`.
            // https://developer.vuplex.com/webview/IWithKeyDownAndUp
            _hardwareKeyboardListener = HardwareKeyboardListener.Instantiate();
            _hardwareKeyboardListener.KeyDownReceived += (sender, eventArgs) => {
                var webViewWithKeyDown = _canvasWebViewPrefab.WebView as IWithKeyDownAndUp;
                if (webViewWithKeyDown == null)
                {
                    _canvasWebViewPrefab.WebView.HandleKeyboardInput(eventArgs.Value);
                }
                else
                {
                    webViewWithKeyDown.KeyDown(eventArgs.Value, eventArgs.Modifiers);
                }
            };
            _hardwareKeyboardListener.KeyUpReceived += (sender, eventArgs) => {
                var webViewWithKeyUp = _canvasWebViewPrefab.WebView as IWithKeyDownAndUp;
                if (webViewWithKeyUp != null)
                {
                    webViewWithKeyUp.KeyUp(eventArgs.Value, eventArgs.Modifiers);
                }
            };
        }

        public void ChangeTag()
        {
            if (_canvasWebViewPrefab.gameObject.tag == "Browser")
            _canvasWebViewPrefab.gameObject.tag = "Untagged";
            else
            _canvasWebViewPrefab.gameObject.tag = "Browser";
        }

        public void DoClickByWorldCoordinates(Vector3 Cords)
        {
            BrowserPointerObject.transform.position = Cords;
            DoClickBrowser(new Vector2((360 + BrowserPointerObject.transform.localPosition.x) / 720, (-BrowserPointerObject.transform.localPosition.y) / 350));
        }

        public void DoClickBrowser(Vector2 point)
        {
            _EasyAPI.SetCanvasWorldSpaceDemo(this);
            _canvasWebViewPrefab.WebView.Click(point);
        }

        public void GoBack()
        {
            _canvasWebViewPrefab.WebView.GoBack();
        }

        public void GoDown()
        {
            for (int i = 0; i < 5; i++)
            {
                _EasyAPI.GetKeyboardScript().ClickDown();
            }
        }
        public void GoUp()
        {
            for (int i = 0; i < 5; i++)
            {
                _EasyAPI.GetKeyboardScript().ClickUp();
            }
        }

        public void OpenKeyboard()
        {
            _EasyAPI.SetCanvasWorldSpaceDemo(this);
            _EasyAPI.GetAppsControllerScript().TurnKeyboard(true);
            _EasyAPI.GetKeyboardScript().SetMode("Browser");
        }

        public void WorkWithURL()
        {
            _EasyAPI.SetCanvasWorldSpaceDemo(this);
            _EasyAPI.GetAppsControllerScript().TurnKeyboard();
            _EasyAPI.GetKeyboardScript().SetMode(URL);
        }

        public void GoForward()
        {
            _canvasWebViewPrefab.WebView.GoForward();
        }

        public void Reload()
        {
            _canvasWebViewPrefab.WebView.Reload();
        }

        public void OpenGoogle()
        {
            _canvasWebViewPrefab.WebView.LoadUrl("https://www.google.com");
        }

        public void OpenYandex()
        {
            _canvasWebViewPrefab.WebView.LoadUrl("https://ya.ru");
        }
        public void DoSendCommand(string command)
        {
            _canvasWebViewPrefab.WebView.HandleKeyboardInput(command);
        }

        public void OpenURLByTMP(TextMeshProUGUI TextInput)
        {
            OpenURL(TextInput.text);
        }

        public void OpenURL(string url)
        {
            _canvasWebViewPrefab.WebView.LoadUrl(url);
        }
    }
}
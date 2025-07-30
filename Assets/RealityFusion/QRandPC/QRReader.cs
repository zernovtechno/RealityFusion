
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using ZXing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Rendering;
using static Mediapipe.TfLiteInferenceCalculatorOptions.Types.Delegate.Types;
using UnityEngine.Experimental.Rendering;
using RealityFusion.API;

namespace Mediapipe.Unity
{
    public class QRReader : MonoBehaviour
    {
        [SerializeField] private MJPEGStreamDecoder MJPEGSD;
        [SerializeField] private Text QRResult;
        [SerializeField] private GameObject BarCodeReader;
        [SerializeField] private GameObject ScreenCaster;
        private EasyAPI _EasyAPI;

        private Result _QRScanResult;
        private IPAddress IPChecker;

        private void Start()
        {
            _EasyAPI = GetComponentInParent<EasyAPI>();
        }

        private void Awake()
        {
            qragain();
        }

        private void Update()
        {
            if (BarCodeReader.activeSelf)
            {
                try
                {
                    _QRScanResult = _EasyAPI.GetQRCodeDecodeResult();
                    if (_EasyAPI.GetQRCodeDecodeResult() != null)
                    {
                        MJPEGSD.stop = false;
                        QRResult.text = "QR декодирован.";
                        Debug.Log(_QRScanResult.Text);
                        MJPEGSD.StartStream(_QRScanResult.Text);
                        BarCodeReader.gameObject.SetActive(false);
                        ScreenCaster.gameObject.SetActive(true);
                    }
                    else if (_QRScanResult != null)
                    {
                        QRResult.text = $"QR не является трансляцией. Содержание QR:{_QRScanResult.Text}";
                    }
                }
                catch (Exception Ex) { }
            }
            if (MJPEGSD.stop)
            {
                QRResult.text = "Сбой подключения :(";
                BarCodeReader.gameObject.SetActive(true);
                ScreenCaster.gameObject.SetActive(false);
            }
        }

        public void qragain()
        {
            ScreenCaster.SetActive(false);
            BarCodeReader.SetActive(true);
        }
    }
}

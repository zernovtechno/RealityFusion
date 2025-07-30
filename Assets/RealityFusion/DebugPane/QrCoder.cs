using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityFusion.API;
using ZXing;

public class QrCoder : MonoBehaviour
{
    [SerializeField] EasyAPI _EasyAPI;
    [SerializeField] TMPro.TextMeshProUGUI ResultText;
    [SerializeField] RectTransform Cube;
    // Start is called before the first frame update
    public void Read()
    {
        ResultText.text = _EasyAPI.GetQRCodeDecodeString();
    }

    public void ReadWithPoints()
    {
        try
        {
            ResultPoint resultPoint = _EasyAPI.GetQRCodeDecodeResultPoint()[0];
            Vector3 Position = new Vector3(resultPoint.X, (1000 - resultPoint.Y));
            Cube.anchoredPosition = Position;
            ResultText.text = " x = " + resultPoint.X + ", y = " + (800-resultPoint.Y);
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        //ReadWithPoints();
    }
}


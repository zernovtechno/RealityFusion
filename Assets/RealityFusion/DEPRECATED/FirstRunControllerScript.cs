using Mediapipe.Unity.Sample.HandTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using RealityFusion.API;

public class FirstRunControllerScript : MonoBehaviour
{
    [SerializeField] EasyAPI _EasyAPI;
    [SerializeField] GameObject FirstSet;

    void Start()
    {
        if (PlayerPrefs.HasKey("TrackingType"))
        {
            switch (PlayerPrefs.GetInt("TrackingType"))
            {
                case 0:
                    _EasyAPI.HandTracking();
                    break;
                case 1:
                    _EasyAPI.CenterTracking();
                    break;
            }
        }
        else
        {
            FirstSet.SetActive(true);
        }
    }
}

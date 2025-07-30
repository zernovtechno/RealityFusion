using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityFusion.API;
namespace RealityFusion.BaseUI
{
    public class VREnvScript : MonoBehaviour
    {
        private EasyAPI _EasyAPI; 
        void Start()
        {
            _EasyAPI = GetComponentInParent<EasyAPI>();
        }

        public void TurnScreen(bool State)
        {
            _EasyAPI.GetLeftScreen().SetActive(State);
            _EasyAPI.GetRightScreen().SetActive(State);
        }
    }
}
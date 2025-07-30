using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RealityFusion.BaseUI
{
    public class CloseButtonScript : MonoBehaviour
    {
        public void DestroyApp()
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

}
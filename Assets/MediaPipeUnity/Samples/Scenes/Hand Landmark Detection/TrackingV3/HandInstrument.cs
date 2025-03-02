using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.Experimental.GlobalIllumination;
using Mediapipe.Unity.Sample.HandTracking;
using Mediapipe.Unity.Sample;
using Mediapipe;
using Mediapipe.Tasks.Vision.HandLandmarker;

public class HandInstrument : MonoBehaviour
{
    [SerializeField] MultiHandLandmarkListAnnotation _MultiHandLandmarkListAnnotation;
    [SerializeField] AudioSource ClickSound;
    [SerializeField] public GameObject AnnotationLayer;
    [SerializeField] WindowPositionController WPC;
    [SerializeField] CommunicatorWithBrowser CWB;
    HandLandmarkListAnnotation _HandLandmarkListAnnotation;
    PointListAnnotation PointAnnotations;
    public bool done = false;
    public bool TrackingIsWorking = false;

    private int FingerNumber = 8;

    float HandSize;


    private void Start()
    {
        InvokeRepeating("CheckHandSize", 1, 0);
    }

    public PointAnnotation GetAnnotation(int PointNumber, int HandNumber)
    {
        return _MultiHandLandmarkListAnnotation.gameObject.GetComponentsInChildren<HandLandmarkListAnnotation>(true)[HandNumber]
            .gameObject.GetComponentInChildren<PointListAnnotation>(true)[PointNumber];
    }

    public void ReCreateInteractor(int _FingerNumber = 8)
    {
        FingerNumber= _FingerNumber;
        done = false;
    }

    private void CheckHandSize() // Check, if hand small (Maybe its a leg?)
    {
        AnnotationLayer.transform.localPosition = new Vector3(20, 0, -(700));
    }

    // Update is called once per frame
    private void Update()
    {
        if (!done)
        {
            try
            {
                _HandLandmarkListAnnotation = _MultiHandLandmarkListAnnotation.gameObject.GetComponentsInChildren<HandLandmarkListAnnotation>(true)[0];
                PointAnnotations = _HandLandmarkListAnnotation.gameObject.GetComponentInChildren<PointListAnnotation>(true);

                Interactor FingerPointInterator = PointAnnotations[FingerNumber].gameObject.AddComponent<Interactor>();
                FingerPointInterator.ClickSound = ClickSound;
                FingerPointInterator.AnnotationLayer = AnnotationLayer;
                FingerPointInterator.WPC = WPC;
                FingerPointInterator.CWB = CWB;

                Rigidbody ColliderRigidBody = PointAnnotations[FingerNumber].gameObject.AddComponent<Rigidbody>();
                ColliderRigidBody.isKinematic = true;
                ColliderRigidBody.useGravity = false;

                SphereCollider FingerSphereCollider = PointAnnotations[FingerNumber].gameObject.AddComponent<SphereCollider>();
                FingerSphereCollider.isTrigger= true;
                FingerSphereCollider.radius = 1f;

                GetAnnotation(4, 0).gameObject.tag = "ThumbFinger";
                GetAnnotation(4, 0).GetComponent<SphereCollider>().radius = 1.5f;
                done = true;
            }
            catch
            {
              done = false;
            }
        }
        if (done && AnnotationLayer.gameObject.activeSelf && _MultiHandLandmarkListAnnotation.gameObject.activeSelf)
        {
            TrackingIsWorking = true;
        }
        else TrackingIsWorking = false;
    }
}

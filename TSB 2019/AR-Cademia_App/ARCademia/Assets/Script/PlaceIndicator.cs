using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;

public class PlaceIndicator : MonoBehaviour
{
    public GameObject placeIndicator;

    private ARSession arSession;
    private ARSessionOrigin arSessionOrigin;
    private ARPlane arPlane;
    private ARRaycastManager arRaycast;

    // Start is called before the first frame update
    void Start()
    {
        arSession = GetComponent<ARSession>();
        arSessionOrigin = GetComponent<ARSessionOrigin>();
        arPlane = GetComponent<ARPlane>();
        arRaycast = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

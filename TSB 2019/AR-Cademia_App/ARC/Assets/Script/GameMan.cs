using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class GameMan : MonoBehaviour
{
    [SerializeField] private HandTrack hand;                        //tangan

    public GameObject objectToPlace;                               //object yang ingin di taruh
    public GameObject pointer;                                     //point

    [SerializeField] private float k_PrefabRotation = 180.0f;       //rotasi object

    public GameObject point;                                        //point untuk membandingkan posisi real dan virtual

    bool pointPlaced;                                               //point sudah di pasang?

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   //set frame rate menjadi 60
        
        objectToPlace.gameObject.SetActive(false);
        pointPlaced = false;                //memastikan di awal point belum di pasang
    }

    // Update is called once per frame
    void Update()
    {
        _UpdateApplicationLifeCycle();  //memastikan smartphone tetap hidup

        TrackableHit hit;               //plane atau benda datar yang terdeteksi
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;
        
        if (Frame.Raycast(0.5f, 0.5f, raycastFilter, out hit)){     //membuar raycast dari tengah kamera ke object datar yang terkena hit
            if (!pointPlaced)                                               //jika point belum dipasang
            {
                point.transform.position = hit.Pose.position;               //mengatur posisi point
                point.transform.Rotate(0, k_PrefabRotation, 0, Space.Self); //mengatur rotasi point

                var anchor = hit.Trackable.CreateAnchor(hit.Pose);          //membuat anchor
                point.transform.parent = anchor.transform;                  //membuat point menjadi "child" dari anchor
           
                pointPlaced = true;                                         //point telah dipasang
            }
        }

        if (hand.started)
        {
            objectToPlace.gameObject.SetActive(true); pointer.gameObject.SetActive(false);
        }
        else objectToPlace.gameObject.SetActive(false); pointer.gameObject.SetActive(true);
    }

    private void _UpdateApplicationLifeCycle()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class GameMan : MonoBehaviour
{
    [SerializeField] private HandTrack hand;                        //tangan
    [SerializeField] private GameObject objectToPlace;              //object yang ingin di taruh
    [SerializeField] private float k_PrefabRotation = 180.0f;       //rotasi object

    public GameObject point;                                        //point untuk membandingkan posisi real dan virtual

    bool pointPlaced;                                               //point sudah di pasang?

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   //set frame rate menjadi 60

        point.gameObject.SetActive(false);  //menonaktifkan point di awal
        pointPlaced = false;                //memastikan di awal point belum di pasang
    }

    // Update is called once per frame
    void Update()
    {
        _UpdateApplicationLifeCycle();  //memastikan smartphone tetap hidup

        TrackableHit hit;               //plane atau benda datar yang terdeteksi
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;        //filter untuk raycast
        
        if (Frame.Raycast(0.5f, 0.5f, raycastFilter, out hit)){     //mendapatkan tengah kamera
            if (hand.started)                                       //jika tangan sudah mulai
            {
                var gameObject = Instantiate(objectToPlace, hit.Pose.position, hit.Pose.rotation);  //menempatkan object yang ingin di taruh
                gameObject.transform.Rotate(0, k_PrefabRotation, 0, Space.Self);                    //mengatur rotasi object yang ditaruh
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);                                  //membuat anchor

                gameObject.transform.parent = anchor.transform;                                     //membuat object menjadi "child" dari anchor
            }
            else //jika belum dimulai
            {
                if (!pointPlaced)                                               //jika point belum dipasang
                {
                    point.SetActive(true);                                      //mengaktifkan point
                    point.transform.position = hit.Pose.position;               //mengatur posisi point
                    point.transform.Rotate(0, k_PrefabRotation, 0, Space.Self); //mengatur rotasi point

                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);          //membuat anchor
                    point.transform.parent = anchor.transform;                  //membuat point menjadi "child" dari anchor
                }
            }
        }
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

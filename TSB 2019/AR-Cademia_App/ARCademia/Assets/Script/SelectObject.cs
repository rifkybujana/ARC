using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SelectObject : MonoBehaviour
{
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.height/2, Screen.width/2));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Debug.DrawRay(ray.origin, hit.point);
    }
}

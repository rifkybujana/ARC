using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    public HandTrack hand;

    public GameObject[] Object;

    // Update is called once per frame
    void Update()
    {
        if (hand.started)
        {
            Object[0].gameObject.SetActive(true);
            Object[1].gameObject.SetActive(false);
        }
        else
        {
            Object[1].gameObject.SetActive(true);
            Object[0].gameObject.SetActive(false);
        }
    }
}

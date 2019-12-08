using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Text text;
    public BluetoothCon bt;

    // Update is called once per frame
    void Update()
    {
        text.text = bt.data;
    }
}

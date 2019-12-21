using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Atom : MonoBehaviour
{
    public GameObject[] Electron;
    public GameObject IntiAtom;
    public GameObject GameManager;
    public Controller ButtonMan;
    

    // Update is called once per frame
    void Update()
    {
        if (ButtonMan.isRotate)
        {
            Electron[0].transform.Rotate(new Vector3(1, 0, 0), Space.Self);
            Electron[1].transform.Rotate(new Vector3(-1, 0, 0), Space.Self);
            Electron[2].transform.Rotate(new Vector3(0, 1, 0), Space.Self);

            IntiAtom.transform.Rotate(new Vector3(1, 1, 1), Space.Self);
        }
    }
}

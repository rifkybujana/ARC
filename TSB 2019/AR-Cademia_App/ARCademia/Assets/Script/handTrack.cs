using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handTrack : MonoBehaviour
{
    [SerializeField] private BluetoothCon bluetooth;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject petunjuk;
    [Space(5)]
    [SerializeField] private Material[] mat;
    [Space(5)]
    [SerializeField] private GameObject[] ObjectInfo;
    [Space(5)]
    [SerializeField] private GameObject Canvas;
    [SerializeField] private string objectTag;
    [SerializeField] private string buttonTag;
    [SerializeField] private Text buttonText;
    [Space(5)]
    [SerializeField] private GameObject BlockPoint;


    [HideInInspector] public Vector3 handPos;
    [HideInInspector] public Quaternion handRot;
    [HideInInspector] public string handState;
    [HideInInspector] public bool buttonState;

    private GameObject target;
    private Material targetMat;
    private Vector3 objectTransform;
    private Vector3 selisihJarak;

    private int[] scale;
    private int[] rotScale;

    private bool isMove;
    private bool isGrep;
    private bool isChoose;
    private bool isStart;

    private bool checkScale;

    #region data
    string data;
    int xPos, yPos, zPos;
    int xRot, yRot, zRot;

    int[] Pos;
    int[] Rot;
    #endregion


    private void Start()
    {
        #region set all state to false
        isChoose = false;
        isMove = false;
        isGrep = false;
        isStart = false;
        buttonState = false;
        checkScale = false;
        #endregion

        #region set default state
        if (hand == null) hand = GetComponentInParent<GameObject>();

        Canvas.gameObject.SetActive(false);
        petunjuk.gameObject.SetActive(true);
        for (int i = 0; i < ObjectInfo.Length; i++)
        {
            ObjectInfo[i].SetActive(false);
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Get Data
        data = bluetooth.data;
        string[] splitData = data.Split(char.Parse(","));
        handState = splitData[6];
        if (handState == "Start") isStart = true;
        #endregion

        #region Track Position Of Hands
        int.TryParse(splitData[0], out xPos);
        int.TryParse(splitData[1], out yPos);
        int.TryParse(splitData[2], out zPos);
        #endregion

        #region Track Rotation Of Hands
        int.TryParse(splitData[3], out xRot);
        int.TryParse(splitData[4], out yRot);
        int.TryParse(splitData[5], out zRot);
        #endregion

        #region Update Position And Rotation of the hand
        if (isStart)
        {
            if(checkScale == false)
            {
                scale[0] = (int)BlockPoint.transform.position.x / xPos;
                scale[1] = (int)BlockPoint.transform.position.y / yPos;
                scale[2] = (int)BlockPoint.transform.position.z / zPos;

                rotScale[0] = (int)BlockPoint.transform.rotation.x / xRot;
                rotScale[1] = (int)BlockPoint.transform.rotation.y / yRot;
                rotScale[2] = (int)BlockPoint.transform.rotation.z / zRot;

                checkScale = true;
            }else if (checkScale)
            {
                Pos[0] = xPos * scale[0];
                Pos[1] = yPos * scale[1];
                Pos[2] = zPos * scale[2];

                Rot[0] = xRot * scale[0];
                Rot[1] = yRot * scale[1];
                Rot[2] = zRot * scale[2];

                handPos = new Vector3(hand.transform.position.x + Pos[0], hand.transform.position.y + Pos[1], hand.transform.position.z + Pos[2]);
                handRot = new Quaternion(hand.transform.rotation.x + Rot[0], hand.transform.rotation.y + Rot[1], hand.transform.rotation.z + Rot[2], 0);
                hand.transform.position = handPos;
                hand.transform.rotation = handRot;
                petunjuk.gameObject.SetActive(false);
            }
        }
        #endregion

        #region object interaction
        if (isMove)
        {
            selisihJarak = target.transform.position - hand.transform.position;
            objectTransform = handPos + selisihJarak;
            target.gameObject.transform.position = objectTransform;
        }
        else if (isGrep)
        {
            selisihJarak = target.transform.position - hand.transform.position;
            Vector3 scaling = handPos - selisihJarak;
            float totalScale = scaling.x + scaling.y + scaling.z;
            float averageScale = totalScale / 3f;
            target.gameObject.transform.localScale += new Vector3(averageScale, averageScale, averageScale);
        }
        else if (isChoose)
        {
            Canvas.gameObject.SetActive(true);

            for (int i = 0; i < mat.Length; i++)
            {
                if(targetMat = mat[i]) ObjectInfo[i].SetActive(true);
                else ObjectInfo[i].SetActive(false);
            }
        }

        if (buttonState)
        {
            buttonText.text = "Rotate";
        }
        else
        {
            buttonText.text = "Stop";
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Get Data Object That Choosen
        if(other.tag == objectTag)
        {
            target = other.gameObject;

            targetMat = target.GetComponent<Renderer>().material;
            if(handState == "Move")
            {
                isMove = true;
                isChoose = false;
                isGrep = false;
            }else if(handState == "Grep")
            {
                isMove = false;
                isChoose = false;
                isGrep = true;
            }
            else if(handState == "Choose")
            {
                isMove = false;
                isChoose = true;
                isGrep = false;
            }
            else
            {
                isMove = false;
                isChoose = false;
                isGrep = false;
            }
        }else if(other.tag == buttonTag)
        {
            if(buttonState == false) buttonState = true; 
            else buttonState = false; 
        }
        #endregion
    }

    private void OnTriggerExit(Collider other)
    {
        #region set data object to null if not touched anymore
        if (other.tag == objectTag)
        {
            target = null;

            isMove = false;
            isChoose = false;
            isGrep = false;
        }
        #endregion
    }
}

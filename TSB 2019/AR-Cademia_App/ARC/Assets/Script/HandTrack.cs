using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class HandTrack : MonoBehaviour
{
    [Tooltip("bluetooth manager")]
    [SerializeField] private BluetoothController BTController;  //bluetooth manager
    [Tooltip("Object tangan")]
    [SerializeField] private GameObject hand;                   //object tangan
    [Tooltip("tag object yang ingin dikendalikan")]
    [SerializeField] private string objectTag;                  //tag object yang ingin dikendalikan
    [Tooltip("Tag button yang ingin dikendalikan")]
    [SerializeField] private string buttonTag;

    [HideInInspector] public string handState;                  //status tangan
    [HideInInspector] public GameObject target;
    [HideInInspector] public GameObject buttonChoose;

    public GameObject Point;                  //point untuk membandingkan posisi tangan di real life dengan virtual 

    public Text statText;

    string[] data;

    float[] rawPos, rawRot;
    float[] posScale, rotScale;
    float[] pos, rot;

    bool check = false;                                     //memastikan bahwa hanya mengecek skala sekali saja
    [HideInInspector] public bool started = false;                                   //memulai hanya sekali
    bool isMove, isChoose, isGrep;                          //tangan sedang apa?
    bool isPlaced = false;

    private Vector3 selisihJarak;

    [Space(5)] public Text deviceNotConnected;

    // Start is called before the first frame update
    void Start()
    {
        check = false;
        started = false;
        statText.text = "Not Started";
    }

    // Update is called once per frame
    void Update()
    {
        if (BTController.device.IsConnected)            //jika sudah terhubung ke device
        {
            deviceNotConnected.gameObject.SetActive(false);     //menghilangkan tulisan "device not connected" di screen

            GetData();                                  //get data of hands

            Point = GameObject.Find("Pointer");

            if (handState == "Start") started = true;   //jika tangan memberi data "Start"

            if (started)                                //jika sudah dimulai
            {
                statText.text = "Started";
                if (Point == null) Point = GameObject.FindGameObjectWithTag("point");    //mencari objek point jika objeknya null

                if (check == false)
                {
                    checkScale(); //jika belum mengecek skala, langsung mengecek skala
                    check = true;
                }
                else                              //jika sudah mengecek skala
                {
                    statText.text = "done";

                    if (Point != null) Point.gameObject.SetActive(false);

                    getPos();                           //mengkalkulasikan untuk mendapat posisi di ruang virtual
                    getRot();                           //mengkalkulasikan untuk mendapat rotasi di ruang virtual

                    hand.transform.position = new Vector3(pos[0], pos[1], pos[2]);          //update posisi objek "tangan" di ruang virtual berdasarkan posisi asli
                    hand.transform.rotation = new Quaternion(rot[0], rot[1], rot[2], 0);    //update rotasi objek "tangan" di ruang virtual berdasarkan rotasi asli

                    if (handState != "Move" && handState != "Grep" && handState != "Choose")
                    {
                        isMove = false;
                        isChoose = false;
                        isGrep = false;
                    }

                    if (isMove)         //jika tangan menyentuh objek dan berposisi "move"
                    {
                        target.gameObject.transform.position = hand.transform.position + selisihJarak;      //mengupdate posisi objek berdasarkan tangan
                        statText.text = "Moving";
                    }
                    else if (isGrep)   //jika tangan menyentuh objek dan berposisi "grep"
                    {
                        float averageScale = (selisihJarak.x + selisihJarak.y + selisihJarak.z) / 3f;                               //menghitung rata rata selisih
                        target.gameObject.transform.localScale = new Vector3(averageScale * 2, averageScale * 2, averageScale * 2); //mengupdate ukuran objek berdasarkan posisi tangan
                        statText.text = "Scaling";
                    }
                }
            }
        }
        else
        {
            deviceNotConnected.gameObject.SetActive(true);      //memunculkan tulisan di screen "device not connected"
        }
    }
    
    /// <summary>
    /// jika menyentuh object
    /// </summary>
    /// <param name="other">object yang disentuh</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == objectTag) //jika objek tag = tag dari objek
        {
            target = other.gameObject;                                                                          //target = objek yang disentuh

            selisihJarak = target.transform.position - hand.transform.position;                                 //mengkalkulasikan jarak antara objek dengan tangan

            if (handState == "Move")        //jika tangan berbentuk "move"
            {
                isMove = true;
                isChoose = false;
                isGrep = false;
            }
            else if (handState == "Grep")   //jika tangan berbentuk "grep"
            {
                isMove = false;
                isChoose = false;
                isGrep = true;
            }
            else if (handState == "Choose") //jika tangan berbentuk "choose"
            {
                isMove = false;
                isChoose = true;
                isGrep = false;
            }
        }
        else if(other.tag == buttonTag)
        {
            buttonChoose = other.gameObject;
        }
        else
        {
            buttonChoose = null;
        }
    }

    /// <summary>
    /// mendapatkan data yang dikirim oleh hardware melalui bluetooth dengan bluetooth module HC-05
    /// </summary>
    void GetData()
    {
        data = BTController.data.Split(char.Parse(",")); //memisahkan data dari  (x,y,z,x,y,z,pos) ==> (x) (y) (z) (x) (y) (z) (pos)

        handState = data[6];

        for (int i = 0; i < 2; i++)
        {
            float.TryParse(data[i], out rawPos[i]);
        }

        for (int i = 3; i < 5; i++)
        {
            float.TryParse(data[i], out rawRot[i - 3]);
        }
    }

    /// <summary>
    /// mendapatkan skala antara objek di ruang virtual dan dunia nyata
    /// rumus: skala = posisi virtual / posisi dunia nyata
    /// </summary>
    void checkScale() 
    {
        posScale[0] = Point.transform.position.x / rawPos[0];
        posScale[1] = Point.transform.position.y / rawPos[1];
        posScale[2] = Point.transform.position.z / rawPos[2];

        rotScale[0] = Point.transform.rotation.x / rawRot[0];
        rotScale[1] = Point.transform.rotation.y / rawRot[1];
        rotScale[2] = Point.transform.rotation.z / rawRot[2];
    }

    /// <summary>
    /// mendapatkan posisi
    /// rumus: data * skala
    /// </summary>
    void getPos()
    {
        for (int i = 0; i < posScale.Length; i++)
        {
            pos[i] = rawPos[i] * posScale[i];
        }
    }

    /// <summary>
    /// mendapatkan rotasi
    /// rumus: data * skala
    /// </summary>
    void getRot()
    {
        for (int i = 0; i < rotScale.Length; i++)
        {
            rot[i] = rawRot[i] * rotScale[i];
        }
    }
}

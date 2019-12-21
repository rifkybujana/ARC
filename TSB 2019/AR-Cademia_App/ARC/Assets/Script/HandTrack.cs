using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using TechTweaking.Bluetooth;

public class HandTrack : MonoBehaviour
{
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
    public GameObject Object;

    public Text statText;

    string[] data;

    float[] rawPos, rawRot;
    float[] posScale, rotScale;
    float[] pos, rot;

    [HideInInspector] public bool check = false;                                     //memastikan bahwa hanya mengecek skala sekali saja
    [HideInInspector] public bool started = false;                                   //memulai hanya sekali
    bool isMove, isChoose, isGrep;                          //tangan sedang apa?
    bool isPlaced = false;

    private Vector3 selisihJarak;

    [HideInInspector] public BluetoothDevice device;
    public Text statusText;

    private string dataRaw;
    public string DeviceName;
    private string debugText;


    private void Awake()                                    //jika script dimulai
    {
        device = new BluetoothDevice();

        if (BluetoothAdapter.isBluetoothEnabled()) connect();
        else
        {
            statusText.text = "Status: Please Enable Your Bluetooth";

            BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
            BluetoothAdapter.listenToBluetoothState();

            BluetoothAdapter.askEnableBluetooth();
        }
    }


    // Start is called before the first frame update
    void Start()                                            //jika aplikasi dimulai
    {
        check = false;
        started = false;

        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;

        statText.text = "Not started yet";
    }

    // Update is called once per frame
    void Update()
    {
        byte[] msg = device.read();                                     //data berupa byte

        dataRaw = System.Text.ASCIIEncoding.ASCII.GetString(msg);       //convert data menjadi string

        statusText.text = "MSG: " + dataRaw;                            //print data yang diterima ke screen
        data = dataRaw.Split(',');                                      //memisahkan data

        /*  convert data rotasi dan posisi menjadi float
         *  data[data ke] >> rawPos
         */
        float.TryParse(data[0], out rawPos[0]);
        float.TryParse(data[1], out rawPos[1]);
        float.TryParse(data[2], out rawPos[2]);

        float.TryParse(data[3], out rawPos[3]);
        float.TryParse(data[4], out rawPos[4]);
        float.TryParse(data[5], out rawPos[5]);

        // convert data handStat ke interger
        int convrt;
        int.TryParse(data[6], out convrt);

        // mengconvert agar tau jika tangan sedang apa
        switch (convrt)
        {
            case 1:
                handState = "Start";
                break;
            case 2:
                handState = "Move";
                break;
            case 3:
                handState = "Grep";
                break;
            case 4:
                handState = "Choose";
                break;
            default:
                handState = " ";
                break;
        }

        // jika handState = start atau sudah dimulai
        if (handState == "Start" || started == true)
        {
            started = true;
            statText.text = "Started";

            if (check == false) // jika belum mengecek
            {
                checkScale();                       //mendapatkan skala
                check = true;                       //objek telah di cek
            }
            else                                    //jika sudah mengecek skala
            {
                getPos();                           //mengkalkulasikan untuk mendapat posisi di ruang virtual
                getRot();                           //mengkalkulasikan untuk mendapat rotasi di ruang virtual

                hand.transform.position = new Vector3(pos[0], pos[1], pos[2]);              //mengganti posisi objek
                hand.transform.rotation = new Quaternion(rot[0], rot[1], rot[2], 0);        //mengganti rotasi objek

                if (isMove)         //jika tangan menyentuh objek dan berposisi "move"
                {
                    target.gameObject.transform.position = hand.transform.position + selisihJarak;      //mengupdate posisi objek berdasarkan tangan
                }
                else if (isGrep)    //jika tangan menyentuh objek dan berposisi "grep"
                {
                    float averageScale = (selisihJarak.x + selisihJarak.y + selisihJarak.z) / 3f;                               //menghitung rata rata selisih
                    target.gameObject.transform.localScale = new Vector3(averageScale * 2, averageScale * 2, averageScale * 2); //mengupdate ukuran objek berdasarkan posisi tangan
                }
            }

            if (data[6] != "Move" && data[6] != "Grep" && data[6] != "Choose")
            {
                isMove = false;
                isChoose = false;
                isGrep = false;
            }
        }
        else
        {
            statText.text = handState;
        }

        if (check)
        {
            Object.gameObject.SetActive(true);
            Point.gameObject.SetActive(false);
        }
        else
        {
            Object.gameObject.SetActive(true);
            Point.gameObject.SetActive(false);
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

    private void connect()
    {
        statusText.text = "Status : Trying To Connect";
        device.Name = DeviceName;
        device.setEndByte(10);
        statusText.text = "Status : Connecting";

        device.connect();
    }

    void HandleOnBluetoothStateChanged(bool isBtEnabled)
    {
        if (isBtEnabled)
        {
            connect();
            BluetoothAdapter.OnBluetoothStateChanged -= HandleOnBluetoothStateChanged;
            BluetoothAdapter.stopListenToBluetoothState();
        }
    }

    void HandleOnDeviceOff(BluetoothDevice dev)
    {
        if (!string.IsNullOrEmpty(dev.Name)) statusText.text = "Status : can't connect to '" + dev.Name + "', device is OFF";
        else if (!string.IsNullOrEmpty(dev.MacAddress)) statusText.text = "Status : can't connect to '" + dev.MacAddress + "', device is OFF";
    }

    void HandleOnDeviceNotFound(BluetoothDevice dev)
    {
        if (!string.IsNullOrEmpty(dev.Name)) statusText.text = "Status : Can't find a device with the name '" + dev.Name + "', device might be OFF or not paird yet ";
    }

    public void disconnect()
    {
        if (device != null)
            device.close();
    }

    void OnDestroy()
    {
        BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;
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
        pos[0] = rawPos[0] * posScale[0];
        pos[1] = rawPos[1] * posScale[1];
        pos[2] = rawPos[2] * posScale[2];
    }

    /// <summary>
    /// mendapatkan rotasi
    /// rumus: data * skala
    /// </summary>
    void getRot()
    {
        rot[0] = rawRot[0] * rotScale[0];
        rot[1] = rawRot[1] * rotScale[1];
        rot[2] = rawRot[2] * rotScale[2];
    }
}

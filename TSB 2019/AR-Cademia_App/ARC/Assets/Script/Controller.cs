using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTweaking.Bluetooth;

public class Controller : MonoBehaviour
{
    private GameObject target;

    private BluetoothDevice device;

    private string[] data;

    private string content;

    private int handState;

    private float[] rawPos = { 0, 0, 0 };
    private float[] rawRot = { 0, 0, 0 };
    private float[] posScale;
    private float[] rotScale;
    private float[] pos;
    private float[] rot;

    private Vector3 selisihJarak;

    private bool check;
    private bool isMove, isChoose, isGrep;


    public GameObject hand;

    [Space(5)]
    [Header("UI")]
    public Text statusText;
    public Text debugText;
    public Text infoText;
    public Text handPoseText;

    [Space(5)]
    public string[] info;

    [Space(5)]
    public GameObject[] buttonList;

    [Space(5)]
    [Header("Object and Device")]
    public string DeviceName = "HC-05";
    public string objectTag = "Selectable";
    public string buttonTag = "Butt";

    [Space(5)]
    public GameObject Point;
    public GameObject Atom;

    [HideInInspector] public bool isStarted;
    [HideInInspector] public bool isRotate;

    private void Awake()
    {
        device = new BluetoothDevice();

        if (BluetoothAdapter.isBluetoothEnabled())
        {
            connect();
        }
        else
        {
            statusText.text = "Status: Please Enable Your Bluetooth";

            BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
            BluetoothAdapter.listenToBluetoothState();

            BluetoothAdapter.askEnableBluetooth();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isRotate = true;
        check = false;

        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;

        Atom.gameObject.SetActive(false);
        Point.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        byte[] msg = device.read(); //membaca data dari device

        if (msg != null && msg.Length > 0)   //jika msg tidak kosong
        {
            content = System.Text.ASCIIEncoding.ASCII.GetString(msg);    //convert msg dari byte menjadi string
            statusText.text = "Data: " + content;

            data = content.Split(',');                                          //memisahkan data yang digunakan
        }

        if (data.Length > 0)
        {
            //convert data posisi dari string menjadi float
            rawPos[0] = float.Parse(data[0]);
            rawPos[1] = float.Parse(data[1]);
            rawPos[2] = float.Parse(data[2]);

            //convert data rotasi dari string menjadi float
            rawRot[0] = float.Parse(data[3]);
            rawRot[1] = float.Parse(data[4]);
            rawRot[2] = float.Parse(data[5]);

            handState = int.Parse(data[6]);                               //convert data dari string menjadi int

            debugText.text = "Readed";
        }
        else
        {
            debugText.text = "data is null";
        }

        if (handState == 1 || isStarted == true)                            //jika tangan berbentuk "Start" atau isStarted = true
        {

            if (isStarted == false)
            {
                isStarted = true;                                           //jika started false, maka isStarted menjadi true
            }

            //memperlihatkan atom dan menghilangkan point
            Atom.gameObject.SetActive(true);
            Point.gameObject.SetActive(false);

            if (check == false)
            {
                debugText.text = "Checking...";

                //mendapatkan skala posisi (posisi virtual / posisi nyata)
                posScale[0] = Point.transform.position.x / rawPos[0];
                posScale[1] = Point.transform.position.y / rawPos[1];
                posScale[2] = Point.transform.position.z / rawPos[2];

                //mendapatkan skala rotasi (rotasi virtual / rotasi nyata)
                rotScale[0] = Point.transform.rotation.x / rawRot[0];
                rotScale[1] = Point.transform.rotation.y / rawRot[1];
                rotScale[2] = Point.transform.rotation.z / rawRot[2];

                check = true;
            }
            else
            {
                debugText.text = "Checked";

                //mendapatkan posisi tangan (posisi nyata * skala)
                pos[0] = rawPos[0] * posScale[0];
                pos[1] = rawPos[1] * posScale[1];
                pos[2] = rawPos[2] * posScale[2];

                //mendapatkan rotasi tangan (rotasi nyata * skala)
                rot[0] = rawRot[0] * rotScale[0];
                rot[1] = rawRot[1] * rotScale[1];
                rot[2] = rawRot[2] * rotScale[2];
                

                //mengubah posisi dan rotasi tangan berdasarkan pos dan rot
                hand.transform.position = new Vector3(pos[0], pos[1], pos[2]);
                hand.transform.rotation = new Quaternion(rot[0], rot[1], rot[2], 0);

                handPoseText.text = "Pos: " + pos[0] + ", " + pos[1] + ", " + pos[2];
            }
        }
        

        //jika posisi tangan = "Start" atau yang lainnya
        if (handState < 2)
        {
            isMove = false; isChoose = false; isGrep = false;
        }

        //jika tangan menyentuh objek dan berbentuk "Move"
        if (isMove)
        {
            target.gameObject.transform.position = hand.transform.position + selisihJarak;  //posisi objek = posisi tangan + selisih jarak
            debugText.text = "Moving...";
        }
        else if (isGrep)   //jika tangan menyentuh objek dan berbentuk "Grep"
        {
            float averageScale = (selisihJarak.x + selisihJarak.y + selisihJarak.z) / 3;                                    //rata rata selsisih jarak
            target.gameObject.transform.localScale = new Vector3(averageScale * 2, averageScale * 2, averageScale * 2);     //ukuran objek = 2 * rata rata selsiih jarak
            debugText.text = "Scaling";
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        //jika tag objek yang disentuh = objectTag
        if(other.tag == objectTag)
        {
            target = other.gameObject;                                          //target = objek yang dipilih
            selisihJarak = target.transform.position = hand.transform.position; //selisih jarak = posisi target - posisi tangan

            switch (handState)                                                  //bentuk tangan
            {
                case 2:                                                         //jika tangan = "Move"
                    isMove = true;
                    isChoose = false;
                    isGrep = false;
                    break;
                case 3:                                                         //jika tangan = "Grep"
                    isMove = false;
                    isChoose = false;
                    isGrep = true;
                    break;
            }
        }else if(other.tag == buttonTag && handState == 4)  //jika tag object yang disentuh = buttonTag
        {
            isMove = false;
            isChoose = true;
            isGrep = false;

            target = other.gameObject;                                          //target = objek yang dipiliih

            for (int i = 0; i < buttonList.Length; i++)
            {
                if(target == buttonList[i])                                     //jika salah satu objek = button
                {
                    if(i == 0)                                                  //jika i = 0
                    {
                        if (isRotate)                                           //stop rotasi jika sedang berotasi
                        {
                            isRotate = false;
                        }
                        else                                                    //rotasi jika sedang tidak berotasi
                        {
                            isRotate = true;
                        }
                    }
                    else                                                        //jika i bukan 0
                    {
                        infoText.text = info[i - 1];                            //memunculkan info
                    }
                }
            }
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
}

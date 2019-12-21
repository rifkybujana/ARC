using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTweaking.Bluetooth;

public class BluetoothController : MonoBehaviour
{
    [HideInInspector] public BluetoothDevice device; //bluetooth device
    public Text statusText;         //status

    [HideInInspector]
    public string data;             //data dari HC-05

    [SerializeField]
    private string DeviceName;      //Nama device HC-05

    public Text status;

    // Start is called before the first frame update
    void Awake()
    {
        device = new BluetoothDevice();

        if (BluetoothAdapter.isBluetoothEnabled()) connect();
        else {
            statusText.text = "Status: Please Enable Your Bluetooth";

            BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
            BluetoothAdapter.listenToBluetoothState();

            BluetoothAdapter.askEnableBluetooth(); //meminta pengguna menyalakan bluetooth
        }
    }


    [HideInInspector] public bool started;

    private void Start()
    {
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;
    }
    

    private void Update()
    {
        byte[] msg = device.read();

        if (msg != null && msg.Length > 0)
        {
            data = System.Text.ASCIIEncoding.ASCII.GetString(msg);

            statusText.text = "MSG : " + data;
        }

        string[] finalData = data.Split(',');
        status.text = finalData[6];

        if (finalData[6] == "Start") started = true;
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

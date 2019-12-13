using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTweaking.Bluetooth;

public class BluetoothController : MonoBehaviour
{
    private BluetoothDevice device; //bluetooth device
    public Text statusText;         //status

    [HideInInspector]
    public string data;             //data dari HC-05

    [SerializeField]
    private string DeviceName;      //Nama device HC-05

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

    private void Start()
    {
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;
    }

    private void connect()
    {
        statusText.text = "Status : Trying To Connect";
        device.Name = DeviceName;
        device.setEndByte(10);
        device.ReadingCoroutine = ManageConnection;
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

    IEnumerator ManageConnection(BluetoothDevice device)
    {
        statusText.text = "Status : Connected & Can read";

        while (device.IsReading)
        {
            if (device.IsDataAvailable)
            {
                byte[] msg = device.read();

                if (msg != null && msg.Length > 0)
                {
                    string content = System.Text.ASCIIEncoding.ASCII.GetString(msg);
                    statusText.text = "MSG : " + content;

                    data = content;
                }
            }

            yield return null;
        }

        statusText.text = "Status : Done Reading";
    }

    void OnDestroy()
    {
        BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;
    }
}

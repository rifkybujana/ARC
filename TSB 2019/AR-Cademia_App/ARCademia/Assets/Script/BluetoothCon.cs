using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTweaking.Bluetooth;

public class BluetoothCon : MonoBehaviour
{

    private BluetoothDevice device;

    [SerializeField] private Text statusText;

    [HideInInspector] public string data;

    void Awake()
    {
        device = new BluetoothDevice();

        if (BluetoothAdapter.isBluetoothEnabled())
        {
            connect();
        }
        else
        {

            //BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user
            statusText.text = "Status : Please enable your Bluetooth";

            BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
            BluetoothAdapter.listenToBluetoothState(); // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON

            BluetoothAdapter.askEnableBluetooth();//Ask user to enable Bluetooth

        }
    }

    void Start()
    {
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;//This would mean a failure in connection! the reason might be that your remote device is OFF

        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound; //Because connecting using the 'Name' property is just searching, the Plugin might not find it!.
    }

    private void connect()
    {
        statusText.text = "Status : Trying To Connect";
        device.Name = "HC-05";
        device.setEndByte(10);
        device.ReadingCoroutine = ManageConnection;
        statusText.text = "Status : trying to connect";

        device.connect();
    }


    //############### Handlers/Recievers #####################
    void HandleOnBluetoothStateChanged(bool isBtEnabled)
    {
        if (isBtEnabled)
        {
            connect();
            //We now don't need our recievers
            BluetoothAdapter.OnBluetoothStateChanged -= HandleOnBluetoothStateChanged;
            BluetoothAdapter.stopListenToBluetoothState();
        }
    }

    //This would mean a failure in connection! the reason might be that your remote device is OFF
    void HandleOnDeviceOff(BluetoothDevice dev)
    {
        if (!string.IsNullOrEmpty(dev.Name))
        {
            statusText.text = "Status : can't connect to '" + dev.Name + "', device is OFF ";
        }
        else if (!string.IsNullOrEmpty(dev.MacAddress))
        {
            statusText.text = "Status : can't connect to '" + dev.MacAddress + "', device is OFF ";
        }
    }

    //Because connecting using the 'Name' property is just searching, the Plugin might not find it!.
    void HandleOnDeviceNotFound(BluetoothDevice dev)
    {
        if (!string.IsNullOrEmpty(dev.Name))
        {
            statusText.text = "Status : Can't find a device with the name '" + dev.Name + "', device might be OFF or not paird yet ";

        }
    }

    public void disconnect()
    {
        if (device != null)
            device.close();
    }

    //############### Reading Data  #####################
    //Please note that you don't have to use this Couroutienes/IEnumerator, you can just put your code in the Update() method
    IEnumerator ManageConnection(BluetoothDevice device)
    {
        statusText.text = "Status : Connected & Can read";

        while (device.IsReading)
        {
            if (device.IsDataAvailable)
            {
                byte[] msg = device.read();//because we called setEndByte(10)..read will always return a packet excluding the last byte 10.

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


    //############### Deregister Events  #####################
    void OnDestroy()
    {
        BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;

    }

}

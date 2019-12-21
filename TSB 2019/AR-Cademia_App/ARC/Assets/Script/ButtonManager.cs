using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private HandTrack hand;                        //tangan

    public GameObject[] button;                                     //list tombol
    
    [SerializeField] private string[] info;                         //info yang ingin ditampilkan

    public Text infoText;                                                  //Text tempat info
    public Canvas canvas;                                                  //canvas
    
    [HideInInspector] public bool isRotate;                         //sedang rotasi?

    // Start is called before the first frame update
    void Start()
    {
        isRotate = true;
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas == null) canvas = GameObject.FindGameObjectWithTag("The Canvas").GetComponent<Canvas>();
        else canvas.worldCamera = Camera.main;

        if (button == null) button = GameObject.FindGameObjectsWithTag("Butt");

        if(infoText == null) infoText = GameObject.FindGameObjectWithTag("Info Text").GetComponent<Text>();

        if(hand.buttonChoose == button[0])
        {
            if (isRotate)
            {
                isRotate = false;
            }
            else
            {
                isRotate = true;
            }
        }
        else
        {
            for (int i = 1; i < button.Length; i++)     //mencari tombol yang mana yang dipilih
            {
                if (hand.buttonChoose == button[i])      //jika tombol yang dipilih ketemu
                {
                    infoText.text = info[i];            //menampilkan info
                }
            }
        }
    }
}

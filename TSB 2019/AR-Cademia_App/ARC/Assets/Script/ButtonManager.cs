using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private HandTrack hand;                //tangan
    [SerializeField] private GameObject[] button;           //list tombol
    [SerializeField] private GameObject rotateButton;       //tombol rotasi
    [SerializeField] private string[] info;                 //info yang ingin ditampilkan

    [SerializeField] Text infoText;                         //Text tempat info
    [SerializeField] Canvas canvas;                         //canvas
    
    [HideInInspector] public bool isRotate;                 //sedang rotasi?

    // Start is called before the first frame update
    void Start()
    {
        isRotate = true;
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(hand.buttonChoose != null)       //jika tangan menyentuh objek tombol
        {
            canvas.gameObject.SetActive(true);      //memunculkan canvas

            if(hand.buttonChoose == rotateButton)   //jika tombol yang disentuh adalah tombol rotasi
            {
                if (isRotate)                   //jika sedang rotasi
                {
                    isRotate = false;           //menghentikan rotasi
                }
                else                            //jika tidak rotasi
                {
                    isRotate = true;            //memulai rotasi
                }
            }
            else                                //jika tombol yang disentuh adalah yang lainnya
            {
                for (int i = 0; i < button.Length; i++)     //mencari tombol yang mana yang dipilih
                {
                    if(hand.buttonChoose == button[i])      //jika tombol yang dipilih ketemu
                    {
                        infoText.text = info[i];            //menampilkan info
                    }
                }
            }
        }
        else        //jika tidak ada tombol yang disentuh
        {
            canvas.gameObject.SetActive(false);     //menonaktifkan canvas
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private HandTrack hand;
    [SerializeField] private Button[] button;
    [SerializeField] private Button rotateButton;
    [SerializeField] private string[] info;

    [SerializeField] Text infoText;
    [SerializeField] Canvas canvas;

    int buttonChoose;
    private bool isRotate;

    // Start is called before the first frame update
    void Start()
    {
        isRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hand.buttonChoose != null)
        {
            canvas.gameObject.SetActive(true);

            if(hand.buttonChoose == rotateButton)
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
                for (int i = 0; i < button.Length; i++)
                {
                    if(hand.buttonChoose == button[i])
                    {
                        infoText.text = info[i];
                    }
                }
            }
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }
}

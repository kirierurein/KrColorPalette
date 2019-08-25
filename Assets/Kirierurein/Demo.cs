using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public Image image;
    public Button red;
    public Button blue;
    public Button green;

    void Start()
    {
        red.onClick.AddListener(()=>
        {
            image.color = KrColorPalette.Palette.Red;
        });

        blue.onClick.AddListener(()=>
        {
            image.color = KrColorPalette.Palette.Blue;
        });

        green.onClick.AddListener(()=>
        {
            image.color = KrColorPalette.Palette.Green;
        });
    }
}

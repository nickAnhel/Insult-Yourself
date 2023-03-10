using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image oldImage;
    public Sprite[] images;

    public void ChangeSprite(int index)
    {
        oldImage.sprite = images[index];
    }
}

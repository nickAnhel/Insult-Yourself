using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownController : MonoBehaviour
{
    public Slider slider;

    public void SetCDTime(float maxTime)
    {
        slider.maxValue = maxTime;
        slider.value = maxTime;
    }

    public void Cooldown(float remTime)
    {
        slider.value = remTime;
    }
}

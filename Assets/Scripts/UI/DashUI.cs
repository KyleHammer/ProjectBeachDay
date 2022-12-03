using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxDashValue(float value)
    {
        slider.maxValue = value;
    }
    
    public void SetDashValue(float value)
    {
        slider.value = slider.maxValue - value;
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText;

    private void Start()
    {
        GameManager.Instance.SetGameUI(transform.parent.gameObject);
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
        hpText.text = health + " / " + slider.maxValue;
    }
}

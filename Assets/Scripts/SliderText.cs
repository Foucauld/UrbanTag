using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderText : MonoBehaviour {

    public Slider slider;
    public Text text;
    public string unit;
    public byte decimals = 2;

    public GameObject[] inputGroups;

    void OnEnable()
    {
        slider.onValueChanged.AddListener(ChangeValue);
        ChangeValue(slider.value);
    }
    void OnDisable()
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    void ChangeValue(float value)
    {
        text.text = value.ToString("n" + decimals) + " " + unit;

        for (int i = 0; i < inputGroups.Length; i++)
        {
                inputGroups[i].SetActive(i < value);
        }
    }
}

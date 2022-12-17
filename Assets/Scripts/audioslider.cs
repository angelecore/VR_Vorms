using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioslider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    void Start()
    {
        SoundManager.Instance.ChangeMasterVolume(_slider.value);
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
    }

}

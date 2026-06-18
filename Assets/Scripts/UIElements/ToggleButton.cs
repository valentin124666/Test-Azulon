using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Sprite OnIcon;
    public Sprite OffIcon;
    public Image buttonImage;

    private bool isMuted = false;

    void Start()
    {
        UpdateIcon();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;

        // Вимикаємо/вмикаємо звук
        AudioListener.volume = isMuted ? 0 : 1;

        // Оновлюємо іконку
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        buttonImage.sprite = isMuted ? OffIcon : OnIcon;
    }
}

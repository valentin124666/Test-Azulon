using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedButtonPanel : MonoBehaviour
{
    [SerializeField] private Transform[] popupButtons;
    private Vector3[] originalPositions;
    [SerializeField] private Button settingsOpen;
    
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private float staggerDelay = 0.05f;
    [SerializeField] private float moveOffset = 100f;

    private bool isShown = false;

    private void Awake()
    {
        originalPositions = new Vector3[popupButtons.Length];

        for (int i = 0; i < popupButtons.Length; i++)
        {
            originalPositions[i] = popupButtons[i].localPosition;
            // Ховаємо кнопки за межі (вгору)
            popupButtons[i].localPosition += Vector3.up * moveOffset;
            popupButtons[i].gameObject.SetActive(false);
        }
        settingsOpen.onClick.AddListener(TogglePopup);

    }

    private void TogglePopup()
    {
        if (isShown)
        {
            HidePopup();
        }
        else
        {
            ShowPopup();
        }

        isShown = !isShown;
    }
    private void ShowPopup()
    {
        for (int i = 0; i < popupButtons.Length; i++)
        {
            popupButtons[i].gameObject.SetActive(true);
            popupButtons[i].DOLocalMove(originalPositions[i], showDuration)
                .SetDelay(i * staggerDelay)
                .SetEase(Ease.OutBack); // Пружинна анімація
        }
    }

    private void HidePopup()
    {
        for (int i = 0; i < popupButtons.Length; i++)
        {
            var button = popupButtons[i];
                
            button.DOLocalMove(settingsOpen.transform.localPosition, showDuration)
                .SetDelay(i * staggerDelay)
                .SetEase(Ease.InBack)
                .OnComplete(() => button.gameObject.SetActive(false));
        }
    }
}

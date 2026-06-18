using DG.Tweening;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;
        [SerializeField] private CanvasGroup canvasGroup;

        public void Show(float? customDuration = null)
        {
            gameObject.SetActive(true);
            canvasGroup.DOFade(1f, customDuration ?? duration)
                .SetEase(Ease.InOutQuad)
                .OnStart(() =>
                {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                });
        }

        public void Hide(float? customDuration = null)
        {
            canvasGroup.DOFade(0f, customDuration ?? duration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.interactable = false;
                    gameObject.SetActive(false);
                });
        }    
        
        public void ShowQuick()
        {
            
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            gameObject.SetActive(true);
        }

        public void HideQuick()
        {
            
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
    }
}
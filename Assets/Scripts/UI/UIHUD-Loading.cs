using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIHUD
{
    [Header("Loading Screen")]
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Slider loadingSlider;

    public void ShowLoading(bool show) {

        loadingScreenCanvasGroup.alpha = show ? 1f : 0f;
        loadingScreenCanvasGroup.blocksRaycasts = show;

        //Reset the value
        if (show) {
            loadingSlider.value = 0f;
        }

    }

    public void SetLoadingProgress(float progress) {

        loadingSlider.value = progress;

    }

}

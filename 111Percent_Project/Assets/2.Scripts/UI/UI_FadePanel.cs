using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

//Todo.... DOTWEEN 플러그인 추가
#if DoTween_Plugin_Included
using DG.Tweening;
#endif

public class UI_FadePanel : UIBase
{
    public enum FadeType { None, FadeIn, FadeOut } //Fade In-> 밝아지는 Fade Out -> 어두워지는

    [SerializeField] Image blackImg = null;

    private bool isFadedOut = false; //FadeOut된 상태인지...
    private FadeType fadeType = FadeType.None;
    private float fadeTime = 2f;

    private bool isFadePlaying = false;

    private Action fadeCallback = null;

    internal override void OnEnable()
    {
        transform.SetAsLastSibling(); //Fade는 항상 위쪽에 위치시키자
    }

    internal override void OnDisable()
    {
        transform.SetAsLastSibling(); //Fade는 항상 위쪽에 위치시키자

        Clear();
    }

    public void Setup(FadeType type, float time = 1.5f, Action callback = null)
    {
        if (isFadePlaying == true)
            return;

        isFadePlaying = true;

        fadeType = type;
        fadeTime = time;

        fadeCallback = callback;

        switch (type)
        {
            case FadeType.FadeIn: //밝아지는
                {
                    if (isFadedOut == false) //어두워지지 않은 상태에서 밝이질 경우... 이러면 안되는데?
                    {
                        Debug.Log("<color=red>Error...! Currently Not Faded Out... </color>");
                        Clear();
                        gameObject.SafeSetActive(false);
                        return;
                    }
                    else
                    {
                        #if DoTween_Plugin_Included
                        blackImg.DOFade(1f, 0f); //어두운 상태에서 시작
                        #endif
                    }
                }
                break;

            case FadeType.FadeOut: //어두워지는
                {
                    #if DoTween_Plugin_Included
                    blackImg.DOFade(0f, 0f); //밝은 상태에서 시작
                    #endif
                    isFadedOut = true;
                }
                break;
        }
    }

    public override void Show()
    {
        base.Show();
        Activate();
    }

    private void Activate()
    {
        gameObject.SafeSetActive(true);

        switch (fadeType)
        {
            case FadeType.None:
                {
                    Clear();
                    gameObject.SafeSetActive(false);
                }
                break;

            case FadeType.FadeIn: //밝아지는
                {
                    #if DoTween_Plugin_Included
                    blackImg.DOFade(0f, fadeTime).OnComplete(OnCompleteFadeIn);
                    #endif
                }
                break;

            case FadeType.FadeOut: //어두워지는
                {
                    #if DoTween_Plugin_Included
                    blackImg.DOFade(1f, fadeTime).OnComplete(OnCompleteFadeOut);
                    #endif
                }
                break;
        }

    }

    private void OnCompleteFadeOut()
    {
        fadeCallback?.Invoke();
        isFadePlaying = false;
    }

    private void OnCompleteFadeIn()
    {
        fadeCallback?.Invoke();
        isFadePlaying = false;
        Clear();
        gameObject.SafeSetActive(false);
    }

    private void Clear()
    {
        fadeCallback = null;
        isFadedOut = false;
        fadeType = FadeType.None;
    }
}

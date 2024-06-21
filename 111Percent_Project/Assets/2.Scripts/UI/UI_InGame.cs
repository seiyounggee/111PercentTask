using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class UI_InGame : UIBase
{
    [SerializeField] UIComponent_InGameInput input;

    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI comboTxt;

    [SerializeField] List<GameObject> healthObj = new List<GameObject>();
    
    [SerializeField] GameObject hitUI = null;

    [SerializeField] GameObject gameOverPopup = null;
    [SerializeField] TextMeshProUGUI gameOverScoreTxt = null;
    [SerializeField] Button gameOverBtn = null;

    [SerializeField] GameObject gameClearPopup = null;
    [SerializeField] TextMeshProUGUI gameClearScoreTxt = null;
    [SerializeField] Button gameClearBtn = null;

    [SerializeField] GameObject tutorialPopup = null;
    [SerializeField] Button tutorialOkBtn = null;

    [SerializeField] Slider progressSlider = null;
    [SerializeField] GameObject enemyIconIdicator = null;
    private List<GameObject> enemyIconList = new List<GameObject>();

    [SerializeField] TextMeshProUGUI msgUI = null;

    private void Awake()
    {
        gameOverBtn.SafeSetButton(OnClickBtn);
        gameClearBtn.SafeSetButton(OnClickBtn);
        tutorialOkBtn.SafeSetButton(OnClickBtn);

        enemyIconIdicator.SafeSetActive(false);
    }

    internal override void OnEnable()
    {
        base.OnEnable();

        input.pointerDownCallback += Input_OnPointerDown;
        input.dragCallback += Input_Drag;
        input.pointerUpCallback += Input_OnPointerUp;
    }

    internal override void OnDisable()
    {
        base.OnDisable();

        input.pointerDownCallback -= Input_OnPointerDown;
        input.dragCallback -= Input_Drag;
        input.pointerUpCallback -= Input_OnPointerUp;
    }

    public override void Show()
    {
        base.Show();

        Clear();
    }

    public void Clear()
    {
        scoreTxt.SafeSetText("0");
        comboTxt.SafeSetText(string.Empty);

        foreach (var i in healthObj) 
        {
            i.SafeSetActive(true);
        }

        hitUI.SafeSetActive(false);
        msgUI.SafeSetActive(false);

        gameOverPopup.SafeSetActive(false);
        gameClearPopup.SafeSetActive(false);

        tutorialPopup.SafeSetActive(false);
    }

    public void UpdateHealthObj()
    {
        var player = InGameManager.Instance.player;
        if (player != null)
        {
            for (int i = 0; i < healthObj.Count; i++)
            {
                healthObj[i].SafeSetActive(false);

                if (i <= player.currHealth - 1)
                {
                    healthObj[i].SafeSetActive(true);
                }
            }
        }
    }

    public void SetupProgressSlider()
    {
        if (enemyIconList != null && enemyIconList.Count > 0)
        {
            foreach (var i in enemyIconList)
            {
                Destroy(i.gameObject);
            }
            enemyIconList.Clear();
        }

        progressSlider.maxValue = InGameManager.Instance.LastRoundIndex;
        progressSlider.minValue = 0;

        var start = progressSlider.value;
        var end = InGameManager.Instance.CurrentRoundIndex;
        UtilityCoroutine.StartCoroutine(ref updateProgressSlider, UpdateProgressSlider(start, end), this);

        var sliderWidth = progressSlider.GetComponent<RectTransform>().rect.width;
        var eachRoundDistX = sliderWidth / InGameManager.Instance.LastRoundIndex;
        var startLocalPosX = -sliderWidth / 2f;
        var count = InGameManager.Instance.LastRoundIndex;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                var go = GameObject.Instantiate(enemyIconIdicator, progressSlider.transform);
                var newLocalPos = go.transform.localPosition;
                newLocalPos.x = startLocalPosX + i * eachRoundDistX;
                go.transform.localPosition = newLocalPos;
                go.SafeSetActive(i > 0); //Ã¹¹øÂ°²¨´Â ²ôÀÚ...
                enemyIconList.Add(go);
            }
        }
    }

    private IEnumerator updateProgressSlider;
    private IEnumerator UpdateProgressSlider(float start, float end)
    {
        while (true)
        {
            progressSlider.value += Time.deltaTime;

            if (progressSlider.value >= end)
            {
                progressSlider.value = end;
                break;
            }

            yield return null;
        }

        progressSlider.value = InGameManager.Instance.CurrentRoundIndex;
    }

    public void UpdateProgressSlider()
    {
        progressSlider.maxValue = InGameManager.Instance.LastRoundIndex;
        progressSlider.minValue = 0;
        progressSlider.value = InGameManager.Instance.CurrentRoundIndex;
    }

    public void ActivateGameOver()
    {
        UtilityInvoker.Invoke(this, () =>
        {
            gameOverPopup.SafeSetActive(true);
            UtilityCoroutine.StartCoroutine(ref countup, CountUp(gameOverScoreTxt), this);
        }, 2f, "gameoverPopup");

    }

    public void ActivateGameClear()
    {
        UtilityInvoker.Invoke(this, () =>
        {
            gameClearPopup.SafeSetActive(true);
            UtilityCoroutine.StartCoroutine(ref countup, CountUp(gameClearScoreTxt), this);
        }, 2f, "gameClearPopup");
    }

    private IEnumerator countup = null;
    private IEnumerator CountUp(TextMeshProUGUI textUI)
    {
        long startNumber = 0; 
        long endNumber = InGameManager.Instance.GameScore; 
        float duration = 2.0f;

    
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            long currentNumber = startNumber + (long)((endNumber - startNumber) * progress);
            textUI.SafeSetText(currentNumber.ToString());
            yield return null;
        }

        textUI.SafeSetText(endNumber.ToString()); // Ensure the final number is set correctly
    }

    public void ActivateHitUI()
    {
        hitUI.GetComponent<Image>().color = new Color32(255, 0, 0, 143);
        hitUI.SafeSetActive(false);
        hitUI.SafeSetActive(true);
        
        hitUI.transform.DORestart();

        UtilityInvoker.Invoke(this, () =>
        {
            hitUI.SafeSetActive(false);
        }, 0.3f, "hitUI");
    }

    public void ActivateMessageUI(string msg)
    {
        msgUI.SafeSetText(msg);
        msgUI.GetComponent<TextMeshProUGUI>().color = new Color32(173, 173, 173, 200);
        msgUI.SafeSetActive(false);
        msgUI.SafeSetActive(true);

        msgUI.transform.DORestart();

        UtilityInvoker.Invoke(this, () =>
        {
            msgUI.SafeSetActive(false);
        }, 1f, "msgUI");
    }

    public void ActivateTutorialUI()
    {
        tutorialPopup.SafeSetActive(true);
    }

    public void UpdateCombo(string msg)
    {
        if (string.IsNullOrEmpty(msg))
        {
            comboTxt.gameObject.SafeSetActive(false);
        }
        else
        {
            comboTxt.gameObject.SafeSetActive(true);
            comboTxt.SafeSetText(msg);

            comboTxt.transform.DOScale(Vector3.one, 0f);
            comboTxt.transform.DOPunchScale(Vector3.one * 1.05f, 0.2f);

            UtilityInvoker.Invoke(this, () =>
            {
                comboTxt.gameObject.SafeSetActive(false);
            }, 1f, "combo");
        }
    }

    public void UpdateScore(string msg)
    {
        scoreTxt.gameObject.SafeSetActive(true);
        scoreTxt.SafeSetText(msg);
        scoreTxt.transform.DOScale(Vector3.one, 0f);
        scoreTxt.transform.DOPunchScale(Vector3.one * 1.15f, 0.2f);
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == gameOverBtn)
        {
            PhaseManager.Instance.ChangePhase(CommonDefine.Phase.OutGame);
        }
        else if (btn == gameClearBtn)
        {
            PhaseManager.Instance.ChangePhase(CommonDefine.Phase.OutGame);
        }
        else if (btn == tutorialOkBtn)
        {
            tutorialPopup.SafeSetActive(false);
            DataManager.Instance.IsTutorialFinish = 1;
        }
    }

    private void Input_OnPointerDown(UIComponent_InGameInput.GestureType type, float strength)
    {
        var player = InGameManager.Instance.player;
        if (player != null)
        { 
        
        }
    }

    private void Input_Drag(UIComponent_InGameInput.GestureType type, float strength)
    {
        var player = InGameManager.Instance.player;
        if (player != null)
        {

        }
    }

    private void Input_OnPointerUp(UIComponent_InGameInput.GestureType type, float strength, float length)
    {
        var player = InGameManager.Instance.player;
        if (player != null)
        {
            //Debug.Log("GestureType >> " + type);
            switch (type)
            {
                case UIComponent_InGameInput.GestureType.PointTouch:
                case UIComponent_InGameInput.GestureType.SwipeLeft:
                case UIComponent_InGameInput.GestureType.SwipeRight:
                    player.Attack();
                    break;
                case UIComponent_InGameInput.GestureType.SwipeUp:
                    player.Jump();
                    break;
                case UIComponent_InGameInput.GestureType.SwipeDown:
                    player.Defense();
                    break;
            }
        }
    }
}

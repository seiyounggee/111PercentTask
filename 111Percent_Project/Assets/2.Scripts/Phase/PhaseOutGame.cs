using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseOutGame : PhaseBase
{
    IEnumerator phaseOutGameCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        SceneManager.sceneLoaded += OnSceneLoaded;

        Scene currScene = SceneManager.GetActiveScene();

        if (currScene.name.Equals(CommonDefine.InGameScene)) //인게임씬인 경우 아웃게임씬으로 전환
        {
            SceneManager.LoadScene(CommonDefine.OutGameScene);
        }
        else if (currScene.name.Equals(CommonDefine.OutGameScene)) //기존 씬을 유지한채 다시 로드했을 경우...
        {
            UtilityCoroutine.StartCoroutine(ref phaseOutGameCoroutine, PhaseOutGameCoroutine(), this);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator PhaseOutGameCoroutine()
    {
        var logoUI = PrefabManager.Instance.UI_Logo;
        logoUI.gameObject.SafeSetActive(false);
        var titleUI = PrefabManager.Instance.UI_Title;
        titleUI.gameObject.SafeSetActive(false);

        OutGameManager.Instance.StartOutGame();

        yield return null;
        

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(CommonDefine.OutGameScene))
        {
            UtilityCoroutine.StartCoroutine(ref phaseOutGameCoroutine, PhaseOutGameCoroutine(), this);
        }
    }

}

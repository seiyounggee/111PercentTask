using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseInGame : PhaseBase
{
    IEnumerator phaseInGameCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        UtilityCoroutine.StartCoroutine(ref phaseInGameCoroutine, PhaseInGameCoroutine(), this);
    }

    IEnumerator PhaseInGameCoroutine()
    {
        SceneManager.LoadScene(CommonDefine.InGameScene);

        yield return null;

        //인게임 씬이 로드 될때까지 기달려주자
        while (true)
        {
            Scene currScene = SceneManager.GetActiveScene();

            if (currScene.name.Equals(CommonDefine.InGameScene))
                break;

            yield return null;
        }

        //게임 시작
        InGameManager.Instance.StartInGame();
    }
}
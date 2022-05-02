using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuHandler : MonoBehaviour
{
    [FormerlySerializedAs("transitionAnimator")] public Animator menuGridWipeDownAnimator;
    public Animator chooseGridWipeInAnimator;
    public Animator exitButtonAnimator;
    private static readonly int OnClickStart = Animator.StringToHash("OnClickStart");
    private static readonly int OnChooseGridShow = Animator.StringToHash("OnChooseGridShow");
    private static readonly int OnMainMenuWipeBack = Animator.StringToHash("OnMainMenuWipeBack");
    private static readonly int OnChooseGridWipeBack = Animator.StringToHash("OnChooseGridWipeBack");
    private static readonly int OnExitButtonWipeUp = Animator.StringToHash("OnExitButtonWipeUp");
    private static readonly int OnExitButtonWipeDown = Animator.StringToHash("OnExitButtonWipeDown");

    public void GameStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitApplication()
    {
        Debug.Log("Application has quit.");
        Application.Quit();
    }

    public void ShowMenuGridWipeDownTransition()
    {
        StartCoroutine(OnClickStartTransition());
    }

    public void ExitButtonAnimation()
    {
        StartCoroutine(OnClickExitButtonAnimation());
    }

    IEnumerator OnClickStartTransition()
    {
        menuGridWipeDownAnimator.SetTrigger(OnClickStart);
        yield return new WaitForSeconds(0.4f);
        chooseGridWipeInAnimator.SetTrigger(OnChooseGridShow);
        exitButtonAnimator.SetTrigger(OnExitButtonWipeDown);
    }

    IEnumerator OnClickExitButtonAnimation()
    {
        chooseGridWipeInAnimator.SetTrigger(OnChooseGridWipeBack);
        exitButtonAnimator.SetTrigger(OnExitButtonWipeUp);
        yield return new WaitForSeconds(0.4f);
        menuGridWipeDownAnimator.SetTrigger(OnMainMenuWipeBack);
    }
}

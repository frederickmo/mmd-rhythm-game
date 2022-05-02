using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

internal struct MusicDetails
{
    private int _index;
    private string _name;

    public MusicDetails(int index, string name)
    {
        this._index = index;
        this._name = name;
    }
}

internal enum CurrentState
{
    MainGrid,
    SelectModelGrid,
    SelectMusicGrid
}

internal enum SelectedModel
{
    MikuGothic,
    Miku,
    Ren,
    Rin,
    Meiko,
    Kaito
}

public class StartSceneManager : MonoBehaviour
{
    public Animator mainMenuAnimator;

    public Animator exitButtonAnimator;

    public Animator selectMainBoxAnimator;

    public Animator selectModelGridAnimator;
    public Animator selectMainGridAnimator;
    public Animator selectMusicGridAnimator;
    public Animator fromSelectModelGridToSelectMainGridAnimator;
    public Animator fromSelectMusicGridToSelectMainGridAnimator;

    // public Animator startGameTransitionAnimator;
    public Animator loadGameSceneAnimator;

    public GameObject selectedModelPreview;
    public GameObject selectedMusicCover;
    public TextMeshProUGUI selectedMusicTitle;
    
    private static readonly int OnClickStart = Animator.StringToHash("OnClickStart");
    private static readonly int OnChooseGridShow = Animator.StringToHash("OnChooseGridShow");
    private static readonly int OnMainMenuWipeBack = Animator.StringToHash("OnMainMenuWipeBack");
    private static readonly int OnChooseGridWipeBack = Animator.StringToHash("OnChooseGridWipeBack");
    private static readonly int OnExitButtonWipeUp = Animator.StringToHash("OnExitButtonWipeUp");
    private static readonly int OnExitButtonWipeDown = Animator.StringToHash("OnExitButtonWipeDown");

    private CurrentState _currentState; // 记录当前在选择模型/音乐的网格里处于哪个层级
    private SelectedModel _selectedModel;
    private int _selectedMusicIndex;

    private List<string> _musicTitleList = new();
    public Sprite[] modelPreviewList;
    public Sprite[] musicAlbumCoverList;

    public AudioSource[] musicList;

    private bool _isAnyMusicPlaying;

    private int _currentPlayingIndex;
    // Start is called before the first frame update
    void Start()
    {
        
        _currentState = CurrentState.MainGrid;
        _isAnyMusicPlaying = false;
        _currentPlayingIndex = -1;

        _musicTitleList.Add("ねぇねぇねぇ。");
        _musicTitleList.Add("おじゃま虫");
        _musicTitleList.Add("妄想感傷代償連盟");
        _musicTitleList.Add("ヴァンパイア");
        _musicTitleList.Add("千本桜");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EnterGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitApplication()
    {
        Debug.Log("Application has quit.");
        Application.Quit();
    }
    /**
     * 点击"开始游戏"，出现选择模型/音乐版面。
     */
    public void StartGameAnimation()
    {
        StartCoroutine(OnClickStartTransition());
    }

    public void EnterGameSceneAnimation()
    {
        StartCoroutine(OnEnterGameSceneAnimation());
    }

    /**
     * 点击左上角的"←"按钮
     * 有三种返回模式：
     *  - 当前为MainGrid，从选择模型/音乐界面回到主界面。
     *  - 当前为SelectModelGrid，从选择模型界面回到选择模型/音乐界面。
     *  - 当前为SelectMusicGrid，从选择音乐界面回到选择模型/音乐界面。
     */
    public void ExitButtonAnimation()
    {
        if (_currentState == CurrentState.MainGrid)
            StartCoroutine(OnClickExitButtonAnimation());
        else if (_currentState == CurrentState.SelectModelGrid)
        {
            _currentState = CurrentState.MainGrid;
            StartCoroutine(OnClickBackToMainGridFromSelectModel());
        }
        else if (_currentState == CurrentState.SelectMusicGrid)
        {
            MuteAllAudio();
            _currentState = CurrentState.MainGrid;
            StartCoroutine(OnClickBackToMainGridFromSelectMusic());
        }
    }


    public void SelectModelAnimation()
    {
        _currentState = CurrentState.SelectModelGrid;
        StartCoroutine(OnClickSelectModelButton());
    }

    public void SelectMusicAnimation()
    {
        _currentState = CurrentState.SelectMusicGrid;
        StartCoroutine(OnClickSelectMusicButton());
    }

    IEnumerator OnEnterGameSceneAnimation()
    {
        loadGameSceneAnimator.SetTrigger("OnCircleWipeIn");
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // loadGameSceneAnimator.SetTrigger("OnCircleWipeOut");
    }

    IEnumerator OnClickStartTransition()
    {
        mainMenuAnimator.SetTrigger(OnClickStart);
        yield return new WaitForSeconds(0.4f);
        selectMainBoxAnimator.SetTrigger(OnChooseGridShow);
        exitButtonAnimator.SetTrigger(OnExitButtonWipeDown);
    }

    IEnumerator OnClickExitButtonAnimation()
    {
        selectMainBoxAnimator.SetTrigger(OnChooseGridWipeBack);
        exitButtonAnimator.SetTrigger(OnExitButtonWipeUp);
        yield return new WaitForSeconds(0.4f);
        mainMenuAnimator.SetTrigger(OnMainMenuWipeBack);
    }

    IEnumerator OnClickSelectModelButton()
    {
        selectMainGridAnimator.SetTrigger("OnSelectMainGridWipeUp");
        yield return new WaitForSeconds(0.2f);
        selectModelGridAnimator.SetTrigger("OnSelectModelGridWipeDown");
    }

    IEnumerator OnClickBackToMainGridFromSelectModel()
    {
        selectModelGridAnimator.SetTrigger("OnSelectModelGridWipeUp");
        yield return new WaitForSeconds(0.2f);
        selectMainGridAnimator.SetTrigger("OnSelectMainGridWipeDown");
    }

    IEnumerator OnClickSelectMusicButton()
    {
        selectMainGridAnimator.SetTrigger("OnSelectMainGridWipeUp");
        yield return new WaitForSeconds(0.2f);
        selectMusicGridAnimator.SetTrigger("OnSelectMusicGridWipeDown");
    }

    IEnumerator OnClickBackToMainGridFromSelectMusic()
    {
        selectMusicGridAnimator.SetTrigger("OnSelectMusicGridWipeUp");
        yield return new WaitForSeconds(0.2f);
        selectMainGridAnimator.SetTrigger("OnSelectMainGridWipeDown");
    }
    
    
    // 以下写一点选择模型的按钮绑定的事件

    public void SelectMikuGothic()
    {
        _selectedModel = SelectedModel.MikuGothic;
        GlobalController.instance.selectedModelIndex = (int)_selectedModel;
        selectedModelPreview.GetComponent<Image>().sprite = modelPreviewList[(int)_selectedModel];
        ExitButtonAnimation();
    }

    public void SelectMiku()
    {
        _selectedModel = SelectedModel.Miku;
        GlobalController.instance.selectedModelIndex = (int)_selectedModel;
        selectedModelPreview.GetComponent<Image>().sprite = modelPreviewList[(int)_selectedModel];
        ExitButtonAnimation();
    }

    public void SelectRen()
    {
        _selectedModel = SelectedModel.Ren;
        GlobalController.instance.selectedModelIndex = (int)_selectedModel;
        selectedModelPreview.GetComponent<Image>().sprite = modelPreviewList[(int)_selectedModel];
        ExitButtonAnimation();
    }

    public void SelectRin()
    {
        _selectedModel = SelectedModel.Rin;
        GlobalController.instance.selectedModelIndex = (int)_selectedModel;
        selectedModelPreview.GetComponent<Image>().sprite = modelPreviewList[(int)_selectedModel];
        ExitButtonAnimation();
    }

    public void SelectMeiko()
    {
        _selectedModel = SelectedModel.Meiko;
        GlobalController.instance.selectedModelIndex = (int)_selectedModel;
        selectedModelPreview.GetComponent<Image>().sprite = modelPreviewList[(int)_selectedModel];
        ExitButtonAnimation();
    }

    public void SelectKaito()
    {
        _selectedModel = SelectedModel.Kaito;
        GlobalController.instance.selectedModelIndex = (int)_selectedModel;
        selectedModelPreview.GetComponent<Image>().sprite = modelPreviewList[(int)_selectedModel];
        ExitButtonAnimation();
    }

    public void PlayMusicByIndex(int index)
    {
        MuteAllAudio();
        if (!_isAnyMusicPlaying)
        {
            musicList[index].Play();
            _currentPlayingIndex = index;
        }
        else if (!_currentPlayingIndex.Equals(index))
        {
            musicList[index].Play();
            _currentPlayingIndex = index;
            return;
        }
        _isAnyMusicPlaying = !_isAnyMusicPlaying;
    }

    public void SelectMusicByIndex(int index)
    {
        MuteAllAudio();
        _selectedMusicIndex = index;
        GlobalController.instance.selectedMusicIndex = index;
        selectedMusicCover.GetComponent<Image>().sprite = musicAlbumCoverList[index];
        selectedMusicTitle.text = _musicTitleList[index];
        ExitButtonAnimation();
    }


    private void MuteAllAudio()
    {
        foreach (var audioSource in musicList)
            audioSource.Stop();
    }
}

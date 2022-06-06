using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;



public class GameManager : MonoBehaviour
{
    
    private static bool _isGamePaused;

    public GameObject pauseMenuUI;

    public GameObject endGameMenu;

    // public AudioSource bgm;
    public AudioSource[] bgmList;
    public int[] bpmOfBgmList;
    private AudioSource _bgm;
    public int selectedBgmIndex;

    public bool startBgm;

    public BeatScroller beatScroller;

    public static GameManager gameManagerInstance;

    public int currentScore;
    
    public int scorePerNote = 10;
    public int scorePerGoodHit = 15;
    public int scorePerPerfectHit = 20;

    public GameObject scoreGrid;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;

    public TextMeshProUGUI finalScoreText;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public PlaceOnPlane placeOnPlane;
    [FormerlySerializedAs("placedPrefabObject")] public GameObject spawnedObject;
    public GameObject[] alternativeSpawnedObjects;

    private Animator _animator;
    [FormerlySerializedAs("_animationStarted")] public bool animationStarted;

    [FormerlySerializedAs("_consecutiveHitCount")] public int consecutiveHitCount; // 累计击打的音符数
    public int consecutive1000Points;

    [FormerlySerializedAs("_hasQuarticHitNotes")] public bool hasQuarticHitNotes; // 是否达到连续击打4个音符
    private System.Random _random = new();
    [FormerlySerializedAs("_curAnimationState")] public int curAnimationState; // 当前动画状态
    [FormerlySerializedAs("_has20ConsecutiveHits")] public bool has20ConsecutiveHits;
    [FormerlySerializedAs("_consecutiveHitCountsLevels")] public int consecutiveHitCountsLevels;
    private static readonly int State = Animator.StringToHash("animationState");

    private bool _fixSpawnedObject;
    private GameObject _alternativeSpawnedObject;
    private int _currentModelIndex;

    public TextMeshProUGUI trialText;

    public GameObject[] leftCharacters;
    public GameObject[] rightCharacters;

    public Slider backgroundMusicSlider;
    public Slider gameMusicSlider;
    public Toggle isBackgroundMusicActiveToggle;
    public AudioSource hallBackgroundMusic;

    public Animator niceHitMessageAnimator;

    public GameObject[] noteGrids;
    
    private int[] _animationBpm = { 96, 106, 114, 108, 109, 114, 116, 114, 102 };

    private float _howLongBgmHasPlayed;
    private bool _bgmHasEnded;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedBgmIndex = GlobalController.instance.selectedMusicIndex;
        _currentModelIndex = GlobalController.instance.selectedModelIndex;
        _bgm = bgmList[selectedBgmIndex];
        
        _isGamePaused = false;
        gameManagerInstance = this;
        scoreText.text = "0";
        multiText.text = "×1";
        currentMultiplier = 1;
        startBgm = false;
        _bgmHasEnded = false;
        beatScroller.hasStarted = false;
        // spawnedObject = placeOnPlane.SpawnedObject;
        // _animator = spawnedObject.GetComponent<Animator>();
        hasQuarticHitNotes = false;
        animationStarted = false;
        has20ConsecutiveHits = false;
        consecutiveHitCount = 0;
        consecutiveHitCountsLevels = 0;
        curAnimationState = 0;

        consecutive1000Points = 0;
        
        // 游戏开始前都播放大厅音乐
        hallBackgroundMusic.Play();

        //test
        // _animator.SetInteger(State, 3);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (startBgm) return;
    //     if (Input.touchCount <= 0) return;
    //     startBgm = true;
    //     beatScroller.hasStarted = true;
    //     bgm.Play();
    // }

    private void Update()
    {
        if (_bgmHasEnded) return;
        
        if (placeOnPlane.isPrefabAlreadyPlaced && !_fixSpawnedObject)
        {
            spawnedObject = placeOnPlane.SpawnedObject.transform.GetChild(1).gameObject;
            _animator = spawnedObject.GetComponent<Animator>();
            // if (_animator != null)
            // {
            //     trialText.text = "_animator successfully get the model.";
            // }

            // _animator.SetInteger(State, 4);
            _fixSpawnedObject = true;
        }

        if (startBgm && !_bgm.isPlaying && !_isGamePaused)
        {
            // if (_bgmHasEnded && !beatScroller.beatScroller.activeSelf)
            //     return;
            _bgmHasEnded = true;
        }

        if (_bgmHasEnded)
        {
            // _bgmHasEnded = false;
            Debug.Log("BGM has ended.");
            beatScroller.beatScroller.SetActive(false);
            StartCoroutine(MusicEndAction());
            
            // 游戏结束后继续播放大厅音乐
            hallBackgroundMusic.Play();
        }

    }

    IEnumerator MusicEndAction()
    {
        finalScoreText.text = scoreText.text;
        yield return new WaitForSeconds(1f);
        scoreGrid.SetActive(false);
        endGameMenu.SetActive(true);
    }


    public void StartGameButtonHandler()
    {
        // if (startBgm) return;
        // if (beatScroller.hasStarted) return;
        // if (Input.touchCount <= 0) return;
        startBgm = true;
        
        noteGrids[selectedBgmIndex].SetActive(true);
        
        beatScroller.hasStarted = true;
        // beatScroller.
        hallBackgroundMusic.Pause();
        _bgm.Play();
    }

    private void NoteHit()
    {
        // Debug.Log("Hit on time");
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
        // currentScore += scorePerNote * currentMultiplier;
        scoreText.text = currentScore.ToString();
        multiText.text = "×" + currentMultiplier;
        
        scoreText.gameObject.GetComponent<Animator>().SetTrigger("OnScoreTextChange");

        if (!animationStarted)
        {
            animationStarted = true;
            curAnimationState = _random.Next(1, 10);
            _animator.SetInteger(State, curAnimationState);
            trialText.text = "Miku is ready.";
        }
        
        if (consecutiveHitCount < 32)
            consecutiveHitCount++;
        else
        {
            // niceHitMessageAnimator.SetTrigger("OnNiceHitMessageShow");

            consecutiveHitCount = 0;
            var randState = _random.Next(1, 10);
            while (randState.Equals(curAnimationState))
                randState = _random.Next(1, 10);
            curAnimationState = randState;
            
            // TODO:测试已放置的AR模型能不能改
            var randModelIndex = _random.Next(0, 6);
            while (randModelIndex.Equals(_currentModelIndex))
                randModelIndex = _random.Next(0, 6);
            _currentModelIndex = randModelIndex;
            
            
            if (_fixSpawnedObject)
            {
                // var nextModelToPlace = alternativeSpawnedObjects[_currentModelIndex];
                // placeOnPlane.SpawnedObject = Instantiate(nextModelToPlace, spawnedObject.transform.position,
                //     spawnedObject.transform.rotation);
                
                // var hitPose = GlobalController.instance.hitPose;
                // Destroy(placeOnPlane.SpawnedObject);
                // placeOnPlane.SpawnedObject = Instantiate(nextModelToPlace, hitPose.position, hitPose.rotation);
                
                // placeOnPlane.SpawnedObject = nextModelToPlace;
                // _animator = placeOnPlane.SpawnedObject.transform.GetChild(1).GetComponent<Animator>();
                
                
                _animator.SetInteger(State, curAnimationState);
                _animator.speed = (bpmOfBgmList[selectedBgmIndex] * 1f) / (_animationBpm[selectedBgmIndex] * 1f);
                // trialText.text = "Animation changed. curState is " + curAnimationState + ".";
                trialText.text = "[CHANGE] model no." + _currentModelIndex + ", animation no." + curAnimationState;
            }

        }

        // if (consecutive1000Points > 1000)
        // {
        //     consecutive1000Points = 0;
        //
        //     var leftIndex = _random.Next(0, leftCharacters.Length);
        //     while (leftIndex.Equals(_currentModelIndex))
        //         leftIndex = _random.Next(0, leftCharacters.Length);
        //     var rightIndex = _random.Next(0, rightCharacters.Length);
        //     while (rightIndex.Equals(_currentModelIndex))
        //         rightIndex = _random.Next(0, rightCharacters.Length);
        //     
        //     var hitPose = GlobalController.instance.hitPose;
        //     Instantiate(leftCharacters[leftIndex], hitPose.position, hitPose.rotation,
        //         placeOnPlane.SpawnedObject.transform);
        //     Instantiate(rightCharacters[rightIndex], hitPose.position, hitPose.rotation,
        //         placeOnPlane.SpawnedObject.transform);
        //
        //     StartCoroutine(DelayedDestroyAllChildren(placeOnPlane.SpawnedObject, 5f));
        // }
        
    }

    private IEnumerator DelayedDestroyAllChildren(GameObject parent, float time)
    {
        yield return new WaitForSeconds(time);
        DestroyAllChildren(parent);
    }

    private static void DestroyAllChildren(GameObject parent)
    {
        var childCount = parent.transform.childCount;
        for (var i = 0; i < childCount; ++i)
            Destroy(parent.transform.GetChild(i));
    }

    public void NormalHit()
    {
        Debug.Log("Normal hit");
        consecutiveHitCount++;
        consecutiveHitCountsLevels++;
        currentScore += scorePerNote * currentMultiplier;
        consecutive1000Points += scorePerNote * currentMultiplier;
        NoteHit();
        // AnimationController();
    }

    public void GoodHit()
    {
        Debug.Log("Good hit");
        consecutiveHitCount++;
        consecutiveHitCountsLevels++;
        currentScore += scorePerGoodHit * currentMultiplier;
        consecutive1000Points += scorePerGoodHit * currentMultiplier;
        NoteHit();
        // AnimationController();
    }

    public void PerfectHit()
    {
        Debug.Log("Perfect hit");
        consecutiveHitCount++;
        consecutiveHitCountsLevels++;
        currentScore += scorePerPerfectHit * currentMultiplier;
        consecutive1000Points += scorePerPerfectHit * currentMultiplier;
        NoteHit();
        // AnimationController();
    }

    public void NoteMissed()
    {
        Debug.Log("Missed note");
        consecutiveHitCount = 0; // 一旦有音符错过则重新计数
        consecutiveHitCountsLevels = 0;
        hasQuarticHitNotes = false;
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "×" + currentMultiplier;
        AnimationController();
    }

    private void AnimationController()
    {
        // // 刚开始游戏时，只要开始按键就开始跳舞
        // if (!animationStarted && currentScore > 0)
        // {
        //     animationStarted = true;
        //     curAnimationState = _random.Next(1, 5);
        //     // _animator.SetInteger(State, curAnimationState);
        //     Debug.Log("animation started");
        // }
        //
        // if (consecutiveHitCount < 4)
        // {
        //     curAnimationState = 0;
        //     // _animator.SetInteger(State, curAnimationState);
        //     Debug.Log("second circumstance HERE");
        // }
        // else
        // {
        //     if (!has20ConsecutiveHits) return;
        //     var nextState = _random.Next(1, 5);
        //     while (nextState.Equals(curAnimationState))
        //         nextState = _random.Next(1, 5);
        //     // _animator.SetInteger(State, nextState);
        //     curAnimationState = nextState;
        //     has20ConsecutiveHits = false;
        //     consecutiveHitCountsLevels = 0;
        //     Debug.Log("third circumstance HERE");
        //
        // }
        
    }
    
    
    public void PauseButtonHandler()
    {
        if (_isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void BackToStartMenuHandler()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _bgm.UnPause();
        _isGamePaused = false;
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _bgm.Pause();
        _isGamePaused = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGameFromPauseState()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackgroundMusicSlider(float volume)
    {
        hallBackgroundMusic.volume = backgroundMusicSlider.value;
    }

    public void GameMusicSlider(float volume)
    {
        _bgm.volume = gameMusicSlider.value;
    }

    public void IsBackgroundMusicActiveToggle()
    {
        if (!isBackgroundMusicActiveToggle.isOn)
            hallBackgroundMusic.Pause();
        else
        {
            hallBackgroundMusic.Play();
        }
    }

}

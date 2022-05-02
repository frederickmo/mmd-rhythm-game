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

    // public AudioSource bgm;
    public AudioSource[] bgmList;
    private AudioSource _bgm;
    public int selectedBgmIndex;

    public bool startBgm;

    public BeatScroller beatScroller;

    public static GameManager GameManagerInstance;

    public int currentScore;
    
    public int scorePerNote = 10;
    public int scorePerGoodHit = 15;
    public int scorePerPerfectHit = 20;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public PlaceOnPlane placeOnPlane;
    [FormerlySerializedAs("placedPrefabObject")] public GameObject spawnedObject;

    private Animator _animator;
    [FormerlySerializedAs("_animationStarted")] public bool animationStarted;

    [FormerlySerializedAs("_consecutiveHitCount")] public int consecutiveHitCount; // 累计击打的音符数

    [FormerlySerializedAs("_hasQuarticHitNotes")] public bool hasQuarticHitNotes; // 是否达到连续击打4个音符
    private System.Random _random = new();
    [FormerlySerializedAs("_curAnimationState")] public int curAnimationState; // 当前动画状态
    [FormerlySerializedAs("_has20ConsecutiveHits")] public bool has20ConsecutiveHits;
    [FormerlySerializedAs("_consecutiveHitCountsLevels")] public int consecutiveHitCountsLevels;
    private static readonly int State = Animator.StringToHash("animationState");

    private bool _fixSpawnedObject;

    public TextMeshProUGUI trialText;


    // Start is called before the first frame update
    void Start()
    {
        selectedBgmIndex = GlobalController.instance.selectedMusicIndex;
        _bgm = bgmList[selectedBgmIndex];
        
        _isGamePaused = false;
        GameManagerInstance = this;
        scoreText.text = "0";
        multiText.text = "×1";
        currentMultiplier = 1;
        startBgm = false;
        beatScroller.hasStarted = false;
        // spawnedObject = placeOnPlane.SpawnedObject;
        // _animator = spawnedObject.GetComponent<Animator>();
        hasQuarticHitNotes = false;
        animationStarted = false;
        has20ConsecutiveHits = false;
        consecutiveHitCount = 0;
        consecutiveHitCountsLevels = 0;
        curAnimationState = 0;
        
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

    }


    public void StartGameButtonHandler()
    {
        // if (startBgm) return;
        // if (beatScroller.hasStarted) return;
        // if (Input.touchCount <= 0) return;
        startBgm = true;
        beatScroller.hasStarted = true;
        // beatScroller.
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

        if (!animationStarted)
        {
            animationStarted = true;
            curAnimationState = _random.Next(1, 5);
            _animator.SetInteger(State, curAnimationState);
            trialText.text = "Miku is ready.";
        }
        
        if (consecutiveHitCount < 20)
            consecutiveHitCount++;
        else
        {
            consecutiveHitCount = 0;
            var randState = _random.Next(1, 5);
            while (randState.Equals(curAnimationState))
                randState = _random.Next(1, 5);
            curAnimationState = randState;
            if (_fixSpawnedObject)
            {
                _animator.SetInteger(State, curAnimationState);
                // trialText.text = "Animation changed. curState is " + curAnimationState + ".";
                trialText.text = "Nice Hit!";
            }

        }
        

    }

    public void NormalHit()
    {
        Debug.Log("Normal hit");
        consecutiveHitCount++;
        consecutiveHitCountsLevels++;
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        // AnimationController();
    }

    public void GoodHit()
    {
        Debug.Log("Good hit");
        consecutiveHitCount++;
        consecutiveHitCountsLevels++;
        currentScore += scorePerGoodHit * currentMultiplier;
        NoteHit();
        // AnimationController();
    }

    public void PerfectHit()
    {
        Debug.Log("Perfect hit");
        consecutiveHitCount++;
        consecutiveHitCountsLevels++;
        currentScore += scorePerPerfectHit * currentMultiplier;
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

}

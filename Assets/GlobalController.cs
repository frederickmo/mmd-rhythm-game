using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalController : MonoBehaviour
{

    public static GlobalController instance;

    public int selectedModelIndex;

    public int selectedMusicIndex;

    public Pose hitPose;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        selectedModelIndex = 0;
        selectedMusicIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    public GameObject beatScroller;

    [FormerlySerializedAs("_hasStarted")] public bool hasStarted;

    private float _beatPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        // if (!gameObject.activeSelf)
        // hasStarted = false;
        // 现实中的bpm是按每分钟算的，但unity里的beatTempo是按每秒算的，故除以60秒
        // beatTempo /= 60f;
        _beatPerSecond = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!hasStarted) return;
        if (!beatScroller.activeInHierarchy) return;
        beatScroller.transform.position -= new Vector3(0f, _beatPerSecond * Time.deltaTime * 360, 0f);
        
    }

    public void StartBeatScroller()
    {
        hasStarted = true;
    }
}

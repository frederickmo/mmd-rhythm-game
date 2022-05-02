using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera curCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        float aspectRatioDesign = (16f / 9f);
        float orthographicStartSize = 3.8f;
         
        float inverseAspectRatio = 1 / aspectRatioDesign;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;
 
        if (currentAspectRatio > aspectRatioDesign)
        {
            currentAspectRatio -= (currentAspectRatio - aspectRatioDesign);
        } else if (currentAspectRatio < inverseAspectRatio)
        {
            currentAspectRatio += (currentAspectRatio - inverseAspectRatio);
        }
 
        curCamera.orthographicSize = aspectRatioDesign * (orthographicStartSize / currentAspectRatio);
    }
}

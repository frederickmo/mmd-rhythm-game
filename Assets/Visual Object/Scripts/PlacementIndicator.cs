using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// 这个类是管理光标的，和模型无关呃呃呃
public class PlacementIndicator : MonoBehaviour
{

    private ARRaycastManager _rayManager;
    [SerializeField]
    private GameObject visual;

    private bool _isPrefabAlreadyPlaced = false;

    void Start()
    {
        // get the components
        _rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        // hide the placement indicator visual
        visual.SetActive(false);
    }

    void Update()
    {
        // shoot a raycast from the center of the screen
        // if (_isPrefabAlreadyPlaced) return;
        var hits = new List<ARRaycastHit>();
        _rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit an AR plane surface, update the position and rotation
        
        // 只允许更改一次位置
        // TODO: 👆🏻好像没用
        // TODO: 用visual.activeSelf禁掉也没用，再点击屏幕里任意地方还是会出现新模型。
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
            // _isPrefabAlreadyPlaced = true;
            
            // enable the visual if it's disabled
            if (!visual.activeInHierarchy)
                visual.SetActive(true);
        }
    }
}
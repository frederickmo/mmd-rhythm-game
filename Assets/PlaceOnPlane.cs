using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="PlacedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [FormerlySerializedAs("m_PlacedPrefab")]
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    
    // PlacedPrefab: 放置的3D模型
    public GameObject mPlacedPrefab;
        
    // visualObject: AR引导光标
    [SerializeField]
    public GameObject visualObject;

    public TextMeshProUGUI textText;
    

    static List<ARRaycastHit> Hits = new();

    public ARRaycastManager raycastManager;
    
    UnityEvent _placementUpdate;

    [FormerlySerializedAs("_isPrefabAlreadyPlaced")] public bool isPrefabAlreadyPlaced;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject PlacedPrefab
    {
        get => mPlacedPrefab;
        set => mPlacedPrefab = value;
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject SpawnedObject { get; private set; }

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        textText.text = "Please place the model on a plane.";

        if (_placementUpdate == null)
            _placementUpdate = new UnityEvent();

        _placementUpdate.AddListener(DisableVisual);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (isPrefabAlreadyPlaced) return;
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (!isPrefabAlreadyPlaced)
        {
            raycastManager.Raycast(touchPosition, Hits, TrackableType.PlaneWithinPolygon);
            var hitPose = Hits[0].pose;
            SpawnedObject = Instantiate(mPlacedPrefab, hitPose.position, hitPose.rotation);
            isPrefabAlreadyPlaced = true;
            textText.text = "Model successfully placed! Now enjoy your game.";
            _placementUpdate.Invoke();
        }

        // if (raycastManager.Raycast(touchPosition, Hits, TrackableType.PlaneWithinPolygon))
        // {
        //     // Raycast hits are sorted by distance, so the first one
        //     // will be the closest hit.
        //     var hitPose = Hits[0].pose;
        //
        //     // BUG: 这下面的代码是如果object未被放置就放置新的，如果已放置就更新位置。但是不知道为什么更新位置不起作用，一直在增加新的模型.
        //     if (SpawnedObject == null)
        //     {
        //         SpawnedObject = Instantiate(mPlacedPrefab, hitPose.position, hitPose.rotation);
        //         _isPrefabAlreadyPlaced = true;
        //     }
        //     else
        //     {
        //         // repositioning of the object
        //         SpawnedObject.transform.position = hitPose.position;
        //         _isPrefabAlreadyPlaced = true;
        //     }
        //     _placementUpdate.Invoke();
        // }
    }

    void DisableVisual()
    {
        visualObject.SetActive(false);
    }
}
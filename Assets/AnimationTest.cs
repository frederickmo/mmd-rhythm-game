using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationTest : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    public bool hasPlaced;
    public GameObject root;
    public KeyCode p, w, a, s, d;
    private Animator _animator;
    [FormerlySerializedAs("_spawner")] public GameObject spawner;
    private static readonly int State = Animator.StringToHash("animationState");

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPlaced)
        {
            if (Input.GetKeyDown(p))
            {
                spawner = Instantiate(prefab, root.transform.position, root.transform.rotation);
                spawner.transform.localScale *= 10000;
                _animator = spawner.GetComponent<Animator>();
            }
        }
        
        if (Input.GetKeyDown(w))
            _animator.SetInteger(State, 1);
        if (Input.GetKeyDown(a))
            _animator.SetInteger(State, 2);
        if (Input.GetKeyDown(s))
            _animator.SetInteger(State, 3);
        if (Input.GetKeyDown(d))
            _animator.SetInteger(State, 4);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;

    private static readonly int DanceState = Animator.StringToHash("animationState");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        Debug.Log(_animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            _animator.SetInteger(DanceState, 1);
        }
        else if (Input.GetKey("2"))
        {
            _animator.SetInteger(DanceState, 2);
        }
        else if (Input.GetKey("3"))
        {
            _animator.SetInteger(DanceState, 3);
        }
        else if (Input.GetKey("4"))
        {
            _animator.SetInteger(DanceState, 4);
        }
        else if (Input.GetKey("0"))
        {
            _animator.SetInteger(DanceState, 0);
        }
    }
}

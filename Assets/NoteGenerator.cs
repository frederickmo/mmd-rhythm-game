using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{

    public GameObject leftArrow;

    public GameObject downArrow;

    public GameObject rightArrow;

    public GameObject upArrow;

    public GameObject leftButton;

    public GameObject downButton;

    public GameObject rightButton;

    public GameObject upButton;

    public GameObject beatScroller;

    public KeyCode left;
    public KeyCode up;
    public KeyCode right;
    public KeyCode down;

    private Vector3 _leftButtonPos, _upButtonPos, _rightButtonPos, _downButtonPos;
    // Start is called before the first frame update
    void Start()
    {
        _leftButtonPos = leftButton.transform.position;
        _upButtonPos = upButton.transform.position;
        _rightButtonPos = rightButton.transform.position;
        _downButtonPos = downButton.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(left))
        {
            
            Instantiate(leftArrow, _leftButtonPos, transform.rotation, beatScroller.transform);
            
        }
        else if (Input.GetKeyDown(up))
        {
            Instantiate(upArrow, _upButtonPos, transform.rotation, beatScroller.transform);
        }
        else if (Input.GetKeyDown(right))
        {
            Instantiate(rightArrow, _rightButtonPos, transform.rotation, beatScroller.transform);
        }
        else if (Input.GetKeyDown(down))
        {
            Instantiate(downArrow, _downButtonPos, transform.rotation, beatScroller.transform);
        }
    }

}

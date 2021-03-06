using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DownNoteObjectHandler : MonoBehaviour
{
    // private bool onTouchThisButton;
    public bool canBePressed;
    [FormerlySerializedAs("button")] public Button downButton;
    public GameObject hitEffect, goodEffect, perfectEffect;
    public GameObject parentNode;

    // Start is called before the first frame update
    void Start()
    {
        // downButton = GameObject.Find("DownButton").GetComponent<Button>();
        downButton.onClick.AddListener(SetArrowUnactivated);
    }

    // Update is called once per frame
    void Update()
    {
        // if (onTouchThisButton)
        // {
        //     if (canBePressed)
        //     {
        //         gameObject.SetActive(false);
        //     }
        // }
    }

    private void SetArrowUnactivated()
    {
        // 这里如果不加activeSelf的检测的话，等音符掉下去了还可以一直判定为canBePressed然后一直加分
        if (!canBePressed || !gameObject.activeSelf) return;
        gameObject.SetActive(false);
        // GameManager.GameManagerInstance.NoteHit();

        // 计算音符与按钮的绝对距离：分三个层次计分
        var distance = Mathf.Abs(transform.position.y - downButton.transform.position.y);
        switch (distance)
        {
            case <= 5:
                GameManager.gameManagerInstance.PerfectHit();
                Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation, parentNode.transform);
                break;
            case > 5 and <= 40:
                GameManager.gameManagerInstance.GoodHit();
                Instantiate(goodEffect, transform.position, goodEffect.transform.rotation, parentNode.transform);
                break;
            default:
                GameManager.gameManagerInstance.NormalHit();
                Instantiate(hitEffect, transform.position, hitEffect.transform.rotation, parentNode.transform);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 音符在离开按钮瞬间且处于激活状态（表明之前已与按钮发生碰撞）若为被点击则记为missed
        if (!other.CompareTag("Activator") || !gameObject.activeSelf) return;
        canBePressed = false;
        GameManager.gameManagerInstance.NoteMissed();
        // Instantiate(missEffect, transform.position, missEffect.transform.rotation);
    }
}

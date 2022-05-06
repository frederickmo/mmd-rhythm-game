using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBetweenScenesController : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("OnCircleWipeOut");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EnterGameSceneAnimation()
    {
        StartCoroutine(OnEnterGameSceneAnimation());
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator OnEnterGameSceneAnimation()
    {
        animator.SetTrigger("OnCircleWipeIn");
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("OnCircleWipeOut");
    }

}

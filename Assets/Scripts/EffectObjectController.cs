using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectController : MonoBehaviour
{
    public float lifetime = 1f;

    public GameObject effectObject;

    // Start is called before the first frame update
    void Start()
    {
        // GameObject.Instantiate(effectObject);
        // Instantiate(effectObject, transform.position, effectObject.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float DestroyIfLowerThan = -100; 
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < DestroyIfLowerThan)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}

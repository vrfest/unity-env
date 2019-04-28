using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Ouch");
        if (collision.gameObject.tag == "SurfaceLevel") {
            Debug.Log("Wow");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

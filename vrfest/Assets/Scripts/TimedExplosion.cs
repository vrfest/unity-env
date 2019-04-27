using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int count = 500;
    // Update is called once per frame
    void Update()
    {
        if (count++ == 500) {
            BroadcastMessage("Explode");
            Destroy(gameObject);
        }
    }
}

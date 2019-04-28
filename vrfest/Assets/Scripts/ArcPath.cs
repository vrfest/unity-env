using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SOOOO MUCH TECHNICAL DEBT.
public class ArcPath : MonoBehaviour
{
    const float finalX = 10f;
    const float finalZ = -300f;
    const float halfFinalZ = -200f;

    Vector3 xMove = new Vector3(0.25f, 0, 0);
    Vector3 yMove = new Vector3(0, 0.5f, 0);
    Vector3 zMove = new Vector3(0, 0, 0.5f);

    int degreesRotated = 0;
    bool rotate = true;


    Route routeScript;

    // Start is called before the first frame update
    void Start()
    {
        routeScript = GameObject.Find("RouteScript").GetComponent<Route>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y <= 3) {
            routeScript.phase = 5;
            GameObject saloon_main = gameObject.transform.Find("Objects").Find("Saloon").Find("saloon_main").gameObject;
            saloon_main.BroadcastMessage("Explode");
            Destroy(gameObject);
        }
        if (gameObject.transform.position.x > finalX) gameObject.transform.position -= xMove;
        if (gameObject.transform.position.z > finalZ) {
            if (gameObject.transform.position.z > halfFinalZ) gameObject.transform.position += yMove;
            else gameObject.transform.position -= yMove;
            gameObject.transform.position -= zMove;
        }
        else gameObject.transform.position -= yMove;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(180, 60, 0), Time.deltaTime * 5);
    }
}

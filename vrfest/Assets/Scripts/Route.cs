using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    const float stage_y_end = -7.42f;
    Vector3 stage_increment = new Vector3(0, 0.25f, 0);

    GameObject surface;
    GameObject stage;

    int phase = 1;

    void Start()
    {
        surface = GameObject.Find("SurfaceLevel");
        stage = GameObject.Find("StageLevel");
    }

    void Update()
    {
        switch (phase) {
            case 1:
                if(stage.transform.position.y < stage_y_end) {
                    stage.transform.position += stage_increment;
                }
                else {
                    stage.transform.position = new Vector3(stage.transform.position.x, stage_y_end, stage.transform.position.z);
                    phase++;
                }
                break;
            case 2:
                foreach(GameObject item in GameObject.FindGameObjectsWithTag("Phase2Explode")) {
                    item.GetComponent<MeshExploder>().Explode();
                }
                break;
        }
    }
}

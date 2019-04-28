using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    const float stage_y_end = -3.4f;
    Vector3 stage_increment = new Vector3(0, 0.1f, 0);

    const float TV_y_end = 9f;
    Vector3 TV_increment = new Vector3(0, 0.05f, 0);

    float VideoObject_scale_x_end;
    float VideoObject_scale_y_end;
    Vector3 VideoObject_scale_increment;

    GameObject surface;
    GameObject stage;
    GameObject interior;
    GameObject TV;
    GameObject VideoObject;

    public int phase = 1;

    void Start()
    {
        surface = GameObject.Find("SurfaceLevel");
        stage = GameObject.Find("StageLevel");
        interior = GameObject.Find("InteriorLevel");
        TV = GameObject.Find("TV");
        VideoObject = TV.transform.Find("VideoObject").gameObject;
        VideoObject_scale_x_end = VideoObject.transform.localScale.x * 8;
        VideoObject_scale_y_end = VideoObject.transform.localScale.y * 8;
        VideoObject_scale_increment = new Vector3(VideoObject_scale_x_end / 100, VideoObject_scale_y_end / 100, 0);
    }

    void Update()
    {
        switch (phase) {
            case 1: // Stage rises, bar explodes
                if(stage.transform.position.y < stage_y_end) {
                    stage.transform.position += stage_increment;
                }
                else {
                    stage.transform.position = new Vector3(stage.transform.position.x, stage_y_end, stage.transform.position.z);
                    //Destroy(interior);
                    interior.GetComponent<ArcPath>().enabled = true;
                    phase++;
                }
                break;
            case 2: // TV rises, frame explodes
                if (TV.transform.position.y < TV_y_end) {
                    TV.transform.position += TV_increment;
                }
                else {
                    TV.transform.position = new Vector3(TV.transform.position.x, TV_y_end, TV.transform.position.z);
                    GameObject frame = TV.transform.Find("Frame").gameObject;
                    frame.BroadcastMessage("Explode");
                    Destroy(frame);
                    phase++;
                }
                break;
            case 3: // TV Expands
                if(VideoObject.transform.localScale.x < VideoObject_scale_x_end && TV.transform.localScale.y < VideoObject_scale_y_end) {
                    VideoObject.transform.localScale += VideoObject_scale_increment;
                }
                else {
                    VideoObject.transform.localScale = new Vector3(VideoObject_scale_x_end, VideoObject_scale_y_end, VideoObject.transform.localScale.z);
                    phase++;
                }
                break;
            case 4: // Wait on interior to fall
                break;
            case 5: // Mars explodes
                Transform planetTransform = surface.transform.Find("Environment");
                int planetChildCount = planetTransform.childCount;
                for(int i = 0; i < planetChildCount; i++) {
                    GameObject child = planetTransform.GetChild(i).gameObject;
                    child.BroadcastMessage("Explode");
                    Destroy(child);
                }
                phase++;
                break;
                
        }
    }
}

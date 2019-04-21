using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoBehavior : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    //private int delay = 500;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        videoPlayer.Play();
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if (delay-- == 0) {
        //    videoPlayer.Play();
        //    Destroy(this);
        //}
        //Debug.Log(delay);
    }
}

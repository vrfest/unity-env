using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public HashSet<string> friends;
    public string currentUsername = null;
    GameObject PlayerInteractionCanvas;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInteractionCanvas = GameObject.Find("PlayerInteractionCanvas");
        friends = new HashSet<string>();
    }

    public void test() {
        Debug.Log("Success");
    }

    public void openFacebook() {
        Application.OpenURL("http://www.facebook.com");
    }

    public void openInstagram() {
        Application.OpenURL("http://www.instagram.com");
    }

    public void openTwitter() {
        Application.OpenURL("http://www.twitter.com");
    }

    public void addFriend() {
        if (friends.Add(currentUsername)) { // Wasn't friends before

        }
        else {

        }
    }


    public void close() {
        PlayerInteractionCanvas.GetComponent<Canvas>().enabled = false; ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

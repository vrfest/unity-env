using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public HashSet<string> friends;
    public string currentUsername = null;
    public List<string> currentInventory;
    public GameObject currentGO;
    public GameObject HUDCanvas;
    public HTC.UnityPlugin.Vive.Menuable currentMenuable;

    public float balance;
    public List<string> playerInventory;
    float lastPrice;
    GameObject PlayerInteractionCanvas;
    // Start is called before the first frame update
    void Start()
    {
        balance = 3.9724F;
        lastPrice = 0F;
        PlayerInteractionCanvas = GameObject.Find("PlayerInteractionCanvas");
        HUDCanvas = GameObject.Find("HUDCanvas");
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
            PlayerInteractionCanvas.transform.Find("AddFriend").transform.Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text = "Added!";
        }
        else {
            PlayerInteractionCanvas.transform.Find("AddFriend").transform.Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text = "Already Friends";
        }
    }

    public void inventory() { // Massive technical debt + hard-coding
        if (currentInventory.Count > 0) {
            if (currentInventory[0].Equals("Horse Head")) {
                PlayerInteractionCanvas.transform.Find("HorseHead").gameObject.GetComponent<UnityEngine.UI.Button>().enabled = true;
                PlayerInteractionCanvas.transform.Find("HorseHead").Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
            else {
                PlayerInteractionCanvas.transform.Find("DevilOrAngel").gameObject.GetComponent<UnityEngine.UI.Button>().enabled = true;
                PlayerInteractionCanvas.transform.Find("DevilOrAngel").Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
        }
        PlayerInteractionCanvas.transform.Find("InventoryText").GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    public void displayPrice(string item) { // Massive technical debt + hard-coding
        GameObject descGO = PlayerInteractionCanvas.transform.Find("DescriptionText").gameObject;

        descGO.GetComponent<UnityEngine.UI.Text>().enabled = true;
        string price = "0.0" + Random.Range(1000, 8000);
        lastPrice = float.Parse(price);
        descGO.GetComponent<UnityEngine.UI.Text>().text = currentUsername + " has listed \'" + item + "\' for: " + price + " Ether";

        GameObject purchaseGO = PlayerInteractionCanvas.transform.Find("Purchase").gameObject;
        purchaseGO.GetComponent<UnityEngine.UI.Button>().enabled = true;
        purchaseGO.GetComponent<UnityEngine.UI.Image>().enabled = true;
        purchaseGO.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().enabled = true;
        purchaseGO.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = true;
    }

    public void purchase() {
        if (lastPrice > 0F) {
            balance -= lastPrice;
            HUDCanvas.transform.Find("BalanceText").GetComponent<UnityEngine.UI.Text>().text = "Your balance: " + balance + " ETH";
            playerInventory.Add(currentInventory[0]);
            currentMenuable.inventory.Clear();
            PlayerInteractionCanvas.transform.Find("HorseHead").gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
            PlayerInteractionCanvas.transform.Find("HorseHead").Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = false;
            PlayerInteractionCanvas.transform.Find("DevilOrAngel").gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
            PlayerInteractionCanvas.transform.Find("DevilOrAngel").Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = false;
            PlayerInteractionCanvas.transform.Find("DescriptionText").GetComponent<UnityEngine.UI.Text>().enabled = false;
            lastPrice = 0;
        }
    }

    public void hidePlayerInventory() {

    }

    public void close() {
        PlayerInteractionCanvas.GetComponent<Canvas>().enabled = false;
        PlayerInteractionCanvas.transform.Find("HorseHead").gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
        PlayerInteractionCanvas.transform.Find("HorseHead").Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = false;
        PlayerInteractionCanvas.transform.Find("DevilOrAngel").gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
        PlayerInteractionCanvas.transform.Find("DevilOrAngel").Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = false;
        PlayerInteractionCanvas.transform.Find("InventoryText").GetComponent<UnityEngine.UI.Text>().enabled = false;
        PlayerInteractionCanvas.transform.Find("DescriptionText").GetComponent<UnityEngine.UI.Text>().enabled = false;

        GameObject purchaseGO = PlayerInteractionCanvas.transform.Find("Purchase").gameObject;
        purchaseGO.GetComponent<UnityEngine.UI.Button>().enabled = false;
        purchaseGO.GetComponent<UnityEngine.UI.Image>().enabled = false;
        purchaseGO.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().enabled = false;
        purchaseGO.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

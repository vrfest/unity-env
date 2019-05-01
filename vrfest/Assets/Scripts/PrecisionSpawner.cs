using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionSpawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    public Material[] clothes;
    public Sprite[] usernames;
    //public Sprite[] face;

    void Start() {
        GameObject[] locations = GameObject.FindGameObjectsWithTag("SpawnLocation");
        int usernamesIndex = 0;
        foreach (GameObject location in locations) {
            var rot = Quaternion.AngleAxis(Random.value * Mathf.PI, Vector2.up);
            var go = Instantiate(_prefab, location.transform.position, rot);
            go.transform.Find("Body").gameObject.GetComponent<SkinnedMeshRenderer>().material = clothes[Random.Range(0, clothes.Length)]; // Set random clothing
            int propIndex = Random.Range(1, 3);
            switch (propIndex) {
                case 1:
                    Destroy(go.transform.Find("Neo_Spine").transform.Find("DemonWings").gameObject);
                    Destroy(go.transform.Find("Neo_Spine1").transform.Find("Halo").gameObject);
                    go.transform.Find("Body").gameObject.GetComponent<HTC.UnityPlugin.Vive.Menuable>().inventory.Add("HorseHead");
                    break;
                case 2:
                    Destroy(go.transform.Find("Neo_Spine1").transform.Find("HorseHead").gameObject);
                    go.transform.Find("Body").gameObject.GetComponent<HTC.UnityPlugin.Vive.Menuable>().inventory.Add("DemonWings");
                    go.transform.Find("Body").gameObject.GetComponent<HTC.UnityPlugin.Vive.Menuable>().inventory.Add("Halo");
                    break;
            }
            go.transform.Find("Body").gameObject.GetComponent<HTC.UnityPlugin.Vive.Menuable>().username = usernames[usernamesIndex].name;
            go.transform.Find("Neo_Spine").transform.Find("Username").gameObject.GetComponent<SpriteRenderer>().sprite = usernames[usernamesIndex++];
            
            var dancer = go.GetComponent<Puppet.Dancer>();

            dancer.footDistance *= Random.Range(0.8f, 2.0f);
            dancer.stepFrequency *= Random.Range(0.4f, 1.6f);
            dancer.stepHeight *= Random.Range(0.75f, 1.25f);
            dancer.stepAngle *= Random.Range(0.75f, 1.25f);

            dancer.hipHeight *= Random.Range(0.75f, 1.25f);
            dancer.hipPositionNoise *= Random.Range(0.75f, 1.25f);
            dancer.hipRotationNoise *= Random.Range(0.75f, 1.25f);

            dancer.spineBend = Random.Range(4.0f, -12.0f);
            dancer.spineRotationNoise *= Random.Range(0.75f, 1.25f);

            dancer.handPositionNoise *= Random.Range(0.5f, 2.0f);
            dancer.handPosition += Random.insideUnitSphere * 0.25f;

            dancer.headMove *= 0.0001f;//Random.Range(0.2f, 2.8f);
            dancer.noiseFrequency *= Random.Range(0.4f, 1.8f);
            dancer.randomSeed = Random.Range(0, 0xffffff);

            //var renderer = dancer.GetComponentInChildren<Renderer>();
            //renderer.material.color = Random.ColorHSV(0, 1, 0.6f, 0.8f, 0.8f, 1.0f);
        }
    }
}

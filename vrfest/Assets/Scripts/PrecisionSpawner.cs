using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrecisionSpawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    void Start() {
        GameObject[] locations = GameObject.FindGameObjectsWithTag("SpawnLocation");

        foreach (GameObject location in locations) {
            var rot = Quaternion.AngleAxis(UnityEngine.Random.value * Mathf.PI, Vector2.up);

            var go = Instantiate(_prefab, location.transform.position, rot);
            var dancer = go.GetComponent<Puppet.Dancer>();

            dancer.footDistance *= UnityEngine.Random.Range(0.8f, 2.0f);
            dancer.stepFrequency *= UnityEngine.Random.Range(0.4f, 1.6f);
            dancer.stepHeight *= UnityEngine.Random.Range(0.75f, 1.25f);
            dancer.stepAngle *= UnityEngine.Random.Range(0.75f, 1.25f);

            dancer.hipHeight *= UnityEngine.Random.Range(0.75f, 1.25f);
            dancer.hipPositionNoise *= UnityEngine.Random.Range(0.75f, 1.25f);
            dancer.hipRotationNoise *= UnityEngine.Random.Range(0.75f, 1.25f);

            dancer.spineBend = UnityEngine.Random.Range(4.0f, -16.0f);
            dancer.spineRotationNoise *= UnityEngine.Random.Range(0.75f, 1.25f);

            dancer.handPositionNoise *= UnityEngine.Random.Range(0.5f, 2.0f);
            dancer.handPosition += UnityEngine.Random.insideUnitSphere * 0.25f;

            dancer.headMove *= UnityEngine.Random.Range(0.2f, 2.8f);
            dancer.noiseFrequency *= UnityEngine.Random.Range(0.4f, 1.8f);
            dancer.randomSeed = UnityEngine.Random.Range(0, 0xffffff);

            //var renderer = dancer.GetComponentInChildren<Renderer>();
            //renderer.material.color = UnityEngine.Random.ColorHSV(0, 1, 0.6f, 0.8f, 0.8f, 1.0f);
        }
    }
}

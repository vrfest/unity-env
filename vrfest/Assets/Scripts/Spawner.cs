using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _columns = 10;
    [SerializeField] int _rows = 10;
    [SerializeField] float _interval = 1;
    [SerializeField] GameObject _origin;
    [SerializeField] GameObject _end;
    void Start()
    {
        // Find the markers indicating the boundaries to spawn dancers
        Vector3 origin = _origin.GetComponent<Transform>().position;
        Vector3 end = _end.GetComponent<Transform>().position;
        // Calculate how much to shift coordinates per dancer
        float originX = origin.x;
        float originZ = origin.z;
        float endX = end.x;
        float endZ = end.z;
        float columnShift = Math.Abs((endX - originX) / _columns);
        float rowShift = Math.Abs((endZ - originZ) / _rows);
        for (var i = 0; i < _columns; i++)
        {
            originZ = origin.z;
            //var x = _interval * (i - _columns * 0.5f + 0.5f);
            for (var j = 0; j < _rows; j++)
            {
                //var y = _interval * (j - _rows * 0.5f + 0.5f);
                var pos = new Vector3(originX + UnityEngine.Random.Range(-2.5f, 2.5f), -2.963375f, originZ + UnityEngine.Random.Range(-2.5f, 2.5f));
                originZ -= rowShift;
                var rot = Quaternion.AngleAxis(UnityEngine.Random.value * Mathf.PI, Vector2.up);

                var go = Instantiate(_prefab, pos, rot);
                var dancer = go.GetComponent<Puppet.Dancer>();

                dancer.footDistance  *= UnityEngine.Random.Range(0.8f, 2.0f);
                dancer.stepFrequency *= UnityEngine.Random.Range(0.4f, 1.6f);
                dancer.stepHeight    *= UnityEngine.Random.Range(0.75f, 1.25f);
                dancer.stepAngle     *= UnityEngine.Random.Range(0.75f, 1.25f);

                dancer.hipHeight        *= UnityEngine.Random.Range(0.75f, 1.25f);
                dancer.hipPositionNoise *= UnityEngine.Random.Range(0.75f, 1.25f);
                dancer.hipRotationNoise *= UnityEngine.Random.Range(0.75f, 1.25f);

                dancer.spineBend           = UnityEngine.Random.Range(4.0f, -16.0f);
                dancer.spineRotationNoise *= UnityEngine.Random.Range(0.75f, 1.25f);

                dancer.handPositionNoise *= UnityEngine.Random.Range(0.5f, 2.0f);
                dancer.handPosition      += UnityEngine.Random.insideUnitSphere * 0.25f;

                dancer.headMove       *= UnityEngine.Random.Range(0.2f, 2.8f);
                dancer.noiseFrequency *= UnityEngine.Random.Range(0.4f, 1.8f);
                dancer.randomSeed      = UnityEngine.Random.Range(0, 0xffffff);

                var renderer = dancer.GetComponentInChildren<Renderer>();
                renderer.material.color = UnityEngine.Random.ColorHSV(0, 1, 0.6f, 0.8f, 0.8f, 1.0f);
            }
            originX -= columnShift;
        }
        Destroy(_prefab);
    }
}

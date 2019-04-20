using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _columns = 10;
    [SerializeField] int _rows = 10;
    [SerializeField] float _interval = 1;

    void Start()
    {
        Vector3 origin = GameObject.Find("OriginMarker").GetComponent<Transform>().position;
        Vector3 end = GameObject.Find("EndMarker").GetComponent<Transform>().position;
        float originX = origin.x;
        float originY = origin.y;
        float endX = end.x;
        float endY = end.y;
        for (var i = 0; i < _columns; i++)
        {
            var x = _interval * (i - _columns * 0.5f + 0.5f);

            for (var j = 0; j < _rows; j++)
            {
                var y = _interval * (j - _rows * 0.5f + 0.5f);

                var pos = new Vector3(x*5, 0, y*5);
                var rot = Quaternion.AngleAxis(Random.value * Mathf.PI, Vector2.up);

                var go = Instantiate(_prefab, pos, rot);
                var dancer = go.GetComponent<Puppet.Dancer>();

                dancer.footDistance  *= Random.Range(0.8f, 2.0f);
                dancer.stepFrequency *= Random.Range(0.4f, 1.6f);
                dancer.stepHeight    *= Random.Range(0.75f, 1.25f);
                dancer.stepAngle     *= Random.Range(0.75f, 1.25f);

                dancer.hipHeight        *= Random.Range(0.75f, 1.25f);
                dancer.hipPositionNoise *= Random.Range(0.75f, 1.25f);
                dancer.hipRotationNoise *= Random.Range(0.75f, 1.25f);

                dancer.spineBend           = Random.Range(4.0f, -16.0f);
                dancer.spineRotationNoise *= Random.Range(0.75f, 1.25f);

                dancer.handPositionNoise *= Random.Range(0.5f, 2.0f);
                dancer.handPosition      += Random.insideUnitSphere * 0.25f;

                dancer.headMove       *= Random.Range(0.2f, 2.8f);
                dancer.noiseFrequency *= Random.Range(0.4f, 1.8f);
                dancer.randomSeed      = Random.Range(0, 0xffffff);

                var renderer = dancer.GetComponentInChildren<Renderer>();
                renderer.material.color = Random.ColorHSV(0, 1, 0.6f, 0.8f, 0.8f, 1.0f);
            }
        }
    }
}

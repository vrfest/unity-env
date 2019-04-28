using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    int initialPause = 500;

    const float TV_y_end = 6f;
    Vector3 TV_increment = new Vector3(0, 0.05f, 0);

    float VideoObject_scale_x_end;
    float VideoObject_scale_y_end;
    Vector3 VideoObject_scale_increment;

    GameObject surface;
    GameObject stage;
    GameObject interior;
    GameObject TV;
    GameObject VideoObject;

    int cooldown = 0; // Please remove later, technical debt high

    public GameObject[] particles;
    int particlesIndex = 0;
    Quaternion rot = new Quaternion();
    int particleDelay = 0;
    List<GameObject> activeParticles = new List<GameObject>();

    private List<Vector3> particleLocations;

    public int phase = 1;

    void Start()
    {
        surface = GameObject.Find("SurfaceLevel");
        stage = GameObject.Find("StageLevel");
        interior = GameObject.Find("InteriorLevel");
        TV = GameObject.Find("TV");
        VideoObject = TV.transform.Find("VideoObject").gameObject;
        VideoObject_scale_x_end = VideoObject.transform.localScale.x * 4;
        VideoObject_scale_y_end = VideoObject.transform.localScale.y * 4;
        VideoObject_scale_increment = new Vector3(VideoObject_scale_x_end / 100, VideoObject_scale_y_end / 100, 0);

        particleLocations = new List<Vector3>();
        foreach(GameObject particleLocation in GameObject.FindGameObjectsWithTag("ParticleLocation")) {
            particleLocations.Add(particleLocation.transform.position);
        }
    }

    void Update()
    {
        // t e c h n i c a l d e b t
        switch (phase) {
            case 1: // Pause before interior flies off
                if(initialPause-- < 0) {
                    interior.GetComponent<ArcPath>().enabled = true;
                    stage.transform.GetChild(0).gameObject.SetActive(true);
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
            case 6: // Cooldown
                if (cooldown++ > 20) phase++;
                break;
            case 7: // PS Lightshow
                if(particleDelay <= 0) {
                    particleDelay = 200;
                    foreach (GameObject activeParticle in activeParticles) {
                        Destroy(activeParticle);
                    }
                    activeParticles.Clear();
                    foreach (Vector3 particleLocation in particleLocations) {
                        GameObject particlePrefab = Instantiate(particles[particlesIndex], particleLocation, rot);
                        activeParticles.Add(particlePrefab);
                        ParticleSystem particlePrefabPS = particlePrefab.GetComponent<ParticleSystem>();
                        particlePrefabPS.Play();
                    }
                    if (particlesIndex++ == particles.Length) particlesIndex = 0;
                }
                else particleDelay--;
                
                break;
                
        }
    }
}

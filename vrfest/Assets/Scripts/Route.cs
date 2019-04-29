using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    int phaseDelay = 400;

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
                if(phaseDelay-- < 0) {
                    particleDelay = 200;
                    foreach (Vector3 particleLocation in particleLocations) {
                        GameObject particlePrefab = Instantiate(particles[particlesIndex], particleLocation, rot);
                        activeParticles.Add(particlePrefab);
                        ParticleSystem particlePrefabPS = particlePrefab.GetComponent<ParticleSystem>();
                        particlePrefabPS.Play();
                    }
                    Destroy(interior);

                    stage.transform.GetChild(0).gameObject.SetActive(true);
                    phase++;
                }
                break;
            case 2: // TV rises, frame explodes
               
                if (TV.transform.position.y < TV_y_end) {
                    TV.transform.position += TV_increment;
                    if (particleDelay-- < 0) {
                        foreach (GameObject activeParticle in activeParticles) {
                            Destroy(activeParticle);
                        }
                        activeParticles.Clear();
                    }
                }
                else {
                    particleDelay = 0;
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
                    phaseDelay = 400;
                    phase++;
                }
                break;
            case 4: // Remove Mars
                if (phaseDelay-- > 0) break;
                foreach (Vector3 particleLocation in particleLocations) {
                    GameObject particlePrefab = Instantiate(particles[particlesIndex], particleLocation, rot);
                    activeParticles.Add(particlePrefab);
                    ParticleSystem particlePrefabPS = particlePrefab.GetComponent<ParticleSystem>();
                    particlePrefabPS.Play();
                }
                particlesIndex++;
                Destroy(surface);
                foreach (GameObject activeParticle in activeParticles) {
                    Destroy(activeParticle);
                }
                activeParticles.Clear();
                particleDelay = 0;
                phase++;
                break;
            case 5: // PS Lightshow
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

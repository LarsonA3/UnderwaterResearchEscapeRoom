using System;
using UnityEngine;

public class ReactorCore : MonoBehaviour
{
    public Light coreLight;
    public MeshRenderer coreMesh;
    public Material coreBlue;
    public Material coreRed;
    [SerializeField] private AudioSource reactorAudioSource;
    private float intensity = 0f;
    public GameObject reactor;
    public GameObject doorLock1;
    public GameObject doorLock2;

    public bool reactivated = false;
    private bool up = true;

    void Start() {
        if (reactorAudioSource == null) reactorAudioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (up) {
            intensity += Time.deltaTime;
            if (intensity >= 1f) {
                intensity = 1f;
                up = false;
            }
        }
        else {
            intensity -= Time.deltaTime;
            if (intensity <= 0.5f) {
                intensity = 0.5f;
                up = true;
            }
        }

        if (coreLight != null) coreLight.intensity = intensity * 10f;
    }

    public void SetLightColor(Color color) { if (coreLight != null) coreLight.color = color; }
    public void SetCoreBlue() { 
        if (coreMesh != null) { 
            coreMesh.material = coreBlue; 
            reactivated = true;
            SetLightColor(Color.cyan);
            Destroy(doorLock1);
            Destroy(doorLock2);
            if (reactorAudioSource != null)
            {
                reactorAudioSource.spatialBlend = 1.0f;
                reactorAudioSource.Play();
            }
        } 
    }
    public void SetCoreRed() { if (coreMesh != null) coreMesh.material = coreRed; }
}

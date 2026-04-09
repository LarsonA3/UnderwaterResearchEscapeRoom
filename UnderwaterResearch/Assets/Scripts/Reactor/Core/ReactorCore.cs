using System;
using UnityEngine;

public class ReactorCore : MonoBehaviour
{
    public Light coreLight;
    public MeshRenderer coreMesh;
    public Material coreBlue;
    public Material coreRed;
    private float intensity = 0f;

    private bool up = true;

    void Start() {}

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
    public void SetCoreBlue() { if (coreMesh != null) coreMesh.material = coreBlue; }
    public void SetCoreRed() { if (coreMesh != null) coreMesh.material = coreRed; }
}

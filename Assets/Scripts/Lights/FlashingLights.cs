using UnityEngine;

public class FlashingLights : MonoBehaviour
{
    private float timer;
    private float maxIntensity;
    private Light light;
    
    private void Awake()
    {
        light = GetComponent<Light>();
        maxIntensity = 4f;
    }
    
    void Update()
    {
        light.intensity = Mathf.PingPong(Time.time * 4, maxIntensity);
    }
}

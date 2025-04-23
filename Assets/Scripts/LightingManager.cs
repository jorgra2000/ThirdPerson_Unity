using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0,24)] private float timeDay;
    [SerializeField] GameManager gameManager;


    private void Update()
    {
        if(preset == null) 
        {
            return;
        }

        if (Application.isPlaying) 
        {
            timeDay += Time.deltaTime;
            if (timeDay >= 24f)
            {
                timeDay %= 24f;
                gameManager.ChangeDay();
            }
            UpdateLighting(timeDay/24f);

        }
    }

    private void UpdateLighting(float timePercent) 
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if(directionalLight != null) 
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }


    private void OnValidate()
    {
        if(directionalLight != null) 
        {
            return;
        }
        if(RenderSettings.sun != null) 
        {
            directionalLight = RenderSettings.sun;
        }
        else 
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights) 
            {
                if(light.type == LightType.Directional) 
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}

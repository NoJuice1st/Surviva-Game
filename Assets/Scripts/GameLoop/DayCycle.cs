using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    private Color originalSkyColor;
    private Color originalFogColor;
    public static int day;
    public float DayLength;
    public float currentTime;
    public GameObject sun;
    public float sunSpeed;
    public GameManager gm;

    private void Start() {
        originalSkyColor = RenderSettings.ambientSkyColor;
        originalFogColor = RenderSettings.fogColor;
        RenderSettings.fog = true;
        sunSpeed = 360 / DayLength;
        sun.transform.rotation = Quaternion.Euler(0, -30, 0);
    }

    private void Update() {
        if (currentTime >= DayLength)
        {
            currentTime = 0;
            day++;
            gm.SetCurrentDay(day);
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        if (currentTime >= DayLength / 2)
        {
            RenderSettings.ambientSkyColor = new Color();
            RenderSettings.fogColor = new Color();
            RenderSettings.fogDensity = 0.04f;
            gm.NightTime(true);
            //RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.ambientSkyColor = originalSkyColor;
            RenderSettings.fogColor = originalFogColor;
            RenderSettings.fogDensity = 0.02f;
            gm.NightTime(false);
            //RenderSettings.fog = false;

        }

        if(sun.gameObject.transform.rotation.x >= 360 )
        {
            sun.transform.rotation = Quaternion.Euler(0, -30, 0);
        }

        sun.gameObject.transform.Rotate(sunSpeed * Time.deltaTime, 0, 0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrail : MonoBehaviour
{
    private void Start()
    {
        TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer t in trails)
        {
            t.startWidth = 0.3f;
            t.endWidth = 1;
        }
    }
}

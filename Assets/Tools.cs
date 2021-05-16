using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools 
{
    public static Vector3 RandomScale(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        Vector3 randomScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
        return randomScale;
    }
}

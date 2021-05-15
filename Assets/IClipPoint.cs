using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IClipPoint : MonoBehaviour
{
    Vector3 ClipPoint;
    void Start()
    {
        ClipPoint = this.transform.position;
    }
}

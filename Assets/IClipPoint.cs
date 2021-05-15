using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IClipPoint : MonoBehaviour
{
    public Vector3 ClipPoint { get; set; }
    void Start()
    {
        ClipPoint = this.transform.position;
    }
}

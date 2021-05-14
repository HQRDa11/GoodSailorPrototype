using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITurnAround : MonoBehaviour
{
    Transform point;
    private void Start()
    {
        point = this.gameObject.transform.parent.gameObject.transform;
    }

    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround( point.position, Vector3.up, 45 * Time.deltaTime);
    }
}


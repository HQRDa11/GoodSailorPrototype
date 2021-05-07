using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255, Random.Range(10, 200));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

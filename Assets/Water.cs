using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Vector3 initPos;
    Vector3 lowerPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        lowerPos = transform.position += Vector3.down ;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    // ici il faudrait faire bouger l ocean entre les deux valeurs
    //    this.gameObject.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", Vector3.left) ;
    //}
}

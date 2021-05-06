using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCamera : MonoBehaviour
{
    public GameObject boat;
    Vector3 initialPos;
    //Quaternion initialRot;

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
        initialPos = this.gameObject.transform.position;
        //initialRot = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = boat.transform.position + initialPos;
        //this.gameObject.transform.rotation = boat.transform.rotation * initialRot;
    }
}

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

        //Update camera:
        Camera.main.transform.position += Vector3.up * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed)*5 ;
        Camera.main.transform.position += Vector3.back * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed)*3;
    }
}
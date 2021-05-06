using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolllowBoat : MonoBehaviour
{
    private GameObject boat;
    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = boat.transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{

    public GameObject boat;
    public GameObject wind;
    public GameObject goal;
    public Text goalDistance;

    public float timerMax;
    public float timerCurrent;
    public Vector3 CameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        timerMax = 4;
        wind = GameObject.Find("Wind");
        boat = GameObject.Find("Boat");
        goal = GameObject.Find("Goal");
        goalDistance = GameObject.Find("GoalDistance").GetComponent<Text>();
        timerCurrent = 0;
        this.CameraOffset = Camera.main.transform.position - this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //instantiate decoratives
        timerCurrent += Time.deltaTime;
        if (timerCurrent >= timerMax)
        {
            GameObject newDecotatives = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Decoratives"), this.gameObject.transform);
            newDecotatives.transform.position = boat.transform.position + Vector3.forward * 256;
            newDecotatives.transform.position += Vector3.down * 6.17f;
            timerCurrent = 0;
        }

        goalDistance.text = "Goal: " + ((int)Vector3.Distance(boat.transform.position, goal.transform.position)).ToString() + "m";
    }

}


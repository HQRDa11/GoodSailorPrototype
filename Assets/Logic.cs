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
    public AudioSource audioSource;
    public AudioClip audioClip;
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
        audioClip = Resources.Load<AudioClip>("AudioClips/Ocean");
        timerCurrent = 0;
        this.CameraOffset = Camera.main.transform.position - this.transform.position;

        goalDistance = GameObject.Find("GoalDistance").GetComponent<Text>();


        audioSource = boat.gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
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

            GameObject newObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/IslandObstacle"), this.gameObject.transform);
            newObstacle.transform.position = boat.transform.position + Vector3.forward * 256;
            newObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));

            GameObject newLongObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/LongObstacle"), this.gameObject.transform);
            newLongObstacle.transform.position = boat.transform.position + Vector3.forward * 256;
            newLongObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));

            int randomSide = Random.Range(0, 2);
            switch (randomSide)
            {
                case 0:
                    newLongObstacle.transform.position +=  Vector3.right * 50;
                    break;
                case 1:
                    newLongObstacle.transform.position += Vector3.left * 50;
                    break;
            }



            timerCurrent = 0;
        }

        goalDistance.text = "Goal: " + ((int)Vector3.Distance(boat.transform.position, goal.transform.position)).ToString() + "m";
    }

}


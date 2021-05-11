using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{

    public Boat boat;
    public GameObject wind;
    public GameObject goal;
    public Text goalDistance;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float timerMax;
    public float timerCurrent;
    public Vector3 CameraOffset;

    public float navPoints;

    public int PlayerMoney { get { return boat.playerMoney; } }

    // Start is called before the first frame update
    void Start()
    {
        timerMax = 4;
        boat = GameObject.Find("Boat").GetComponent<Boat>();
        wind = GameObject.Find("Wind");
        goal = GameObject.Find("Goal");

        audioClip = Resources.Load<AudioClip>("AudioClips/Ocean");
        timerCurrent = 0;
        this.CameraOffset = Camera.main.transform.position - this.transform.position;

        goalDistance = GameObject.Find("GoalDistance").GetComponent<Text>();


        audioSource = boat.gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();

        navPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        navPoints = boat.navPoints;


        //instantiate decoratives
        timerCurrent += Time.deltaTime;
        if (timerCurrent >= timerMax)
        {
            // Underwaters;
            GameObject newDecotatives = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Decoratives"), this.gameObject.transform);
            newDecotatives.transform.position = boat.transform.position + Vector3.forward * 420;
            newDecotatives.transform.position += Vector3.down * 6.17f;

            GameObject newTrigger = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TriggerMaxSpeed"), this.gameObject.transform);
            newTrigger.transform.position = 
                        boat.transform.position
                        + Vector3.forward * 420
                        + Vector3.left * Random.Range(50, -50);
            newTrigger.transform.Rotate(Vector3.up, Random.Range(0, 180));

            GameObject newTrigger2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TriggerMaxSpeed"), this.gameObject.transform);
            newTrigger2.transform.position =
                        boat.transform.position
                        + Vector3.forward * 420
                        + Vector3.left * Random.Range(50, -50);
            newTrigger2.transform.Rotate(Vector3.up, Random.Range(0, 180));

            // middle obstacle island and loots
            GameObject newObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/IslandObstacle"), this.gameObject.transform);
            newObstacle.transform.position = boat.transform.position + Vector3.forward * 420;
            newObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));


            GameObject pickUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PickUp"), this.gameObject.transform);
            pickUp.transform.position = newObstacle.transform.position + Vector3.right * Random.Range(-20, 20);
            GameObject pickUp2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PickUp"), this.gameObject.transform);
            pickUp2.transform.position = newObstacle.transform.position + Vector3.right * Random.Range(-25, 25);

            //Big Island Module
            // middle obstacle island and loots
            bool isNewModule = (Random.Range(0, 20) >= 18) ? true : false;
            switch (isNewModule)
            {
                case true:
                    GameObject newModule = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BaseModule"), this.gameObject.transform);
                    newModule.transform.position =
                        boat.transform.position
                        + Vector3.forward * 420
                        + Vector3.left * Random.Range(80, -80);
                    newModule.transform.Rotate(Vector3.up, Random.Range(0, 180));
                    break;
            }

            //close side Long Object
            GameObject newLongObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/LongObstacle"), this.gameObject.transform);
            newLongObstacle.transform.position = 
                boat.transform.position 
                + Vector3.forward * 420 
                + Vector3.left* Random.Range(120,-120);
            newLongObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));

            //GameObject newSeagullPicKUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/SeagullsPickUp"), newLongObstacle.transform);
            //newSeagullPicKUp.transform.position += Vector3.left*2 + Vector3.down* 0.1f;

            bool isNewPassengers = (Random.Range(0, 10) >= 7) ? true : false;
            switch(isNewPassengers)
            {
                case true:
                GameObject passengerPickUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PassengerTransfert") , this.gameObject.transform);
                passengerPickUp.transform.position = newLongObstacle.transform.position + Vector3.forward * Random.Range(-0.2f, 0.2f);
                break;
            }

            //far side giant Object
            // to limit player with semi boundaries;

            int randomSide = Random.Range(0, 2);
            switch (randomSide)
            {
                case 0:
                    newLongObstacle.transform.position +=  Vector3.right * 40;
                    break;
                case 1:
                    newLongObstacle.transform.position += Vector3.left * 40;
                    break;
            }



            timerCurrent = 0;
        }

        goalDistance.text = "Goal: " + ((int)Vector3.Distance(boat.transform.position, goal.transform.position)).ToString() + "m";
    }

}


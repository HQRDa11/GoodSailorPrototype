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
    public Vector3 CameraOffset;
    public float navPoints;

    public int PlayerMoney { get { return boat.playerMoney; } }

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat").GetComponent<Boat>();
        wind = GameObject.Find("Wind");
        goal = GameObject.Find("Goal");

        audioClip = Resources.Load<AudioClip>("AudioClips/Ocean");

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
        goalDistance.text = "Goal: infinite";
    }

}


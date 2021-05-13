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

    public World m_world;
    public Game_UserInterface m_ui;

    public int PlayerMoney { get { return boat.playerMoney; } }

    public float m_sideMovement;

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

        m_world = new World(WorldState.MIDDLE_ISLANDS);
        m_ui = GameObject.Find("UI").GetComponent<Game_UserInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        navPoints = boat.navPoints;
        goalDistance.text = "Goal: undefined yet";

        m_world.Update();
        Update_SideMovement();

    }


    public void Update_SideMovement()
    { 
        //Debug.Log(boat.GetComponent<Rigidbody>().velocity.x) ;
        if (boat.GetComponentInChildren<Rigidbody>().velocity.x > 30)
        {
            m_sideMovement += Time.deltaTime;
        }
        else if (boat.GetComponentInChildren<Rigidbody>().velocity.x < - 30)
        {
            m_sideMovement -= Time.deltaTime;
        }
        if (m_sideMovement > 8)
        {
            NextWorld(false);
            m_sideMovement = - 8;
        }
        else if (m_sideMovement < -8)
        {
            NextWorld(true);
            m_sideMovement = 8;
        }
    }

    public void NextWorld( bool isWest_notEast)
    {
        Debug.Log("nextState");
        switch (isWest_notEast)
        {
            case true:
                m_world.NextState_West();
                return;
            case false:
                m_world.NextState_East();
                return;
        }
        m_ui.Display_AreaName(m_world.GetAreaName());
    }
}


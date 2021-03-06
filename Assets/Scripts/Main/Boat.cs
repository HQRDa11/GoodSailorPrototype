using UnityEngine;
using UnityEngine.UI;


public enum NavigationState { SAILING = 0, TRANSFERT, DOCKED } // The player journey state
public enum SailsState {  CLOSE = 0, MID_OPEN, FULL_OPEN} // The boat's sails state

public class Boat : MonoBehaviour
{
    // v
    public GameObject cameraPoint;

    public Text text;
    public float currentSpeed;
    private float maxSpeed;
    private float maxSpeedBonus;
    private Vector3 startingPosition, speedvec;

    public int playerMoney;

    GameObject wind;

    private Rigidbody m_rigidbody;
    float speed;
    float brakeForce;

    public SailsState sailState;

    public float comparison;
    public float rotSpeed;

    public float navPoints;

    public PlayerCamera m_playerCamera;

    public AudioSource boatSpeed_AudioSource;
    public AudioClip speed1_Audio;
    public AudioClip speed2_Audio;
    public AudioClip speed3_Audio;

    public PassengerCargo passengerCargo;

    public TrailRenderer satisfactionTrail;

    public NavigationState navigationState;

    float resistance;

    public GameObject dockedAt;

    void Start()
    {
        text = GameObject.Find("BoatSpeed Text").GetComponent<Text>();
        wind = GameObject.Find("Wind");
        startingPosition = transform.position;

        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody = GetComponent<Rigidbody>();
        rotSpeed = 28;

        speed = 140 ;
        maxSpeed = 50;
        maxSpeedBonus = 0;

        brakeForce = 10.2f;
        comparison = 0;

        boatSpeed_AudioSource = gameObject.GetComponent<AudioSource>();
        speed1_Audio = Resources.Load<AudioClip>("AudioClips/Speed1");
        speed2_Audio = Resources.Load<AudioClip>("AudioClips/Speed2");
        speed3_Audio = Resources.Load<AudioClip>("AudioClips/Speed3");

        passengerCargo = gameObject.GetComponentInChildren<PassengerCargo>();

        satisfactionTrail = GameObject.Find("SatisfactionTrail").GetComponent<TrailRenderer>();

        sailState = SailsState.CLOSE;

        navigationState =  NavigationState.SAILING;

        playerMoney = 0;
        resistance = 10 * Time.deltaTime;

        m_playerCamera = Camera.main.GetComponent<PlayerCamera>();
        cameraPoint = GameObject.Find("BoatCameraPoint");
    }

    void FixedUpdate()
    {

    }
    private void Update()
    {
        // SI LE JOUEUR EST A L ARRET RIEN LE BATEAU NE S UPDATE PAS
        switch(navigationState)
        {
            case NavigationState.DOCKED:
                if (m_playerCamera.state != CameraState.ISLANDfocus)
                {
                    m_playerCamera.request(CameraState.ISLANDfocus, dockedAt);
                }
                this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                
                
                switch ( Input.GetKeyDown(KeyCode.Space) )
                {
                    case true:
                        navigationState = NavigationState.SAILING;
                        m_playerCamera.request(CameraState.BOATfocus, this.gameObject);
                        break;

                }
                return;

            case NavigationState.TRANSFERT:
                if (m_playerCamera.state != CameraState.TRANSFERTfocus)
                {
                    m_playerCamera.request(CameraState.TRANSFERTfocus, this.gameObject);
                }
                this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                switch (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    case true:
                        navigationState = NavigationState.SAILING;
                        m_playerCamera.request(CameraState.BOATfocus, this.gameObject);
                        break;

                }
                return;
        }
        ///////////////////////////////////////////////////////////
        startingPosition = transform.position;
        SailsState sailState = Update_SailState();

        //Debug.Log("Sails: " + sailState);
        int sailSpeedBonus = (sailState == SailsState.CLOSE) ? 0 : (sailState == SailsState.FULL_OPEN) ? 2 : 1;
        Update_HorizontalMoves(sailSpeedBonus);
        Update_Acceleration(sailSpeedBonus);

        currentSpeed = m_rigidbody.velocity.magnitude;
        text.text = (int)currentSpeed/2 + " mph.";  // or mph

        // Debug.Log("speed = " + m_rigidbody.velocity);

        // Cap velocity:
        m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, maxSpeed + maxSpeedBonus);
        if (maxSpeedBonus > 0 ) { maxSpeedBonus -= 1 * Time.deltaTime; }


        m_rigidbody.AddForce(-Vector3.Project(m_rigidbody.velocity, transform.right) * resistance);

        // Floating : does it actually works?
        if (gameObject.transform.position.y < 0)
        {
            gameObject.transform.position += Vector3.up * Time.deltaTime;
        }

        //Add navPoints:
        if (sailState == SailsState.FULL_OPEN)
        {
            navPoints += currentSpeed / 10 * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                navPoints += currentSpeed / 30 * Time.deltaTime;
            }
        }
        //Add satisfaction to passengers
        passengerCargo.OnPassengerBonus((currentSpeed - 12) * Time.deltaTime /10);

        UpdateSound();

        if (currentSpeed >= 38)
        {
            satisfactionTrail.startWidth = (currentSpeed-38)/12*Mathf.Sqrt(currentSpeed/10);
            satisfactionTrail.endWidth = 0.01f;
            satisfactionTrail.emitting = true;

        }
        else satisfactionTrail.emitting = false;

    }

    private float compare(Quaternion quatA, Quaternion quatB)
    {
        return Quaternion.Angle(quatA, quatB);
    }

    public SailsState Update_SailState()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            sailState = SailsState.FULL_OPEN;
            return sailState;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            sailState = SailsState.CLOSE;
            return sailState;
        }
        sailState = SailsState.MID_OPEN;
        return sailState;
    }
    public void Update_HorizontalMoves( int sailSpeedBonus)
    {
        // read user inputs
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.gameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
            // GetComponent<Rigidbody>().velocity += Vector3.right * Time.deltaTime ;
            Brakes(1);
            if (transform.rotation.z < +20)
            {
                // this.gameObject.transform.Rotate(Vector3.right * Time.deltaTime * rotSpeed);
                this.m_rigidbody.AddForce(Vector3.right * Time.deltaTime * sailSpeedBonus);
            }

        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.gameObject.transform.Rotate(Vector3.down * Time.deltaTime * rotSpeed);
            // GetComponent<Rigidbody>().velocity += Vector3.left * Time.deltaTime;
            Brakes(1);
            if (transform.rotation.z > -20)
            {
                //this.gameObject.transform.Rotate(Vector3.left* Time.deltaTime * rotSpeed);
                this.m_rigidbody.AddForce(Vector3.left * Time.deltaTime * sailSpeedBonus);
            }
        }
    }
    public void Update_Acceleration(int sailSpeedBonus)
    {
        // Acceleration
        // Check wind / boat angle
        comparison = compare(transform.rotation, wind.transform.rotation);
        if (comparison < 50)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.90f * sailSpeedBonus);
            wind.GetComponent<Wind>().Scale = 0.90f;
            //Debug.Log("x1 speed");
        }

        else if (comparison < 100)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.65f * sailSpeedBonus);
            wind.GetComponent<Wind>().Scale = 0.60f;
            //Debug.Log("x0.7 speed");
            Cloth cloth = GameObject.Find("Voile").GetComponent<Cloth>();
            cloth.externalAcceleration = Vector3.forward * wind.transform.rotation.y;
        }

        else if (comparison < 167)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.80f * sailSpeedBonus);
            wind.GetComponent<Wind>().Scale = 0.80f;
            //Debug.Log("x0.8 speed");
        }
        else
        {
            wind.GetComponent<Wind>().Scale = 0.5f;
            //Braking
            Brakes(3);
        }
        if (sailSpeedBonus == 0 ) Brakes(3); // if player is braking
    }
    private void Brakes(int additionalBrakeForce)
    {
        this.m_rigidbody.AddForce(-m_rigidbody.velocity * this.brakeForce * Time.deltaTime * additionalBrakeForce);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PickUp" )
        {
            other.gameObject.GetComponent<Pickup>().OnPickUp();
            passengerCargo.OnPassengerBonus(15);
            int diceRescue = Random.Range(0, 3);
            switch (diceRescue) { case 0: passengerCargo.AddPassenger(); break; }   
        }
        if (other.transform.tag == "PassengerPickUp" && currentSpeed < 20 && sailState != SailsState.FULL_OPEN)
        {
            dockedAt = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            other.gameObject.GetComponent<PassengerPickUp>().OnPickUp();
            //Debug.Log("here will come some passengers");
            navigationState = NavigationState.DOCKED;
            passengerCargo.OnPassengerTransfer(other.gameObject.GetComponent<PassengerPickUp>().getEmbarkPoint());
        }
        if (other.transform.tag == "IslandEntry" && currentSpeed < 20 && sailState != SailsState.FULL_OPEN)
        {
            other.gameObject.GetComponent<IslandEntry>().OnPickUp();
            //Debug.Log("here will come some passengers");
            navigationState = NavigationState.DOCKED;
            dockedAt = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            Debug.Log("parent parent :"+ other.gameObject.transform.parent.gameObject.transform.parent.gameObject);
        }
        if (other.transform.tag == "TriggerMaxSpeed")
        {
            Debug.Log("maxSpeed");
            this.maxSpeedBonus = 8;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            float loss = -currentSpeed ;
            Debug.Log("Passengers loses satisfaction: " + loss.ToString());
            passengerCargo.OnPassengerBonus(loss);
        }
    }
    private void UpdateSound()
    {
        int speed = (currentSpeed < 16) ? 1 : (currentSpeed < 32) ? 2 : 3;
        switch (speed)
        {
            case 1:
                if (boatSpeed_AudioSource.clip != speed1_Audio)
                {
                    boatSpeed_AudioSource.clip = speed1_Audio;
                    boatSpeed_AudioSource.Play();
                }
                break;
            case 2:
                if (boatSpeed_AudioSource.clip != speed2_Audio)
                {
                    boatSpeed_AudioSource.clip = speed2_Audio;
                    boatSpeed_AudioSource.Play();
                }
                break;
            case 3:
                if (boatSpeed_AudioSource.clip != speed3_Audio)
                {
                    boatSpeed_AudioSource.clip = speed3_Audio;
                    boatSpeed_AudioSource.Play();
                }
                break;
          
        }
    }
}
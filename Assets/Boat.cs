using UnityEngine;
using UnityEngine.UI;


public enum NavigationState { SAILING = 0, DOCKED } // The player journey state
public enum SailsState {  CLOSE = 0, MID_OPEN, FULL_OPEN} // The boat's sails state

public class Boat : MonoBehaviour
{
   // v
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


    public AudioSource boatSpeed_AudioSource;
    public AudioClip speed1_Audio;
    public AudioClip speed2_Audio;
    public AudioClip speed3_Audio;

    public PassengerCargo passengerCargo;

    public TrailRenderer satisfactionTrail;

    public NavigationState navigationState;

    void Start()
    {
        text = GameObject.Find("BoatSpeed Text").GetComponent<Text>();
        wind = GameObject.Find("Wind");
        startingPosition = transform.position;

        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody = GetComponent<Rigidbody>();
        rotSpeed = 28;

        speed = 130 ;
        maxSpeed = 46;
        maxSpeedBonus = 0;

        brakeForce = 8.2f;
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
    }

    void FixedUpdate()
    {

    }
    private void Update()
    {
        startingPosition = transform.position;
        SailsState sailState = Update_SailState();

        //Debug.Log("Sails: " + sailState);
        int sailSpeedBonus = (sailState == SailsState.CLOSE) ? 0 : (sailState == SailsState.FULL_OPEN) ? 2 : 1;
        Update_HorizontalMoves(sailSpeedBonus);
        Update_Acceleration(sailSpeedBonus);

        currentSpeed = m_rigidbody.velocity.magnitude;
        text.text = (int)currentSpeed + "km/h";  // or mph

        // Debug.Log("speed = " + m_rigidbody.velocity);

        // Cap velocity:
        float resistance = 0.5f;
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
                navPoints += currentSpeed / 12 * Time.deltaTime;
            }
        }
        //Add satisfaction to passengers
        passengerCargo.OnPassengerBonus(currentSpeed * Time.deltaTime /5);

        UpdateSound();

        if (currentSpeed >= 36)
        {
            satisfactionTrail.startWidth = (currentSpeed-36)/4;
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
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.82f * sailSpeedBonus);
            //Debug.Log("x1 speed");
        }

        else if (comparison < 100)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.52f * sailSpeedBonus);
            
            //Debug.Log("x0.7 speed");
            Cloth cloth = GameObject.Find("Voile").GetComponent<Cloth>();
            cloth.externalAcceleration = Vector3.forward * wind.transform.rotation.y;
        }

        else if (comparison < 167)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.68f * sailSpeedBonus);
            //Debug.Log("x0.8 speed");
        }
        else
        {
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
            Debug.Log("here i need to put a timed bonus");

            int diceRescue = Random.Range(0, 3);
            switch (diceRescue) { case 0: passengerCargo.AddPassenger(); break; }   
        }
        if (other.transform.tag == "PassengerPickUp" && currentSpeed < 16 && sailState != SailsState.FULL_OPEN)
        {
            other.gameObject.GetComponent<PassengerPickUp>().OnPickUp();
            Debug.Log("here will come some passengers");
            passengerCargo.OnPassengerTransfer(other.gameObject.GetComponent<PassengerPickUp>().getEmbarkPoint());
        }
        if (other.transform.tag == "TriggerMaxSpeed")
        {
            Debug.Log("maxSpeed");
            this.maxSpeedBonus = 8;
        }
        if (other.transform.tag == "Seagulls")
        {
            other.gameObject.GetComponent<SeagullPickUp>().OnPickUp();
            Debug.Log("Bonus happyness fot passengers");

        }
        if (other.transform.tag == "Obstacle")
        {
            Debug.Log("Passengers loses satisfaction");
            passengerCargo.OnPassengerBonus(-30);
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
    //function OnMouseDown()
    //{
    //    aDefault = stickReference.transform.localEulerAngles;
    //    aCurr = aDefault;
    //}

    //function OnMouseUp()
    //{
    //    //reset joystick to default Position
    //    transform.localEulerAngles = aDefault;
    //    aCurr = aDefault;
    //}

    //function OnMouseDrag()
    //{
    //    aDefault = stickReference.transform.localEulerAngles;
    //    ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);

    //    //ray.origin -- the mouse position in world space at the near plane of the camera
    //    //ray.direction -- the direction of the mouse in world Space
    //    transform.LookAt(ray.origin);
    //    aCurr = transform.localEulerAngles;
    //}

    //function FixedUpdate()
    //{
    //    //eulerAngles.y is related to accelleration; eulerAngles.x is related to direction
    //    ship.rigidbody.AddRelativeForce(Vector3.forward * Mathf.Clamp(Mathf.Repeat((aCurr.x - aDefault.x) + 180, 360) - 180, -aMax, aMax) * speedScale * -1);// need to change [0,360] to [-180,180]
    //    //stop drifting of the ship
    //    if (ship.rigidbody.velocity.magnitude >= speedMax)
    //    {
    //        ship.rigidbody.velocity = ship.rigidbody.velocity.normalized * (speedMax - 0.1);
    //    }
    //    ship.rigidbody.AddRelativeTorque(Vector3.up * Mathf.Clamp(aCurr.y - aDefault.y, -aMax, aMax) * aScale);
    //}

}
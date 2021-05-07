using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SailState {  CLOSE = 0, MID_OPEN, FULL_OPEN}
public class Boat : MonoBehaviour
{
   // v
    public Text text;
    private float currentSpeed;
    private float maxSpeed;
    private Vector3 startingPosition, speedvec;

    GameObject wind;

    private Leak leakLeftSpeed;
    private Leak leakRightSpeed;

    private Rigidbody m_rigidbody;
    float speed;
    float brakeForce;


    public float comparison;
    public float rotSpeed;          
    void Start()
    {
        text = GameObject.Find("BoatSpeed Text").GetComponent<Text>();
        wind = GameObject.Find("Wind");
        startingPosition = transform.position;

        m_rigidbody = GetComponent<Rigidbody>();
        rotSpeed = 30;

        speed = 120 ;
        maxSpeed = 40;


        brakeForce =7f;
        comparison = 0;

        leakLeftSpeed = GameObject.Find("LeakLeft").GetComponent<Leak>();
        leakRightSpeed = GameObject.Find("LeakRight").GetComponent<Leak>();
        if (!GameObject.Find("LeakRight").GetComponent<Leak>()) { Debug.LogError("ERROR HERE"); };

    }
    void FixedUpdate()
    {

    }
    private void Update()
    {
        startingPosition = transform.position;
        
        SailState sailState = Update_SailState();
       
        //Debug.Log("Sails: " + sailState);
        int sailSpeedBonus = (sailState == SailState.CLOSE) ? 0 : (sailState == SailState.FULL_OPEN) ? 2 : 1 ;
        Update_HorizontalMoves(sailSpeedBonus);
        Update_Acceleration(sailSpeedBonus);


        currentSpeed = m_rigidbody.velocity.magnitude;
        text.text = (int)currentSpeed + "km/h";  // or mph

        leakLeftSpeed.spawnSpeed = currentSpeed;
        leakRightSpeed.spawnSpeed = currentSpeed;

        Debug.Log("speed = " + m_rigidbody.velocity);

        // Cap velocity:
        float resistance = 0.5f;
        m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, maxSpeed);
        m_rigidbody.AddForce(-Vector3.Project(m_rigidbody.velocity, transform.right) * resistance);

        // Floating : does it actually works?
        if (gameObject.transform.position.y < 0)
        {
            gameObject.transform.position += Vector3.up * Time.deltaTime;
        }
    }
    private float compare(Quaternion quatA, Quaternion quatB)
    {
        return Quaternion.Angle(quatA, quatB);
    }
    private void Brakes( int sailSpeedBonus)
    {
        this.m_rigidbody.AddForce( -m_rigidbody.velocity* brakeForce * Time.deltaTime * sailSpeedBonus);
    }
    public SailState Update_SailState()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            return SailState.FULL_OPEN;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            return SailState.CLOSE;
        }
        return SailState.MID_OPEN;
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
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * sailSpeedBonus);
            //Debug.Log("x1 speed");
        }

        else if (comparison < 100)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.7f * sailSpeedBonus);
            
            //Debug.Log("x0.7 speed");
            Cloth cloth = GameObject.Find("Voile").GetComponent<Cloth>();
            cloth.externalAcceleration = Vector3.forward * wind.transform.rotation.y;
        }

        else if (comparison < 150)
        {
            this.m_rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.8f * sailSpeedBonus);
            //Debug.Log("x0.8 speed");
        }
        else
        {
            //Braking
            Brakes(3);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SailState {  CLOSE = 0, MID_OPEN, FULL_OPEN}
public class Boat : MonoBehaviour
{
   // v
    public Text text;
    private double speedDisplayed;
    private float maxSpeed;
    private Vector3 startingPosition, speedvec;

    GameObject wind;

    float boatBalancementZ;

    private Rigidbody rigidbody;
    float speed;
    float brakeForce;


    public float comparison;
    public float rotSpeed;          
    void Start()
    {
        text = GameObject.Find("BoatSpeed Text").GetComponent<Text>();
        wind = GameObject.Find("Wind");
        startingPosition = transform.position;

        rigidbody = GetComponent<Rigidbody>();
        rotSpeed = 20;
        boatBalancementZ = transform.rotation.z;


        speed =70;
        maxSpeed = 20;


        brakeForce =5f;
        comparison = 0;

    }
    void FixedUpdate()
    {

    }
    private void Update()
    {
        startingPosition = transform.position;
        text.text = speedDisplayed + "km/h";  // or mph


        
        SailState sailState = Update_SailState();
       
        //Debug.Log("Sails: " + sailState);
        int sailSpeedBonus = (sailState == SailState.CLOSE) ? 0 : (sailState == SailState.FULL_OPEN) ? 2 : 1 ;
        Update_HorizontalMoves(sailSpeedBonus);
        Update_Acceleration(sailSpeedBonus);

        speedvec = ((transform.position - startingPosition));
        speedDisplayed = (speedvec.magnitude) * 1.6; // 3.6 is the constant to convert a value from m/s to km/h, because i think that the speed wich is being calculated here is coming in m/s, if you want it in mph, you should use ~2,2374 instead of 3.6 (assuming that 1 mph = 1.609 kmh)
        Debug.Log("speed = " + rigidbody.velocity);

        // Cap velocity:
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

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
        this.rigidbody.AddForce( -rigidbody.velocity* brakeForce * Time.deltaTime * sailSpeedBonus);
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
                this.rigidbody.AddForce(Vector3.right * Time.deltaTime * sailSpeedBonus);
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
                this.rigidbody.AddForce(Vector3.left * Time.deltaTime * sailSpeedBonus);
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
            this.rigidbody.AddForce( transform.forward * Time.deltaTime * speed * sailSpeedBonus);
            //Debug.Log("x1 speed");
        }

        else if (comparison < 100)
        {
            this.rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.7f * sailSpeedBonus);
            
            //Debug.Log("x0.7 speed");
            Cloth cloth = GameObject.Find("Voile").GetComponent<Cloth>();
            cloth.externalAcceleration = Vector3.forward * wind.transform.rotation.y;
        }

        else if (comparison < 150)
        {
            this.rigidbody.AddForce( transform.forward * Time.deltaTime * speed * 0.8f * sailSpeedBonus);
            //Debug.Log("x0.8 speed");
        }
        else
        {
            //Braking
            Brakes(3);
        }
    }
}
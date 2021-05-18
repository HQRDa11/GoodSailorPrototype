using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState { BOATfocus = 0, ISLANDfocus, TRANSFERTfocus}
public class PlayerCamera : MonoBehaviour
{
    public CameraState state;
    public GameObject boat;

    public GameObject m_target;

    Vector3 initialBoatOffset;
    //Quaternion initialRot;

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
        this.gameObject.transform.position = boat.GetComponent<Boat>().cameraPoint.transform.position;
        initialBoatOffset = this.gameObject.transform.position - boat.transform.position;
        request(CameraState.BOATfocus,boat);
        //initialRot = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case CameraState.BOATfocus:
                this.gameObject.transform.position = boat.transform.position + initialBoatOffset;
                Camera.main.transform.position += Vector3.up * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed) * 5;
                Camera.main.transform.position += Vector3.back * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed) * 3;
                return;

            case CameraState.ISLANDfocus:
                Camera.main.transform.LookAt(m_target.transform.position);
                Camera.main.transform.RotateAround(m_target.transform.position, Vector3.up, 5*Time.deltaTime);
                return;

            case CameraState.TRANSFERTfocus:
                Camera.main.transform.LookAt(m_target.transform.position);
                Camera.main.transform.RotateAround(m_target.transform.position, Vector3.up, 5 * Time.deltaTime);
                return;
        }
    }

    public void request(CameraState newState, GameObject target)
    {
        switch (newState)
        {
            case CameraState.BOATfocus:
                state = CameraState.BOATfocus;


                Vector3 newDirection = Vector3.forward + Vector3.down/2 ;

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                Camera.main.transform.transform.rotation = Quaternion.LookRotation(newDirection);


                
                Camera.main.transform.position = boat.transform.position + initialBoatOffset;

                return;

            case CameraState.ISLANDfocus:
                state = CameraState.ISLANDfocus;
                m_target = target;
                this.gameObject.transform.position = m_target.transform.position ;
                Camera.main.transform.position += Vector3.up * 50;
                Camera.main.transform.position += Vector3.back * 80;
                return;

            case CameraState.TRANSFERTfocus:
                state = CameraState.TRANSFERTfocus;
                m_target = target;
                this.gameObject.transform.position = m_target.transform.position;
                Camera.main.transform.position += Vector3.up * 12;
                Camera.main.transform.position += Vector3.back * 12;
                return;
        }
    }
}

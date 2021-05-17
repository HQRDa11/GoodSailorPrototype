using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState { BOATfocus = 0, ISLANDfocus}
public class PlayerCamera : MonoBehaviour
{
    public CameraState state;
    public GameObject boat;

    public GameObject m_target;

    Vector3 initialPos;
    //Quaternion initialRot;

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
        initialPos = this.gameObject.transform.position;
        state = CameraState.BOATfocus;
        //initialRot = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case CameraState.BOATfocus:
                Debug.Log("boatState");
                this.gameObject.transform.position = boat.transform.position + initialPos;
                Camera.main.transform.position += Vector3.up * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed) * 5;
                Camera.main.transform.position += Vector3.back * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed) * 3;
                return;

            case CameraState.ISLANDfocus:
                Camera.main.transform.LookAt(m_target.transform.position);
                Camera.main.transform.RotateAround(m_target.transform.position, Vector3.up, 5*Time.deltaTime);
                return;
        }
    }

    public void request(CameraState newState, GameObject target)
    {
        switch (newState)
        {
            case CameraState.BOATfocus:
                state = CameraState.BOATfocus;
                this.gameObject.transform.position = boat.transform.position + initialPos;
                Camera.main.transform.position += Vector3.up * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed) * 5;
                Camera.main.transform.position += Vector3.back * Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed) * 3;
                return;

            case CameraState.ISLANDfocus:
                state = CameraState.ISLANDfocus;
                m_target = target;
                this.gameObject.transform.position = m_target.transform.position + initialPos;
                Camera.main.transform.position += Vector3.up * 34;
                Camera.main.transform.position += Vector3.back * 55;
                return;
        }
    }
}

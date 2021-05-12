using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterTrail : MonoBehaviour
{
    public Boat boat;
    public float emissionFlowTimer;
    public float emissionFlowTimer_current;
    TrailRenderer[] trails;
    private void Start()
    {
        boat = GameObject.Find("Boat").GetComponent<Boat>();
        trails = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer t in trails)
        {
            t.startWidth = 0.05f;
            t.endWidth = 0.8f;
            emissionFlowTimer = 0;
            emissionFlowTimer_current = 0;

        }
    }

    private void Update()
    {
        emissionFlowTimer_current += Time.deltaTime;
        switch(emissionFlowTimer_current > emissionFlowTimer)
        {
            case true:
                switch (boat.sailState)
                {
                    case SailsState.CLOSE:
                        emissionFlowTimer = 0.3f;
                        foreach (TrailRenderer t in trails)
                        {
                            t.emitting = (t.emitting == true) ? false : true;
                        }
                        break;

                    case SailsState.MID_OPEN:
                        emissionFlowTimer = Random.Range(0.05f, 0.5f);
                        foreach (TrailRenderer t in trails)
                        {
                            t.emitting = (t.emitting == true)? false : true;
                        }
                        break;

                    case SailsState.FULL_OPEN:
                        emissionFlowTimer = 1f;
                        foreach (TrailRenderer t in trails)
                        {
                            t.emitting = true;
                        }
                        break;
                }
                emissionFlowTimer_current = 0;
                break;
        }

    }
}

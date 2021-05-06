using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float timerMax;
    public float timerCurrent;
    public GameObject boat;

    // Start is called before the first frame update
    void Start()
    {
        timerMax = 12;
        timerCurrent = 0;
        this.boat = GameObject.Find("Boat");
    }

    // Update is called once per frame
    void Update()
    {
        timerCurrent += Time.deltaTime;
        if(timerCurrent >= timerMax)
        {
            TimerOk();
            timerCurrent = 0;
        }
        this.gameObject.transform.position = boat.transform.position;
    }

    public void TimerOk()
    {
        this.transform.transform.rotation = this.transform.rotation * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.up);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float timerMax;
    public float timerCurrent;
    public GameObject boat;
    public Vector3 boatOffset;

    public Transform directionIcon;
    public Vector3 originalScale;
    public  float Scale { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        timerMax = 16;
        timerCurrent = 0;
        this.boat = GameObject.Find("Boat");
        boatOffset = this.transform.position - boat.transform.position;

        directionIcon = GameObject.Find("arrow").transform;
        originalScale = directionIcon.localScale;
        Scale = 1;
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

        float currentSpeed = Mathf.Sqrt(boat.GetComponent<Boat>().currentSpeed);
        this.transform.position = boat.transform.position + boatOffset + Vector3.up * 2 * currentSpeed + Vector3.back * currentSpeed ;
        this.directionIcon.localScale = originalScale * Scale;
    }

    public void TimerOk()
    {
        this.transform.transform.rotation = this.transform.rotation * Quaternion.AngleAxis(Random.Range(1, 360), Vector3.up);
    }
}

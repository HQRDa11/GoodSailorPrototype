using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    public GameObject target;
    public bool isFollowing;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Boat");
        isFollowing = true;
    }

    // Update is called once per frame
    void Update()
    {
        DoTheBird();
    }

    public void DoTheBird()
    {
        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position + Vector3.up + Vector3.left * 5);
        switch (isFollowing)
        {
            case true:
                if (distance > 1)
                {
                    this.transform.position = Vector3.MoveTowards(
                        this.transform.position, 
                        target.transform.position, 
                        Time.deltaTime * Mathf.Sqrt(distance)*3
                        );
                    this.transform.rotation = Quaternion.LookRotation(this.transform.position, target.transform.position);
                    return;
                }
                else
                {
                    isFollowing = false;
                    return;
                }

            case false:
                this.transform.position += (Vector3.up + Vector3.left) * Time.deltaTime;
                if (Vector3.Distance(gameObject.transform.position, target.transform.position) > 30)
                { GameObject.Destroy(this.gameObject); }
                return;
        }
    }
}

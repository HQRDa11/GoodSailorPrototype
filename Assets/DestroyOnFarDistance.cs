using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFarDistance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.localPosition, Camera.main.transform.localPosition);
        //Debug.Log(distance);
        if ( distance > 512)
        {
            GameObject.Destroy(this.gameObject);
            Debug.Log("destructed");
        }
    }

}

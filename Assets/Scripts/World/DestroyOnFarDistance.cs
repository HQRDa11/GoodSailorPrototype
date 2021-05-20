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
        float distance = Vector3.Distance(gameObject.transform.position, Camera.main.transform.localPosition);
        //Debug.Log(distance);
        if ( distance > 1048)
        {
            GameObject.Destroy(this.gameObject);

        }
    }

}

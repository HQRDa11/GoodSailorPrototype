using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGoBackToStartPosition : MonoBehaviour
{
    public Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = this.gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, origin) > 0.05f) 
        {
            this.gameObject.transform.position =
                gameObject.transform.parent.transform.position
                + Vector3.Lerp(gameObject.transform.position, origin, 0.1f)
                * Time.deltaTime /30 ;
        }
    }
}

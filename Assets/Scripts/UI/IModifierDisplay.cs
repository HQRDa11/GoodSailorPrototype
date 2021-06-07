using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IModifierDisplay : MonoBehaviour
{
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 1.8f ;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < 0) { GameObject.Destroy(this.gameObject); return; }
        lifeTime -= Time.deltaTime;
        this.gameObject.transform.position += Vector3.up* Mathf.Lerp(3,1,lifeTime) /3;
    }
}

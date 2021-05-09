using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGO : MonoBehaviour
{
    public bool lateralDirection;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255, Random.Range(10, 200));
    }
    public void setLateralDirection(bool dir)
    {
        lateralDirection = dir;
    }
    private void Update()
    {
        switch (lateralDirection)
        {
            case true:
                gameObject.transform.position += Vector3.left * Time.deltaTime * 2;
                Debug.Log("reded");
                break;
            case false:
                gameObject.transform.position += Vector3.right * Time.deltaTime * 2;
                break;
        }
    }
    
}

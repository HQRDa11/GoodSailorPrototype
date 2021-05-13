using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    public PassengerStatus status;
    public float minHeight;
    public float satisfaction;

    // Start is called before the first frame update
    void Start()
    {
        minHeight = 10;
        status = PassengerStatus.WHITE;
        satisfaction = 0;
        SetMaterial(Resources.Load<Material>("Materials/Status/WhiteStatus"));
    }
    private void Update()
    {
        // Si + Grand => retrecir a min
        switch (gameObject.transform.localScale.y > minHeight)
        {
            case true:
                this.transform.localScale -= Vector3.up * Time.deltaTime *30 ;
                break;
        }

    }
    public void SetMaterial(Material material)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers) {  r.material = material; };

        this.transform.localScale += Vector3.up *12 ;
    }

}

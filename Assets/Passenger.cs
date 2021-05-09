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
        minHeight = this.gameObject.transform.localScale.y;
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
                    Debug.Log("should shrink");
                this.transform.localScale -= Vector3.up * Time.deltaTime *18 ;
                break;
        }

    }
    public void SetMaterial(Material material)
    {
        Debug.Log(material);
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Debug.Log("passenger renderers Count:" + renderers.Length );
        foreach (Renderer r in renderers) {  r.material = material; };

        this.transform.localScale += Vector3.up *30 ;
        Debug.Log("Tell me more" );
    }

}

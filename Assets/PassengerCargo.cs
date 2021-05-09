using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerCargo : MonoBehaviour
{

    public List<GameObject> passengers;
    public GameObject passengerGo;
    // Start is called before the first frame update
    void Start()
    {
        passengers = new List<GameObject>();
        passengerGo = Resources.Load<GameObject>("Prefabs/Passenger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPassenger()
    {
        GameObject newPassenger = GameObject.Instantiate(passengerGo);
        newPassenger.transform.parent = this.transform;
        newPassenger.transform.position = this.transform.position + Vector3.right * Random.Range(-2.3f,2.3f);
        passengers.Add(newPassenger);
    }
    public void RemovePassenger()
    {
        GameObject removed = passengers[Random.Range(0, passengers.Count - 1)];
        GameObject.Destroy(removed);
        passengers.Remove(removed);
    }
}

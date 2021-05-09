using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassengerStatus { RED = 0, WHITE, GREEN }
public class PassengerCargo : MonoBehaviour
{

    public List<Passenger> passengers;
    public GameObject passengerGo;

    public Material colorRed;
    public Material colorWhite;
    public Material colorGreen;

    // Start is called before the first frame update
    void Start()
    {
        passengers = new List<Passenger>();
        passengerGo = Resources.Load<GameObject>("Prefabs/Passenger");

        colorRed   = Resources.Load<Material>("Materials/Status/RedStatus");
        colorWhite = Resources.Load<Material>("Materials/Status/WhiteStatus");
        colorGreen = Resources.Load<Material>("Materials/Status/GreenStatus");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPassenger()
    {
        Debug.Log("1");
        GameObject newPassenger = GameObject.Instantiate(passengerGo);
        newPassenger.transform.parent = this.transform;
        Debug.Log("2");
        newPassenger.transform.position = this.transform.position + Vector3.right * Random.Range(-2.3f,2.3f);
        passengers.Add(newPassenger.GetComponent<Passenger>());
        Debug.Log("1");
    }
    public void RemovePassenger(Passenger removed)
    {
        Debug.Log("3");
        GameObject.Destroy(removed.gameObject);
        Debug.Log("4");
        passengers.Remove(removed);
    }

    public void OnPassengerTransfer(int newPassengers)
    {
        Debug.Log("5");
        List<Passenger> removePool = new List<Passenger>();
        for (int i=0; i<newPassengers; i++)
        {
            Debug.Log("6");
            AddPassenger();
        }
        foreach (Passenger passenger in passengers)
        {
            switch (passenger.status == PassengerStatus.GREEN)
            {

                case true: removePool.Add(passenger); break;
            }
            Debug.Log("7");
        }
        //while (removePool.Count != 0) { Debug.Log("ERROR"); RemovePassenger(removePool[0]); }
        for (int i =0; i < removePool.Count ;i++)
        {
            RemovePassenger(removePool[i]);
        }
    }
    public void OnPassengerBonus(float bonus)
    {
        bonus /= passengers.Count +1;
        
        foreach (Passenger passenger in passengers)
        {
            passenger.satisfaction += bonus;
            switch (passenger.status)
            {
                case PassengerStatus.RED:
                    switch(passenger.satisfaction>0)
                    {
                        case true: passenger.status = PassengerStatus.WHITE;
                            Debug.Log("7");
                            passenger.SetMaterial(colorWhite);
                            break;
                    }
                    break;

                case PassengerStatus.WHITE:
                    switch (passenger.satisfaction > 30)
                    {
                        case true:
                            Debug.Log("10");
                            passenger.status = PassengerStatus.GREEN;
                            passenger.SetMaterial(colorGreen);
                            break;
                    }
                    switch (passenger.satisfaction < -5)
                    {
                        case true:
                            Debug.Log("11");
                            passenger.status = PassengerStatus.RED;
                            passenger.SetMaterial(colorRed);
                            break;
                    }
                    break;

                case PassengerStatus.GREEN:
                    switch (passenger.satisfaction < 20 )
                    {
                        case true:
                            Debug.Log("12");
                            passenger.status = PassengerStatus.WHITE;
                            passenger.SetMaterial(colorWhite);
                            break;
                    }
                    break;
            }

        }
    }

}

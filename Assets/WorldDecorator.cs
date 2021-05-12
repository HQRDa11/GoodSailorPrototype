using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDecorator : MonoBehaviour
{
    public float waterLevelAdjustment;
    public Boat boat;
    public float timerMax;
    public float timerCurrent;

    public List<GameObject> decors;
    public int maxNbDecor;

    // Start is called before the first frame update
    void Start()
    {
        timerMax = 10;
        boat = GameObject.Find("Boat").GetComponent<Boat>();

        timerCurrent = 0;

        waterLevelAdjustment = 6;

        List<GameObject> decors = new List<GameObject>();
        maxNbDecor = 400;
    }
    public void DestroyFarDistance()
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject go in decors)
        {
            switch ( go == null || Vector3.Distance(go.transform.position, boat.transform.position) >= 512)
            {
                case true:
                    toRemove.Add(go);
                    break;
            }
        }
        foreach (GameObject go in toRemove)
        {
            decors.Remove(go);
            GameObject.Destroy(go);
        }
    }
    void Update()
    {
        DestroyFarDistance();
        timerCurrent += Time.deltaTime * boat.GetComponent<Boat>().currentSpeed/10;
        switch (timerCurrent >= timerMax)
        {
            case true:
                switch (decors.Count < maxNbDecor)
                {
                    case true:
                        DecorNewPassengerTransfert();
                        DecorUnderwater();
                        DecorSmallIslandsAndLoots();
                        DecorBigIslandsModules();
                        DecorWithGiantLongSideObjects();
                        break;
                }
                timerCurrent = 0;
                break;
        }




    }

    public void DecorUnderwater()
    {
        // Underwaters;
        GameObject newDecotatives = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Decoratives"), this.gameObject.transform);
        newDecotatives.transform.position = boat.transform.position + Vector3.forward * 420;
        newDecotatives.transform.position += Vector3.down * 6.17f;

        decors.Add(newDecotatives);

        GameObject newTrigger = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TriggerMaxSpeed"), this.gameObject.transform);
        newTrigger.transform.position =
                    boat.transform.position  // VOIR EN DESSOUS
                    + Vector3.forward * 420
                    + Vector3.left * Random.Range(50, -50);
        newTrigger.transform.Rotate(Vector3.up, Random.Range(0, 180));
        decors.Add(newTrigger);

    }

    public void DecorSmallIslandsAndLoots()
    {
        GameObject newObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/IslandObstacle"), this.gameObject.transform);
        newObstacle.transform.position = boat.transform.position + Vector3.forward * 420;
        newObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));
        decors.Add(newObstacle);

        GameObject pickUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PickUp"), this.gameObject.transform);
        pickUp.transform.position = newObstacle.transform.position + Vector3.right * Random.Range(-20, 20);
        decors.Add(pickUp);
        GameObject pickUp2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PickUp"), this.gameObject.transform);
        pickUp2.transform.position = newObstacle.transform.position + Vector3.right * Random.Range(-25, 25);
        decors.Add(pickUp2);
    }

    public void DecorBigIslandsModules()
    {
        //Big Island Module
        // middle obstacle island and loots
        bool isNewIsland = (Random.Range(0, 20) >= 16) ? true : false;
        switch (isNewIsland)
        {
            case true:
                int randomIsland = Random.Range(0, 2);
                switch (randomIsland)
                {
                    case 0:
                        GameObject newIsland = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BaseModule"), this.gameObject.transform);
                        newIsland.transform.position =
                            boat.transform.position
                            + Vector3.forward * 420
                            + Vector3.left * Random.Range(80, -80);
                        newIsland.transform.position += Vector3.up * waterLevelAdjustment;
                        newIsland.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        decors.Add(newIsland);
                        break;

                    case 1:
                        GameObject newIsland2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Island2"), this.gameObject.transform);
                        newIsland2.transform.position =
                            boat.transform.position
                            + Vector3.forward * 420
                            + Vector3.left * Random.Range(80, -80);
                        newIsland2.transform.position += Vector3.up * waterLevelAdjustment;
                        newIsland2.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        decors.Add(newIsland2);
                        break;
                }
                break;

        }
    }
    public void DecorNewPassengerTransfert()
    {
        //GameObject newSeagullPicKUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/SeagullsPickUp"), newLongObstacle.transform);
        //newSeagullPicKUp.transform.position += Vector3.left*2 + Vector3.down* 0.1f;

        bool isNewPassengers = (Random.Range(0, 12) >= 10) ? true : false;
        switch (isNewPassengers)
        {
            case true:
                GameObject passengerPickUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PassengerTransfert"), this.gameObject.transform);
                passengerPickUp.transform.position = boat.transform.position
                            + Vector3.forward * 420
                            + Vector3.left * Random.Range(80, -80);
                decors.Add(passengerPickUp); 
                break;
        }
    }
    public void DecorWithGiantLongSideObjects()
    {

        //close side Long Object
        GameObject newLongObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/LongObstacle"), this.gameObject.transform);
        newLongObstacle.transform.position =
            boat.transform.position
            + Vector3.forward * 420;
        newLongObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));
        decors.Add(newLongObstacle);

        int randomSide = Random.Range(0, 2);
        switch (randomSide)
        {
            case 0:
                newLongObstacle.transform.position += Vector3.right * 50;
                break;
            case 1:
                newLongObstacle.transform.position += Vector3.left * 50;
                break;
        }
    }
    // Update is called once per frame
}

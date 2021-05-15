using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldDecorator<T>
{
    public void Update();
    public void SetDecorList(T listOfDecoratives);
    public T GetDecorList();
}

public class WorldDecorator
{
    public float waterLevelAdjustment;
    public Boat boat;
    public float m_timerMax;
    public float m_timerCurrent;

    protected bool m_isEntryLoaded;


    public int maxNbDecor;

    protected List<GameObject> m_decors;
    protected GameObject m_go;

    public WorldDecorator()
    {
        m_go = new GameObject("WorldDecorator");
        m_go.transform.position = Vector3.zero;
        m_timerMax = 10;
        boat = GameObject.Find("Boat").GetComponent<Boat>();

        m_timerCurrent = 0;

        waterLevelAdjustment = 6;

        m_decors = new List<GameObject>();
        maxNbDecor = 400;

        m_isEntryLoaded = false;
    }
    public void DestroyFarDistanceObjects()
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject go in m_decors)
        {
            switch (go == null || Vector3.Distance(go.transform.position, boat.transform.position) >= 512)
            {
                case true:
                    toRemove.Add(go);
                    break;
            }
        }
        foreach (GameObject go in toRemove)
        {
            m_decors.Remove(go);
            GameObject.Destroy(go);
        }
    }
}
public class WorldDecorator_MiddleIslands : WorldDecorator, IWorldDecorator<List<GameObject>>
{
    public WorldDecorator_MiddleIslands() : base()
    {
        
    }
    public void Update()
    {
        //Debug.Log("you are now in Middle");

        DestroyFarDistanceObjects();
        m_timerCurrent += Time.deltaTime * boat.GetComponent<Boat>().currentSpeed / 10;
        switch (m_timerCurrent >= m_timerMax)
        {
            case true:
                switch (m_decors.Count < maxNbDecor)
                {
                    case true:
                        DecorNewPassengerTransfert();
                        DecorUnderwater();
                        DecorSmallIslandsAndLoots();
                        DecorBigIslandsModules();
                        DecorWithGiantLongSideObjects();
                        break;
                }
                m_timerCurrent = 0;
                break;
        }
    }
    public List<GameObject> GetDecorList()
    {
        return m_decors;
    }    
    public void SetDecorList(List<GameObject> decors)
    {
        m_decors = decors;
    }
    public void DecorUnderwater()
    {
        // Underwaters;
        GameObject newDecotatives = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Decoratives"), this.m_go.transform);
        newDecotatives.transform.position = boat.transform.position + Vector3.forward * 420;
        newDecotatives.transform.position += Vector3.down * 6.17f;

        m_decors.Add(newDecotatives);

        GameObject newTrigger = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TriggerMaxSpeed"), this.m_go.transform);
        newTrigger.transform.position =
                    boat.transform.position  // VOIR EN DESSOUS
                    + Vector3.forward * 420
                    + Vector3.left * Random.Range(50, -50);
        newTrigger.transform.Rotate(Vector3.up, Random.Range(0, 180));
        m_decors.Add(newTrigger);

    }
    public void DecorSmallIslandsAndLoots()
    {
        GameObject newObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/IslandObstacle"), this.m_go.transform);
        newObstacle.transform.position = boat.transform.position + Vector3.forward * 420;
        newObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));
        m_decors.Add(newObstacle);

        GameObject pickUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PickUp"), this.m_go.transform);
        pickUp.transform.position = newObstacle.transform.position + Vector3.right * Random.Range(-20, 20);
        m_decors.Add(pickUp);
        GameObject pickUp2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PickUp"), this.m_go.transform);
        pickUp2.transform.position = newObstacle.transform.position + Vector3.right * Random.Range(-25, 25);
        m_decors.Add(pickUp2);
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
                        GameObject newIsland = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BaseModule"), this.m_go.transform);
                        newIsland.transform.position =
                            boat.transform.position
                            + Vector3.forward * 420
                            + Vector3.left * Random.Range(80, -80);
                        newIsland.transform.position += Vector3.up * waterLevelAdjustment;
                        newIsland.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        m_decors.Add(newIsland);
                        break;

                    case 1:
                        GameObject newIsland2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Island2"), this.m_go.transform);
                        newIsland2.transform.position =
                            boat.transform.position
                            + Vector3.forward * 420
                            + Vector3.left * Random.Range(80, -80);
                        newIsland2.transform.position += Vector3.up * waterLevelAdjustment;
                        newIsland2.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        m_decors.Add(newIsland2);
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
                GameObject passengerPickUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Stops/DockA"), this.m_go.transform);
                passengerPickUp.transform.position = boat.transform.position
                            + Vector3.forward * 420
                            + Vector3.left * Random.Range(80, -80);
                m_decors.Add(passengerPickUp);
                break;
        }
    }
    public void DecorWithGiantLongSideObjects()
    {

        //close side Long Object
        GameObject newLongObstacle = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/LongObstacle"), this.m_go.transform);
        newLongObstacle.transform.position =
            boat.transform.position
            + Vector3.forward * 420;
        newLongObstacle.transform.Rotate(Vector3.up, Random.Range(0, 180));
        m_decors.Add(newLongObstacle);

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
public class WorldDecorator_Ocean : WorldDecorator, IWorldDecorator<List<GameObject>>
{
    public WorldDecorator_Ocean() : base()
    {

    }
    public void Update()
    {
        DestroyFarDistanceObjects();
        m_timerCurrent += Time.deltaTime * boat.GetComponent<Boat>().currentSpeed / 10;
        switch (m_timerCurrent >= m_timerMax)
        {
            case true:
                switch (m_decors.Count < maxNbDecor)
                {
                    case true:
                        break;
                }
                m_timerCurrent = 0;
                break;
        }
    }
    public List<GameObject>GetDecorList()
    {
        return m_decors;
    }
    public void SetDecorList(List<GameObject> decors)
    {
        m_decors = decors;
    }
}

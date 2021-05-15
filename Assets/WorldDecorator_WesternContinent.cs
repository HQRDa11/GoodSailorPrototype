using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDecorator_WesternContinent : WorldDecorator, IWorldDecorator<List<GameObject>>
{
    public Continent_Factory m_continent_factory;
    public WorldDecorator_WesternContinent() : base()
    {
        m_continent_factory = new Continent_Factory();

        maxNbDecor = 200;
        m_timerMax = 36;
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
                        GameObject newContinent = m_continent_factory.Create_Continent();
                        newContinent.transform.position =
                            boat.transform.position
                            + Vector3.forward * 360
                            + Vector3.left * Random.Range(80, -80);
                        //newContinent.transform.position += Vector3.up * waterLevelAdjustment;
                        newContinent.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        m_decors.Add(newContinent);
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
}


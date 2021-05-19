using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDecorator_WestPeninsula : WorldDecorator, IWorldDecorator<List<GameObject>>
{
    public Island_Factory m_island_factory;
    public WorldDecorator_WestPeninsula() : base()
    {
        m_island_factory = new WestPeninsula_Island_Factory();

        maxNbDecor = 200;
        m_timerMax = 40;

        //Load_AreaEntry();

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
                        GameObject newContinent = m_island_factory.CreateIsland(Random.Range(3,200));
                        newContinent.transform.position =
                            boat.transform.position
                            + Vector3.forward * 720
                            + Vector3.right * Random.Range(80, -80);
                        //newContinent.transform.position += Vector3.up * waterLevelAdjustment;
                        newContinent.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        m_decors.Add(newContinent);
                        break;
                }
                m_timerCurrent = 0;
                break;
        }
    }
    public List<GameObject> GetTotalModulesList()
    {
        return m_decors;
    }
    public void SetTotalModulesList(List<GameObject> decors)
    {
        m_decors = decors;
    }

    public void Load_AreaEntry()
    {
        GameObject areaEntry = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Continents/EasternContinents/EasternContinents Entry"));
        areaEntry.transform.position =
            boat.transform.position
            + Vector3.forward * 180
            + Vector3.right * 150;
        m_decors.Add(areaEntry);
    }
}

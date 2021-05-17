using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_Factory 
{
    protected double m_oceanLevel;
    protected int m_idDistrib;
    protected GameObject m_baseA;
    protected GameObject m_blockA;

    protected GameObject m_dock;

    protected List<IslandModule> m_modules;
    protected List<Collider> m_colliders;
    protected List<GameObject> m_modulePrefabs;
    protected List<GameObject> m_beachPrefabs;
    protected GameObject  m_island;
    public Island_Factory()
    {
        m_oceanLevel = -2.5f;
        m_idDistrib = 0;

        m_baseA = Resources.Load<GameObject>("Prefabs/Island/BaseA");

        

        m_beachPrefabs = new List<GameObject>();
        m_beachPrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/BeachA"));
        m_beachPrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/BeachB"));

        m_modulePrefabs = new List<GameObject>();
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleA"));
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleB"));
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleC"));
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleD"));
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleE"));
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleF"));
        


        m_dock = Resources.Load<GameObject>("Prefabs/Stops/DockC");

        m_modules = new List<IslandModule>();
    }
    public GameObject CreateIsland(int level)
    {
        m_modules = new List<IslandModule>();
        m_idDistrib++;
        m_island  = Instantiate_IslandGameObject(level);
        m_island.transform.position = GameObject.Find("Boat").transform.position + Vector3.forward*320;

        ClearModuleList();
        GameObject baseModule = CreateBaseModule(m_island.GetComponent<Island>());

        for (int i = 0; i < level / 3; i++)
        {
            ClearModuleList();
            CreateBeachModule(m_island.GetComponent<Island>());
        }
        for (int i = 0; i < level; i++)
        {
            ClearModuleList();
            CreateModule(m_island.GetComponent<Island>());
        }

        List_AllColliders();
        GameObject newDock = CreateDock();
        int destroyed = 0;
        foreach (Collider collider in m_colliders)
        {
                switch(newDock.GetComponentInChildren<SphereCollider>().bounds.Intersects(collider.bounds))
                { case true: GameObject.Destroy(collider.gameObject); destroyed++; break; }
        }
        Debug.Log("modules:" + m_modules.Count);
       
        m_island.transform.Rotate(Vector3.up,Random.Range(0, 360));
        m_island.GetComponent<Island>().modules = m_modules;
        TranslateToWaterLevel(m_island);
        return m_island;
    }
    public GameObject Instantiate_IslandGameObject(int level)
    {
        GameObject islandGameObject = new GameObject("Island #" + m_idDistrib.ToString() +" lvl" + level.ToString(),
        typeof(Island),
        typeof(DestroyOnFarDistance));
        return islandGameObject;
    }
    public void TranslateToWaterLevel(GameObject obj)
    {
        obj.transform.position += Vector3.up * (float)m_oceanLevel;
    }
    public GameObject CreateBaseModule(Island parentIsland)
    {
        GameObject newBase = GameObject.Instantiate(m_baseA, parentIsland.transform, true);
        newBase.transform.localScale = Tools.RandomScale(34, 55, 3, 5f, 34, 55);
        newBase.transform.position = parentIsland.transform.position;
        newBase.GetComponent<IslandModule>().CreateClips();
        parentIsland.modules.Add(newBase.GetComponent<IslandModule>());
        return newBase;
    }

    public GameObject CreateBeachModule(Island parentIsland)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_beachPrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, parentIsland.transform, true);
        newModule.transform.position = Find_FreeClipPoint(parentIsland);
        newModule.transform.localScale = Tools.RandomScale(8, 13, 3, 5, 8, 13);
        newModule.GetComponent<IslandModule>().CreateClips();
        parentIsland.modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;
    }

    public GameObject CreateModule(Island island)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_modulePrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, island.transform);
        newModule.transform.position = Find_FreeClipPoint(island) + Vector3.back ;
        newModule.transform.localScale = Tools.RandomScale(8, 13, 5, 8, 8, 13);
        newModule.transform.Rotate(Vector3.up, Random.Range(0, 360));
        newModule.GetComponent<IslandModule>().CreateClips();
        m_modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;
    }
    public Vector3 FindRandom_FreeClipPoint()
    {
        foreach (IslandModule module in m_modules)
        {
            foreach (IClipPoint clip in module.ClipPoints())
            {
                int random = Random.Range(0, module.ClipPoints().Length);
                if (module.ClipPoints()[random].isFree == true)
                {
                    return clip.Clip();
                }
            }
        }
        return Vector3.zero;
    }

    public Vector3 Find_FreeClipPoint(Island island)
    {
        switch(island.modules.Count!=0)
        {
            case true:
                foreach (IslandModule module in island.modules)
                {
                    foreach (IClipPoint clip in module.ClipPoints())
                    {
                        if (clip.GetComponent<IClipPoint>().isFree == true)
                        {
                            return clip.Clip();
                        }
                    }
                }
                break;

        }

        return Vector3.zero;
    }
    public void List_AllColliders()
    {
        m_colliders = new List<Collider>();
        foreach (IslandModule module in m_modules)
        {
            foreach (Collider colider in module.gameObject.GetComponentsInChildren<Collider>())
            {
                m_colliders.Add(colider);
            }
        }
    }

    public GameObject CreateDock()
    {
        foreach (IslandModule module in m_modules)
        {
            foreach (IClipPoint clip in module.ClipPoints())
            {
                if (clip.GetComponent<IClipPoint>().isFree == true)
                {
                    GameObject newDock = new GameObject("dock" + m_idDistrib) ;
                    newDock = GameObject.Instantiate(m_dock, module.gameObject.transform, true);
                    newDock.transform.position = clip.Try();
                    newDock.transform.LookAt(m_island.gameObject.transform);
                    bool noCollisionOk= true;
                    foreach (Collider c in m_colliders)
                    {
                        if (newDock.GetComponentInChildren<PassengerPickUp>()
                                  .GetComponent<Collider>().
                                  bounds.Intersects(c.bounds)
                                  )
                        {
                            noCollisionOk = false;
                        }
                    }
                    switch (noCollisionOk)
                    {
                        case true: clip.Clip(); return newDock;
                    
                    }
                }
            }
        }
        Debug.LogWarning("no dock?");
        return new GameObject("dockError");
    }

    public void ClearModuleList()
    {
        List<IslandModule> toRemove = new List<IslandModule>();
        foreach (IslandModule module in m_modules)
        {
            switch (module.gameObject == null)
            {
                case true: toRemove.Add(module); break;
            }
        }
        foreach(IslandModule module in toRemove)
        {
            m_modules.Remove(module);
        }
    }

    public void LevelUp(Island island)
    {
        island.modules.Add(CreateModule(island).GetComponent<IslandModule>());
        Debug.Log("LevelUp!");


        //    IslandModule a = null;
        //    IslandModule b = null;
        //    List<Collider> colliders = new List<Collider>();
        //    foreach (IslandModule i in island.modules)
        //    {
        //        foreach(Collider c in i.GetAllColliders())
        //        {
        //            colliders.Add(i);
        //        }
        //    }
        //    for (int i = 0; i<island.modules.Count && b!=null;i++)
        //    {
        //        IslandModule tested = island.modules[Random.Range(0, island.modules.Count)];
        //        switch (tested.GetAllColliders()[0].bounds.Intersects(colliders[i].bounds))
        //        {
        //            case true:
        //                a = tested;
        //                b = colliders[i].gameObject.GetComponent<IslandModule>();
        //                break;
        //        }
        //    }
        //    switch (b!=null)
        //    {
        //        case true:
        //            MergeModuls(a, b);
        //            break;
        //    }
        //}

        //public GameObject
    }
}

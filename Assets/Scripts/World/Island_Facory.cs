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
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleG"));
        m_modulePrefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleH"));


        m_dock = Resources.Load<GameObject>("Prefabs/Stops/DockC");

        m_modules = new List<IslandModule>();
    }
    public GameObject CreateIsland(int level)
    {
        m_modules = new List<IslandModule>();
        m_idDistrib++;
        Island island = Instantiate_IslandGameObject(level).GetComponent<Island>(); //!!!
        island.transform.position = GameObject.Find("Boat").transform.position + Vector3.forward*320;

        ClearModuleList();
        GameObject baseModule = CreateBaseModule(island.GetComponent<Island>());

        for (int i = 0; i < level / 3; i++)
        {
            ClearModuleList();
            CreateBeachModule(island.GetComponent<Island>());
        }
        for (int i = 0; i < level; i++)
        {
            ClearModuleList();
            CreateModule(island.GetComponent<Island>());
        }

        List_AllColliders();
        GameObject newDock = CreateDock(island);
        int destroyed = 0;
        foreach (Collider collider in m_colliders)
        {
                switch(newDock.GetComponentInChildren<SphereCollider>().bounds.Intersects(collider.bounds))
                { case true: GameObject.Destroy(collider.gameObject); destroyed++; break; }
        }
        Debug.Log("modules:" + m_modules.Count);

        island.transform.Rotate(Vector3.up, Random.Range(0, 360));

        // rotate island to dowk face south
        switch (newDock.transform.position.z > island.transform.position.z)
        {
            case true:
                island.transform.Rotate(Vector3.up * 180);
                break;
        }
       
        island.Modules = m_modules;
        TranslateToWaterLevel(island.gameObject);
        return island.gameObject;
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

    public GameObject CreateBeachModule(Island parentIsland)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_beachPrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, parentIsland.transform, true);
        newModule.transform.position = Find_FreeClipPoint(parentIsland);
        newModule.transform.localScale = Tools.RandomScale(8, 13, 3, 5, 8, 13);
        newModule.GetComponent<IslandModule>().CreateClips();
        parentIsland.Modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;
    }

    public GameObject CreateModule(Island island)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_modulePrefabs.Count)];
        if (!island) { Debug.LogError("no island"); }
        GameObject newModule = GameObject.Instantiate(dicedPrefab, island.transform);
        newModule.transform.position = Find_FreeClipPoint(island); // System a refaire
        newModule.transform.localScale = Tools.RandomScale(8, 13, 5, 8, 8, 13);
        newModule.transform.Rotate(Vector3.up, Random.Range(0, 360));
        newModule.GetComponent<IslandModule>().CreateClips();
        newModule.GetComponent<IslandModule>().isMainModule = false;
        m_modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;
    }

    public GameObject CreateDock(Island island)
    {
        foreach (IslandModule module in m_modules)
        {
            foreach (IClipPoint clip in module.ClipPoints())
            {
                if (clip.GetComponent<IClipPoint>().isFree == true)
                {
                    GameObject newDock = new GameObject("dock" + m_idDistrib) ;
                    newDock = GameObject.Instantiate(m_dock, module.gameObject.transform, true);
                    if (!newDock.gameObject.GetComponent<IslandModule>()) { newDock.gameObject.AddComponent<IslandModule>(); }
                    newDock.GetComponent<IslandModule>().isMainModule = true;
                    newDock.transform.position = clip.Try();
                    newDock.transform.LookAt(island.gameObject.transform);
                    newDock.transform.parent = island.gameObject.transform;

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

    public void LevelUp(Island island)
    { 
        IslandModule newModule = CreateModule(island).GetComponent<IslandModule>();
        
        int randomPanel = island.Modules.Count;
        List<IslandModule> removedModules = new List<IslandModule>();
        int highestLevel = 1;
        for (int i = 0; i < randomPanel; i++)
        {
            Collider newCollider = newModule.GetCollider();
            Collider randomTest = island.Modules[Random.Range(0, island.Modules.Count)].GetCollider();

            switch (randomTest != null && newCollider != null && randomTest.gameObject.GetComponent<IslandModule>())
            {
                case true:
                    Debug.Log("Colliding: "+ randomTest.gameObject.name);
                    switch (randomTest.gameObject.GetComponent<IslandModule>().isMainModule == false 
                        && newCollider.bounds.Intersects(randomTest.bounds) 
                        && newModule.Level >= randomTest.gameObject.GetComponent<IslandModule>().Level)
                    {
                        case true:
                            IslandModule testmodule = randomTest.gameObject.GetComponent<IslandModule>();
                            highestLevel = (testmodule.Level > highestLevel)? testmodule.Level : highestLevel ;
                            removedModules.Add(testmodule);
                            break;
                    }
                    break;
            }

        }
        switch (removedModules.Count != 0)
        {
            case true:
                newModule.Level = highestLevel+1;
                newModule.transform.localScale = Vector3.one * Mathf.Sqrt(newModule.Level);

                while (removedModules.Count != 0)
                {
                    island.Modules.Remove(removedModules[0]);
                    GameObject.Destroy(removedModules[0].gameObject);
                    removedModules.RemoveAt(0);
                }

                break;
        }
        island.Modules.Add(newModule);
    }

    public GameObject CreateBaseModule(Island parentIsland)
    {
        GameObject newBase = GameObject.Instantiate(m_baseA, parentIsland.transform, true);
        newBase.transform.localScale = Tools.RandomScale(34, 55, 3, 5f, 34, 55);
        newBase.transform.position = parentIsland.transform.position;
        newBase.GetComponent<IslandModule>().CreateClips();
        newBase.GetComponent<IslandModule>().isMainModule = true;
        parentIsland.Modules.Add(newBase.GetComponent<IslandModule>());
        return newBase;
    }

    public Vector3 Find_FreeClipPoint(Island island)
    {
        switch (island.Modules.Count != 0)
        {
            case true:
                foreach (IslandModule module in island.Modules)
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
        foreach (IslandModule module in toRemove)
        {
            m_modules.Remove(module);
        }
    }
}

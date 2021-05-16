using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_Factory 
{
    protected int m_oceanLevel;
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
        m_oceanLevel = -3;
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

        m_dock = Resources.Load<GameObject>("Prefabs/Stops/DockB");
        m_modules = new List<IslandModule>();
    }
    public GameObject CreateIsland(int level)
    {
        m_modules = new List<IslandModule>();
        m_idDistrib++;
        m_island  = Instantiate_IslandGameObject();
        m_island.transform.position = GameObject.Find("Boat").transform.position + Vector3.forward*320;

        ClearModuleList();
        GameObject baseModule = CreateBaseModule(m_island.transform);

        for (int i = 0; i < level / 3; i++)
        {
            ClearModuleList();
            BeachModule(m_island.transform);
        }
        for (int i = 0; i < level; i++)
        {
            ClearModuleList();
            CreateModule(m_island.transform);
        }

        List_AllColliders();
        CreateDock();
        int destroyed = 0;
        foreach (Collider collider in m_colliders)
        {
                switch(m_dock.GetComponentInChildren<SphereCollider>().bounds.Intersects(collider.bounds))
                { case true: GameObject.Destroy(collider.gameObject); destroyed++; break; }
        }
        Debug.Log("modules:" + m_modules.Count);
        Debug.Log("destroyed:"+destroyed);
        
        TranslateToWaterLevel(m_island);
        m_island.transform.Rotate(Vector3.up,Random.Range(0, 360));
        m_island.GetComponent<Island>().modules = m_modules;
        
        return m_island;
    }
    public GameObject Instantiate_IslandGameObject()
    {
        GameObject islandGameObject = new GameObject("Island #" + m_idDistrib.ToString(),
        typeof(Island),
        typeof(DestroyOnFarDistance));
        
        return islandGameObject;
    }
    public void TranslateToWaterLevel(GameObject obj)
    {
        obj.transform.position += Vector3.up * m_oceanLevel;
    }
    public GameObject CreateBaseModule(Transform parent)
    {
        GameObject newBase = GameObject.Instantiate(m_baseA, parent, true);
        newBase.transform.localScale = Tools.RandomScale(34, 55, 3, 5f, 34, 55);
        newBase.transform.position = parent.transform.position;
        newBase.GetComponent<IslandModule>().CreateClips();
        m_modules.Add(newBase.GetComponent<IslandModule>());
        return newBase;
    }

    public GameObject BeachModule(Transform parent)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_beachPrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, parent.transform, true);
        newModule.transform.position = Find_FreeClipPoint();
        newModule.transform.localScale = Tools.RandomScale(8, 13, 3, 5, 8, 13);
        newModule.GetComponent<IslandModule>().CreateClips();
        m_modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;
    }

    public GameObject CreateModule(Transform parent)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_modulePrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, parent.transform);
        newModule.transform.position = Find_FreeClipPoint() + Vector3.back ;
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

    public Vector3 Find_FreeClipPoint()
    {
        foreach (IslandModule module in m_modules)
        {
            foreach (IClipPoint clip in module.ClipPoints())
            {
                if (clip.GetComponent<IClipPoint>().isFree == true)
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

    public bool CreateDock()
    {
        foreach (IslandModule module in m_modules)
        {
            foreach (IClipPoint clip in module.ClipPoints())
            {
                if (clip.GetComponent<IClipPoint>().isFree == true)
                {
                    m_dock = GameObject.Instantiate(m_dock, module.transform, true);
                    m_dock.transform.position = clip.Try();
                    m_dock.transform.LookAt(m_island.gameObject.transform);
                    bool noCollisionOk= true;
                    foreach (Collider c in m_colliders)
                    {
                        if (m_dock.GetComponentInChildren<PassengerPickUp>()
                                  .GetComponent<Collider>().
                                  bounds.Intersects(
                            c.bounds)
                                  )
                        {
                            noCollisionOk = false;
                        }
                    }
                    switch (noCollisionOk) { 
                        case true: clip.Clip(); return true; }
                }
            }
        }
        Debug.Log("dock NO CLIP");
        return false;
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
}

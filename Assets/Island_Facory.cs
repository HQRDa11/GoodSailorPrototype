using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_Factory 
{
    private int m_oceanLevel;
    private int m_idDistrib;
    private GameObject m_baseA;
    private GameObject m_blockA;

    private GameObject m_dock;

    private List<IslandModule> m_modules;
    private List<Collider> m_colliders;
    List<GameObject> m_modulePrefabs;
    List<GameObject> m_beachPrefabs;
    private GameObject  m_island;
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
        m_idDistrib++;
        m_island  = Instantiate_IslandGameObject();

        GameObject baseModule = CreateBaseModule(m_island.transform);

        for (int i = 0; i < level / 2; i++)
        {
            BeachModule(m_island.transform);
        }
        for (int i = 0; i < level; i++)
        {
            CreateModule(m_island.transform);
        }

        List_AllColliders();
        CreateDock();
        foreach (Collider collider in m_colliders)
        {
            foreach(Collider c in m_dock.GetComponentsInChildren<Collider>())
            {
                Debug.Log(c);
                switch(c.bounds.Intersects(collider.bounds))
                { case true: GameObject.Destroy(c.gameObject);Debug.Log(true); break; }
            }
        }
        TranslateToWaterLevel(m_island);
        m_island.transform.Rotate(Vector3.up,Random.Range(0, 360));
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
        Debug.Log("newModuleOk");
        return newModule;
    }

    public GameObject CreateModule(Transform parent)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_modulePrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, parent.transform);
        newModule.transform.position = Find_FreeClipPoint();
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
                Debug.Log(colider);
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
                                  .GetComponent<Collider>().bounds.Intersects(c.bounds)
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
        return false;
    }
}

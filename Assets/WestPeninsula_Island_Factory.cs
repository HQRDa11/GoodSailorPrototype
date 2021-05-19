using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WestPeninsula_Island_Factory : Island_Factory
{
    public WestPeninsula_Island_Factory(List<GameObject> modulePrefabs): base(modulePrefabs)
    {

    }

    public GameObject CreateIsland(int level)
    {
        m_idDistrib++;
        Island island = Instantiate_IslandGameObject(level).GetComponent<Island>(); ;
        island.transform.position = GameObject.Find("Boat").transform.position + Vector3.forward * 320;
        ClearModuleList(island);
        GameObject baseModule = CreateBaseModule(island);

        for (int i = 0; i < level / 3; i++)
        {
            ClearModuleList(island);
            CreateBeachModule(island);
        }
        for (int i = 0; i < level; i++)
        {
            ClearModuleList(island);
            CreateModule(island);
        }
        GameObject newDock = CreateDock(island);
        int destroyed = 0;
        foreach (Collider collider in GetAllColliders(island))
        {
            switch (newDock.GetComponentInChildren<SphereCollider>().bounds.Intersects(collider.bounds))
            { case true: GameObject.Destroy(collider.gameObject); destroyed++; break; }
        }
        Debug.Log("modules:" + island.Modules.Count);

        island.transform.Rotate(Vector3.up, Random.Range(0, 360));

        // rotate island to dowk face south
        switch (newDock.transform.position.z > island.transform.position.z)
        {
            case true:
                island.transform.Rotate(Vector3.up * 180);
                break;
        }
        TranslateToWaterLevel(island.gameObject);
        return island.gameObject;
    }
    public GameObject Instantiate_IslandGameObject(int level)
    {
        GameObject islandGameObject = new GameObject("Island #" + m_idDistrib.ToString() + " lvl" + level.ToString(),
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
        newBase.GetComponent<IslandModule>().Brakable = false;
        parentIsland.Modules.Add(newBase.GetComponent<IslandModule>());
        return newBase;
    }

    public GameObject CreateBeachModule(Island parentIsland)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_beachPrefabs.Count)];
        GameObject newModule = GameObject.Instantiate(dicedPrefab, parentIsland.transform, true);
        newModule.transform.position = Find_FreeClipPoint(parentIsland);
        newModule.transform.localScale = Tools.RandomScale(8, 13, 3, 5, 8, 13);
        newModule.GetComponent<IslandModule>().Brakable = true;
        newModule.GetComponent<IslandModule>().CreateClips();
        parentIsland.Modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;
    }

    public GameObject CreateModule(Island island)
    {
        GameObject dicedPrefab = m_modulePrefabs[Random.Range(0, m_modulePrefabs.Count)];
        if (!dicedPrefab) { Debug.LogError("no prefab"); }
        GameObject newModule = GameObject.Instantiate(dicedPrefab, island.transform);
        newModule.GetComponent<IslandModule>().Level = 1;
        newModule.GetComponent<IslandModule>().Brakable = true;
        Vector3 randomClipPosition = Vector3.zero;
        for (int i = 0; i < island.Modules.Count || randomClipPosition != Vector3.zero ; i++)
        {
            randomClipPosition = FindRandom_FreeClipPoint(island);
            switch (randomClipPosition == Vector3.zero) // should not enter Why is his pos 0?
            {
                case true:
                    newModule.transform.position = Find_FreeClipPoint(island);
                    Debug.LogWarning("Should not habe a position 0");
                    newModule.transform.position = Vector3.forward * 3; // to remove but Why is his pos 0?
                    break;
            }
        }

        newModule.transform.localScale = Tools.RandomScale(8, 13, 5, 8, 8, 13);
        newModule.transform.Rotate(Vector3.up, Random.Range(0, 360));
        newModule.GetComponent<IslandModule>().CreateClips();
        island.Modules.Add(newModule.GetComponent<IslandModule>());
        return newModule;

    }

    public Vector3 FindRandom_FreeClipPoint(Island island)
    {
        for(int i = 0; i < 12; i++)
        {
            IslandModule triedModule = island.Modules[Random.Range(0, island.Modules.Count)];
            foreach (IClipPoint clip in triedModule.ClipPoints())
            {
                int random = Random.Range(0, triedModule.ClipPoints().Length);
                if (triedModule.ClipPoints()[random].IsFree == true)
                {
                    return clip.Clip();
                }
            }
        }
        return Vector3.zero;
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
                        if (clip.GetComponent<IClipPoint>().IsFree == true)
                        {
                            return clip.Clip();
                        }
                    }
                }
                break;

        }

        return Vector3.zero;
    }
    public List<Collider> GetAllColliders(Island island)
    {
        List<Collider> output = new List<Collider>();
        foreach (IslandModule module in island.Modules)
        {
            foreach (Collider colider in module.gameObject.GetComponentsInChildren<Collider>())
            {
                output.Add(colider);
            }
        }
        return output;
    }

    public GameObject CreateDock(Island island)
    {
        foreach (IslandModule module in island.Modules)
        {
            foreach (IClipPoint clip in module.ClipPoints())
            {
                if (clip.GetComponent<IClipPoint>().IsFree == true)
                {
                    GameObject newDock = new GameObject("dock" + m_idDistrib);
                    newDock = GameObject.Instantiate(m_dock, module.gameObject.transform, true);
                    newDock.GetComponent<IslandModule>().Brakable = false;
                    newDock.transform.position = clip.Try();
                    newDock.transform.LookAt(island.gameObject.transform);
                    newDock.transform.parent = island.gameObject.transform;
                    bool noCollisionOk = true;
                    foreach (Collider c in GetAllColliders(island))
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
                        
                        case true:
                            Debug.LogWarning("dock++");
                            newDock.GetComponent<IslandModule>().Brakable = false;
                            clip.Clip(); return newDock;

                    }
                }
            }
        }

        Debug.LogWarning("no dock?");
        return new GameObject("dockError");
    }

    public void ClearModuleList(Island island)
    {
        //List<IslandModule> toRemove = new List<IslandModule>();
        //foreach (IslandModule module in island.Modules)
        //{
        //    switch (module.gameObject == null)
        //    {
        //        case true: toRemove.Add(module); break;
        //    }
        //}
        //foreach (IslandModule module in toRemove)
        //{
        //    island.Modules.Remove(module);
        //    Debug.Log("Cleaner Works");
        //}
    }

    public void LevelUp(Island island)
    {
        ClearModuleList(island);
        IslandModule newModule = CreateModule(island).GetComponent<IslandModule>();
        Collider newCollider = newModule.GetCollider();
        List<IslandModule> removedModules = FindLowerLevelCollidingModules(island, newModule.Level, newModule.GetCollider());
        int totalRemoved = removedModules.Count / 2;
        newModule.Level = 1;
        switch (totalRemoved > 10)
        {
            case true:
                Vector3 totalScale = Vector3.zero;
                foreach (IslandModule removed in removedModules)
                {
                    if (!newModule) { Debug.LogWarning("error1"); }
                    if (!removed) { Debug.LogWarning("error2"); }
                    else
                    {
                        newModule.Level = (removed.Level > newModule.Level) ? removed.Level : newModule.Level;
                        totalScale += removed.gameObject.transform.localScale;
                        Debug.Log("destructed:" + totalRemoved);
                    }
                }
                newModule.transform.localScale += Vector3.one * newModule.Level * 3;
                Debug.Log("module Level" + newModule.Level);
                break;
        }
        for (int i = 0; i < removedModules.Count && removedModules.Count!=0; i++)
        {
            island.Modules.Remove(removedModules[i]);
            if (!removedModules[i])
            {
                removedModules.RemoveAt(i);
                Debug.LogWarning("no module!");
            }
            else {GameObject.Destroy(removedModules[i].gameObject); };
        }

        island.Modules.Add(newModule);


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

    public List<IslandModule> FindLowerLevelCollidingModules(Island island, int maxLevel, Collider collider)
    {
        List<IslandModule> collidingModules = new List<IslandModule>();
        foreach (IslandModule tested in island.Modules)
        {
            switch (tested.GetCollider() != null
                        && collider != null
                        && tested.Level <= maxLevel
                        && tested.Brakable == true
                        )
            {
                case true:
                    switch (collider.bounds.Intersects(tested.GetCollider().bounds))
                    {
                        case true:
                            collidingModules.Add(tested.gameObject.transform.parent.gameObject.GetComponent<IslandModule>());
                            break;
                    }
                    break;

                case false:
                    Debug.LogWarning(
                        "collider1" + collider.ToString() + "  " +
                        "collider2" + tested.GetCollider().ToString() + "  " +
                        "level:" + tested.Level + "  " +
                        "brakable:" + tested.Brakable);
                    break;
            }
        }

        return collidingModules;
    }
}

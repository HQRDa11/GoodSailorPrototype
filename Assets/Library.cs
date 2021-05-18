using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Library 
{
    // Start is called before the first frame update
    public static List<GameObject> Get_WesternPeninsula_modulePredabs()
    {
        List <GameObject> prefabs = new List<GameObject>();
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleA"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleB"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleC"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleD"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleE"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleF"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleG"));
        prefabs.Add(Resources.Load<GameObject>("Prefabs/Island/ModuleH"));

        return prefabs;
    }
}

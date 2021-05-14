using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent_Factory 
{

    private GameObject m_baseModule;
    private GameObject m_moduleA;
    private GameObject m_moduleB;
    private GameObject m_moduleC;
    private GameObject m_moduleD;

    private int id_distributor;

    // Start is called before the first frame update
    public Continent_Factory()
    {
        id_distributor = 0;
        m_baseModule = Resources.Load<GameObject>("Prefabs/Continent/Continent BaseModule");
        m_moduleA    = Resources.Load<GameObject>("Prefabs/Continent/Continent ModuleA");
        m_moduleB    = Resources.Load<GameObject>("Prefabs/Continent/Continent ModuleB");
        m_moduleC    = Resources.Load<GameObject>("Prefabs/Continent/Continent ModuleC");
        m_moduleD    = Resources.Load<GameObject>("Prefabs/Continent/Continent ModuleD");
    }   

    // Update is called once per frame
    public GameObject Create_Continent()
    {
        id_distributor++;
        GameObject continent = new GameObject("Continent" + id_distributor.ToString());



        return continent;
    }
}

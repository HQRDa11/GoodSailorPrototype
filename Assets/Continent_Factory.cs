using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent_Factory 
{

    private GameObject m_beachModuleA;
    private GameObject m_beachModuleB;
    private GameObject m_moduleA;
    private GameObject m_moduleB;
    private GameObject m_moduleC;
    private GameObject m_moduleD;

    private int id_distributor;
    // Start is called before the first frame update
    public Continent_Factory()
    {
        id_distributor = 0;
        m_beachModuleA = Resources.Load<GameObject>("Prefabs/Continents/Continent BeachModuleA");
        m_beachModuleB = Resources.Load<GameObject>("Prefabs/Continents/Continent BeachModuleB");

        m_moduleA    = Resources.Load<GameObject>("Prefabs/Continents/Continent ModuleA");
        m_moduleB    = Resources.Load<GameObject>("Prefabs/Continents/Continent ModuleB");
        m_moduleC    = Resources.Load<GameObject>("Prefabs/Continents/Continent ModuleC");
        m_moduleD    = Resources.Load<GameObject>("Prefabs/Continents/Continent ModuleD");
    }   

    // Update is called once per frame
    public GameObject Create_Continent()
    {
        id_distributor++;
        
        GameObject continent = new GameObject("Continent" + id_distributor.ToString());
        Debug.Log("continent:"+(continent));
        
        GameObject beachModuleA = GameObject.Instantiate(m_beachModuleA, continent.transform);
        beachModuleA.transform.localScale += Vector3.right * Random.Range(1, 3) + Vector3.forward * Random.Range(1, 3);
        beachModuleA.transform.Rotate(Vector2.up, Random.Range(-180, 180));
        
        GameObject beachModuleB = GameObject.Instantiate(m_beachModuleB, continent.transform);
        beachModuleB.transform.localScale += Vector3.right * Random.Range(1, 3) + Vector3.forward * Random.Range(1, 3);
        beachModuleB.transform.Rotate(Vector2.up, Random.Range(-180, 180));

        //baseModule.transform.Translate(Vector3.up * m_depth);

        GameObject module1 = CreateModule(continent, continent);
        GameObject module2 = CreateModule(continent, module1);
        GameObject module3 = CreateModule(continent, module2);
        GameObject module4 = CreateModule(continent, module3);
        GameObject module5 = CreateModule(continent, module4);
        GameObject module6 = CreateModule(continent, module5);
        GameObject module7 = CreateModule(continent, module6);
        GameObject module8 = CreateModule(continent, module7);

        return continent;
    }

    public GameObject CreateModule(GameObject continent, GameObject baseModule)
    {
        GameObject newModule = GameObject.Instantiate(RandomModule(), continent.transform);
        newModule.transform.localScale += RandomScale();
        //newModule.transform.position = Vector3.up * m_depth;
        newModule.transform.position = RandomPositionAround(baseModule.transform.localPosition, 12 * newModule.transform.localScale.magnitude) ;
        newModule.transform.Rotate(Vector2.up, Random.Range(-180, 180));

        // ROTATION ICI !!!
        return newModule;
    }

    public Vector3 RandomScale()
    {
        Vector3 randomScale = new Vector3(Random.Range(0.2f, 1f), Random.Range(0.2f, 3.1f), Random.Range(0.2f, 1f))/2;
        return randomScale;
    }

    public Vector3 RandomPositionAround(Vector3 origin, float diameter)
    {
        Vector3 randomPosition = new Vector3(origin.x + Random.Range(-diameter, diameter), Random.Range(0, 1), origin.z + Random.Range(-diameter, diameter));
        return randomPosition;
    }
    public GameObject RandomModule()
    {
        int dice = Random.Range(0,4);
        switch (dice)
        {
            case 0:
                return m_moduleA;
            case 1:
                return m_moduleB;
            case 2:
                return m_moduleC;
            case 3:
                return m_moduleD;
        }
        return m_moduleA;
    }
}
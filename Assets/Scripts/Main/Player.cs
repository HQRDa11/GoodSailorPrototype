using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { SAILINGstate = 0, ISLANDstate }
public class Player : MonoBehaviour
{
    public PlayerState m_state;
    public Boat boat;
    public Island islandTarget;
    public Island_Factory testFactory;

    float m_modulesFound;
    public IslandModule targetModule;

    // Start is called before the first frame update
    void Start()
    {
        targetModule = Resources.Load<GameObject>("Prefabs/Error Item").GetComponent<IslandModule>();
        m_modulesFound = 0;
        m_state = new PlayerState();
        boat = GameObject.Find("Boat").GetComponent<Boat>();
        testFactory = new Island_Factory();
    }

    // Update is called once per frame
    void Update()
    {
        switch (boat.navigationState)
        {
            case (NavigationState.DOCKED):
            {
                if (!boat) { Debug.LogError("no boat"); };
                islandTarget = boat.dockedAt.GetComponent<Island>();
                if (Input.GetKeyDown(KeyCode.U))
                {
                    if (!islandTarget) { Debug.LogError("no island here1"); };
                    testFactory.LevelUp(islandTarget);
                    Debug.Log("target Island modules:" + islandTarget.Modules.Count);
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    for (int i = 0; i<5; i++)
                    {
                        if (!islandTarget) { Debug.LogError("no island here2"); };
                        testFactory.LevelUp(islandTarget);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (IsModuleAtClickPoint())
                    {
                        if(targetModule.GetComponent<IslandModule>().isMainModule == false)
                        {
                            Refound(targetModule.GetComponent<IslandModule>());
                        }
                            
                    }
                }
                break;
            }
        }

    }

    public void AddLevel()
    {
        this.islandTarget = boat.dockedAt.GetComponent<Island>();

        switch (this.islandTarget != null)
        {
            case true:
                Island_Factory f = new Island_Factory();
                for (int i = 0; i < 8; i++)
                {     
                    f.LevelUp(islandTarget);
                }
                break;
        }

    }
    public bool IsModuleAtClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask islandMask = LayerMask.GetMask("Island");
        switch (Physics.Raycast(ray,out hit,Mathf.Infinity, 1<<6)
            && hit.transform.parent.gameObject.GetComponent<IslandModule>())
        {
            case true:
                targetModule = hit.transform.parent.gameObject.GetComponent<IslandModule>();
                return true;
            case false:
                Debug.Log("hit mismatch");
                Debug.Log(hit.transform.gameObject);
                //if (hit.transform.gameObject != null) { Debug.Log("name:"+ hit.transform.gameObject.name); }
                return false;
        }
    }

        
    public void Refound(IslandModule module)
    {
        m_modulesFound += module.Level; 
        Island parent = module.GetIsland();
        parent.Modules.Remove(module);
        GameObject.Destroy(module.gameObject);
    }
}


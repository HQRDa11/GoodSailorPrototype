using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { SAILINGstate = 0, ISLANDstate }
public class Player : MonoBehaviour
{
    public PlayerState m_state;
    public Boat boat;
    public Island islandTarget;
    Island_Factory testFactory;

    // Start is called before the first frame update
    void Start()
    {
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
                        Debug.Log("targetIsland: " + islandTarget);
                        for (int i = 0; i<8; i++)
                        {
                            if (!islandTarget) { Debug.LogError("no island here2"); };
                            testFactory.LevelUp(islandTarget);
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
}


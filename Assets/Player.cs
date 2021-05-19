using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { SAILINGstate = 0, ISLANDstate }
public class Player : MonoBehaviour
{
    public PlayerState m_state;
    public Boat boat;
    public Logic logic;
    public Island islandTarget;
    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat").GetComponent<Boat>();
        logic = GameObject.Find("Logic").GetComponent<Logic>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (boat.navigationState)
        {
            case (NavigationState.DOCKED):
            {
                    this.islandTarget = boat.dockedAt.GetComponent<Island>();
                    if (Input.GetKeyDown(KeyCode.U))
                    {
                        logic.WestPeninsulaFactory.LevelUp(islandTarget);
                        Debug.Log("target Island modules:" + islandTarget.Modules.Count);
                    }
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        for (int i = 0; i<8; i++)
                        {
                            logic.WestPeninsulaFactory.LevelUp(islandTarget);
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
                for (int i = 0; i < 8; i++)
                {
                    logic.WestPeninsulaFactory.LevelUp(islandTarget);
                }
                break;
        }

    }
}


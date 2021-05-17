using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { SAILINGstate = 0, ISLANDstate }
public class Player : MonoBehaviour
{
    public PlayerState m_state;
    public Boat boat;
    public Island islandTarget;
    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat").GetComponent<Boat>();
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
                        islandTarget.levelUp();
                    }
                    break;
            }
        }
    }
}

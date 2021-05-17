using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public List<IslandModule> modules;
    private void Awake()
    {
        modules = new List<IslandModule>();
    }
}

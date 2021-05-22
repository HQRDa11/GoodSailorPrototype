using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public List<IslandModule> m_modules;
    public List<IslandModule> Modules { get { return m_modules; } set { m_modules = value; } }
    public int Level { get; set; }
    private IslandModule m_dock;
    public IslandModule Dock { get { return m_dock; } set { m_dock = value; } }

    private void Awake()
    {
        m_modules = new List<IslandModule>();
        IslandModule[] modules = gameObject.GetComponentsInChildren<IslandModule>();
        foreach (IslandModule toAdd in modules)
        {
            m_modules.Add(toAdd);
        }
    }
}

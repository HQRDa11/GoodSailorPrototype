using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    private List<IslandModule> m_modules;
    public List<IslandModule> Modules { get { return m_modules; } set { m_modules = value; } }
    public int Level { get; set; }

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

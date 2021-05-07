using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    private GameObject m_leave_prefab;
    private List<GameObject> m_leaveInstances;
    private List<Leave> m_leaves;

    private float m_spawnTimerMax;
    private float m_spawnTimer;
    public float spawnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_leave_prefab = Resources.Load<GameObject>("Prefabs/Leave");
        m_leaveInstances = new List<GameObject>();
        m_leaves = new List<Leave>();
        m_spawnTimerMax = 1f;
        m_spawnTimer = 0;
        spawnSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnTimer += spawnSpeed * Time.deltaTime ;
        
        if (m_spawnTimer >=m_spawnTimerMax)
        {
            m_leaves.Add(new Leave(2));
            GameObject leaveGO = Instantiate(m_leave_prefab);
            leaveGO.transform.position = this.transform.position;
            m_leaveInstances.Add(leaveGO);
            m_spawnTimer = 0;
        }

        List<Leave> deletedLeaves = new List<Leave>();
        foreach( Leave leave in m_leaves)
        {
            if (leave.UpdateAndAskIsDead(Time.deltaTime))
            {
                GameObject deleted = m_leaveInstances[0];
                m_leaveInstances.RemoveAt(0);
                GameObject.Destroy(deleted);
                deletedLeaves.Add(leave);
            }
        }

        foreach (Leave l in deletedLeaves)
        {
            m_leaves.Remove(l);
        }
    }



    /// <summary>
    /// ///////////////////////////////////////////
    /// </summary>
    public class Leave
    {
        public float m_maxLife;
        public float m_currentLife;
        public Leave(float maxLife)
        {
            m_maxLife = maxLife;
            m_currentLife = 0;
        }
        public bool UpdateAndAskIsDead(float modifier)
        {
            m_currentLife += modifier;
            return (m_currentLife > m_maxLife) ? true : false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandModule : MonoBehaviour
{
    public int Level;
    public bool isMainModule;
    private Island m_islandParent;
    public int LayerMask = 6;
    public void Start()
    {
        Level = 1;
        this.gameObject.layer = LayerMask;
        if (isMainModule)
        {
            LayerMask = 0;
        }
        foreach (Transform transform in this.gameObject.GetComponentsInChildren<Transform>())
        {
            transform.gameObject.layer = LayerMask;
        }
    }
    public enum Coordinates
    {
        N = 180,
        NE = 135,
        E = 90,
        SE = 45,
        S = 0,
        SO = -45,
        O = -90,
        NO = -135,

        Count = 8
    }
    private bool[] m_branches;
    public void CreateClips()
    {
        List<GameObject> clipPoints = new List<GameObject>();
        for (int i = 0; i <= 360; i += Random.Range(45, 90))
        {
            GameObject clip = new GameObject("Clip", typeof(IClipPoint));
            clip.transform.parent = this.gameObject.transform;
            clip.transform.position = this.gameObject.transform.position +
                       Vector3.back * this.gameObject.transform.localScale.z / 2;
            clip.transform.RotateAround(this.gameObject.transform.position, Vector3.up, i);
            clipPoints.Add(clip);
        }

        if (!this.isMainModule)
        for (int i = 0; i < clipPoints.Count; i++)
        {
            bool dice = (Random.Range(1, 10) > 3) ? true : false;
            switch (dice)
            { case true: GameObject.Destroy(clipPoints[i]); break; }
        }
    }
    public IClipPoint ClipPoint()
    {
        return gameObject.GetComponentInChildren<IClipPoint>();
    }
    public IClipPoint[] ClipPoints()
    {
        return gameObject.GetComponentsInChildren<IClipPoint>();
    }
    public Collider[] GetAllColliders()
    {
        return gameObject.GetComponentsInChildren<Collider>();
    }
    public Collider GetCollider()
    {
        return gameObject.GetComponentInChildren<Collider>();
    }

    public Island GetIsland()
    {
        switch (m_islandParent != null)
        {
            case true:
                return m_islandParent;

            case false:
                m_islandParent = gameObject.transform.parent.gameObject.GetComponent<Island>();
                switch (m_islandParent != null)
                {
                    case true:
                        Debug.Log("island parent: " + m_islandParent);
                        return m_islandParent;

                    case false:
                        Debug.LogWarning("Error island not set to " + this.gameObject.name);
                        Island error = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Error Irem"), this.gameObject.transform).AddComponent<Island>();
                        error.transform.position = this.gameObject.transform.position;
                        return error;
                }
        }
    }
    public void SetClips(bool value)
    {
        foreach(IClipPoint clip in gameObject.GetComponentsInChildren<IClipPoint>())
        {
            clip.isFree = true;
        }
    }
}

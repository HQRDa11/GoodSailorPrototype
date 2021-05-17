using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandModule : MonoBehaviour
{
    public enum Coordinates { 
        N   = 180,
        NE  = 135, 
        E   = 90,
        SE  = 45,
        S   = 0,
        SO  = -45,
        O   = -90,
        NO  = -135,
        
        Count = 8
    }
    private bool[] m_branches;
    public void CreateClips()
    {
        List<GameObject> clipPoints = new List<GameObject>();
        for (int i = 0; i<=360; i+=Random.Range(45,90))
        {
            GameObject clip = new GameObject("Clip",typeof(IClipPoint));
            clip.transform.parent = this.gameObject.transform;
            clip.transform.position = this.gameObject.transform.position + 
                       Vector3.back * this.gameObject.transform.localScale.z/2 ;
            clip.transform.RotateAround(this.gameObject.transform.position, Vector3.up, i);
            clipPoints.Add(clip);
        }

        for (int i = 0; i<clipPoints.Count; i++)
        {
            bool dice = (Random.Range(1, 10) > 6) ? true : false;
            switch(dice)
            { case true: GameObject.Destroy(clipPoints[i]);break; }
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IClipPoint : MonoBehaviour
{
    public bool isFree;
    void Awake()
    {
        isFree = true;
    }
    public Vector3 Clip()
    {
        switch(isFree)
        {
            case true:
                isFree = false;
                return this.transform.position;
        }
        return Vector3.zero;
    }
    public bool Check()
    {
        return isFree;
    }
    public Vector3 Try()
    {
        switch (isFree)
        {
            case true:
                return this.transform.position;
        }
        return Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pickup_Sound;
    bool picked;
    float lifeLeft;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        pickup_Sound = Resources.Load<AudioClip>("AudioClips/Bounce");
        picked = false;
        lifeLeft =0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        switch(picked)
        {
            case true:
                if (lifeLeft > 0)
                {
                    gameObject.transform.position += Vector3.up * Time.deltaTime * 80;
                    gameObject.transform.localScale += Vector3.one * Time.deltaTime * 12f;
                    lifeLeft -= Time.deltaTime;
                }
                else { GameObject.Destroy(this.gameObject); }
                break;
        }
    }

    public void OnPickUp()
    {
        if (audioSource.clip != pickup_Sound)
        {
            audioSource.clip = pickup_Sound;
            audioSource.Play();
        }
        picked = true;
    }
}

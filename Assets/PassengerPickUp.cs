using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerPickUp : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pickup_Sound;
    bool picked;
    float lifeLeft;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        pickup_Sound = Resources.Load<AudioClip>("AudioClips/Bells");
        picked = false;
        lifeLeft = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (picked)
        {
            case true:
                if (lifeLeft > 0)
                {
                    gameObject.transform.localScale -= Vector3.one * Time.deltaTime * 3f;
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
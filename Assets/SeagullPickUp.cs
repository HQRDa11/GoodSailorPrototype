using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullPickUp : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pickup_Sound;
    bool picked;
    float lifeLeft;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        pickup_Sound = Resources.Load<AudioClip>("AudioClips/Seagulls");
        picked = false;
        lifeLeft = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (picked)
        {
            case true:
                if (lifeLeft > 0)
                {
                    gameObject.transform.localScale -= Vector3.one * Time.deltaTime * 2f;
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

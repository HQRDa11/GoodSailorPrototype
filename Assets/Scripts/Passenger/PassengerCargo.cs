using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassengerStatus { RED = 0, WHITE, GREEN }
public class PassengerCargo : MonoBehaviour
{
    Boat boat;
    public List<Passenger> passengers;
    public GameObject passengerGo;

    public Material colorRed;
    public Material colorWhite;
    public Material colorGreen;

    //Transfert
    public float transfertFlow_Timer;
    public float transfertFlow_TimerCurrent;
    public int   passengerIncoming;
    Vector3      embarkTarget;
    bool transfertState;

    AudioSource audioSource;
    AudioClip passengerLevelUp_audioClip;

    Game_UserInterface UI;
    Player player;
    public int CurrentSatisfactionDisplay { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat").GetComponent<Boat>();
        passengers = new List<Passenger>();
        passengerGo = Resources.Load<GameObject>("Prefabs/Passenger");

        colorRed   = Resources.Load<Material>("Materials/Status/RedStatus");
        colorWhite = Resources.Load<Material>("Materials/Status/WhiteStatus");
        colorGreen = Resources.Load<Material>("Materials/Status/GreenStatus");

        transfertFlow_Timer = 0.6f;
        transfertFlow_TimerCurrent = 0;
        passengerIncoming = 0;
        embarkTarget = Vector3.zero;
        transfertState = false;

        audioSource = gameObject.GetComponent<AudioSource>();
        passengerLevelUp_audioClip = Resources.Load<AudioClip>("AudioClips/Droplet");
        audioSource.clip = passengerLevelUp_audioClip;

        player = GameObject.Find("Player").GetComponent<Player>();

        UI = GameObject.Find("UI").GetComponent<Game_UserInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (transfertState)
        {
            case true:
                switch (passengerIncoming > 0 || getIsGreenLeft()!=null)
                {
                    case true:
                        transfertFlow_TimerCurrent += Time.deltaTime;
                        if (transfertFlow_TimerCurrent > transfertFlow_Timer)
                        {
                            // exchanger
                            ProceedTransfertStep();
                            transfertFlow_TimerCurrent = 0;
                        }
                        return;
                    case false:
                        transfertState = false;
                        break;
                }
                break;
        }
    }

    public void SortPassenger()
    {
        List<Passenger> listToSort = new List<Passenger>();
        foreach(Passenger p in passengers)
        {
            if(p.status == PassengerStatus.GREEN)
            {
                listToSort.Add(p);
            }
        }
        foreach (Passenger p in passengers)
        {
            if (p.status == PassengerStatus.WHITE)
            {
                listToSort.Add(p);
            }
        }
        foreach (Passenger p in passengers)
        {
            if (p.status == PassengerStatus.RED)
            {
                listToSort.Add(p);
            }
        }
        float margin = 1f;
        int nbOfRanks = Mathf.CeilToInt(Mathf.Sqrt(listToSort.Count));
        Vector3 origin = this.transform.position + Vector3.left*(margin*nbOfRanks-1)/2 + Vector3.forward * (margin * nbOfRanks - 1) / 2;
        for (int x = 0; x < nbOfRanks && listToSort.Count> 0; x++)
        {
            for (int y = 0; y < nbOfRanks && listToSort.Count > 0; y++)
            {
                listToSort[0].transform.position = origin + (Vector3.right * x * margin) + (Vector3.back * y * margin);
                listToSort.RemoveAt(0);
            }
        }

    }
    public void ProceedTransfertStep()
    {
        // REMOVAL
        switch ( getIsGreenLeft() != null )
        {
            case true:
                Passenger removed = getIsGreenLeft();
                int newIncome = (int)removed.satisfaction / 20;
                GetComponentInParent<Boat>().playerMoney += newIncome;
                UI.DisplayTransaction(newIncome);
                RemovePassenger(removed);
                return;
        }
        
        switch (passengerIncoming > 0)
        {
            case true:
                AddPassenger();
                passengerIncoming--;
                break;
            case false:
                transfertState = false;
                return;
        }
    }
    public Passenger AddPassenger()
    {

        switch ( boat.navigationState != NavigationState.TRANSFERT && boat.currentSpeed<=10 )
        {
            case true:
                boat.navigationState = NavigationState.TRANSFERT;
                break;
        }

        GameObject newPassenger = GameObject.Instantiate(passengerGo);
        newPassenger.transform.parent = this.transform;
        newPassenger.transform.position = this.transform.position + Vector3.right * Random.Range(-1.8f,1.8f);
        passengers.Add(newPassenger.GetComponent<Passenger>());
        audioSource.Play();
        SortPassenger();
        return newPassenger.GetComponent<Passenger>();
    }
    public void RemovePassenger(Passenger removed)
    {
        player.AddLevel();
        GameObject.Destroy(removed.gameObject);
        passengers.Remove(removed);
        audioSource.Play();
        SortPassenger();
    }
    public void OnPassengerTransfer(Vector3 embarkPoint)
    {
        passengerIncoming = Random.Range(1,6);
        embarkTarget = embarkPoint;
        transfertState = true;
    }
    public void OnPassengerBonus(float bonus)
    {
        int palier = 10;
        CurrentSatisfactionDisplay = (int)(bonus / Time.deltaTime * 10);

        bonus /= (passengers.Count/2) +1;
        foreach (Passenger passenger in passengers)
        {
            passenger.satisfaction += bonus;
            switch (passenger.status)
            {
                case PassengerStatus.RED:
                    switch(passenger.satisfaction>0)
                    {
                        case true: passenger.status = PassengerStatus.WHITE;
                            passenger.SetMaterial(colorWhite);
                            audioSource.Play();
                            break;
                    }
                    break;

                case PassengerStatus.WHITE:
                    switch (passenger.satisfaction > palier+palier)
                    {
                        case true:
                            passenger.status = PassengerStatus.GREEN;
                            passenger.SetMaterial(colorGreen);
                            audioSource.Play();
                            break;
                    }
                    switch (passenger.satisfaction < 0-palier)
                    {
                        case true:
                            Debug.Log("11");
                            passenger.status = PassengerStatus.RED;
                            passenger.SetMaterial(colorRed);
                            audioSource.Play();
                            break;
                    }
                    break;

                case PassengerStatus.GREEN:
                    switch (passenger.satisfaction < palier+palier )
                    {
                        case true:
                            Debug.Log("12");
                            passenger.status = PassengerStatus.WHITE;
                            passenger.SetMaterial(colorWhite);
                            audioSource.Play();
                            break;
                    }
                    break;
            }

        }
    }
    public Passenger getIsGreenLeft()
    {
        foreach (Passenger passenger in passengers)
        {
            switch (passenger.status == PassengerStatus.GREEN)
            {
                case true:
                    return passenger;

            }
        }
        return null;
    }
    
}

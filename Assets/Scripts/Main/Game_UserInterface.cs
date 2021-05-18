using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UserInterface : MonoBehaviour
{
    public Logic logic;

    public Text navigationPoints;
    public Text playerMoney;

    public int displayTimer;
    public float displayTimer_current;

    public Text transaction_display;
    public List<int> transactions;

    public PassengerCargo passengerCargo;
    public GameObject satisfactionDisplay_prefab;


    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.Find("Logic").GetComponent<Logic>();
        navigationPoints = GameObject.Find("NavPoints Display").GetComponent<Text>();
        playerMoney = GameObject.Find("PlayerMoney Display").GetComponentInChildren<Text>();

        transactions = new List<int>();
        displayTimer = 1;
        displayTimer_current = 0;

        transaction_display = GameObject.Find("Transaction Display").GetComponent<Text>();

        passengerCargo = GameObject.Find("Passenger Cargo").GetComponent<PassengerCargo>();
        satisfactionDisplay_prefab = Resources.Load<GameObject>("Prefabs/UI/Satisfaction Display");
    }

    // Update is called once per frame
    void Update()
    {
        int nps = (int)logic.navPoints;
        navigationPoints.text = nps.ToString() + "NP";
        int money = (int)logic.PlayerMoney;
        playerMoney.text =  money.ToString()+"€";

        displayTimer_current += Time.deltaTime;
        switch (displayTimer_current > displayTimer)
        {
            case true:
                displayTimer_current = 0;
                Update_TransactionDisplay();
                Update_SatisfactionDisplay();
                break;
        }
    }

    public void DisplayTransaction(int moneyAmount)
    {
        transactions.Add(moneyAmount);
        displayTimer_current = displayTimer;
    }

    private void Update_TransactionDisplay()
    {
        switch ( transactions.Count != 0 )
        {
            case true:
                // make go active
                if (!transaction_display.gameObject.activeSelf)
                {
                    transaction_display.gameObject.SetActive(true);
                }
                transaction_display.text = transactions[0].ToString();
                transaction_display.transform.position += Vector3.up;
                transactions.RemoveAt(0);
                break;

            case false:
                transaction_display.gameObject.SetActive(false);
                break;
        }
    }
    private void Update_SatisfactionDisplay()
    {
        if (passengerCargo.CurrentSatisfactionDisplay!=0)
        {
            GameObject display = GameObject.Instantiate(satisfactionDisplay_prefab, this.transform);
            display.GetComponent<Text>().text = passengerCargo.CurrentSatisfactionDisplay.ToString();
        }
    }

    public void Display_AreaName( string name )
    {
        GameObject newDisplay = Instantiate(Resources.Load<GameObject>("Prefabs/UI/AreaName Display"));
        newDisplay.transform.SetParent(this.transform,false) ;
        newDisplay.GetComponent<Text>().font = Resources.Load<Font>("Fonts/CraftyLover");
        newDisplay.AddComponent<AreaName_Display>().Initialise(name);
    }
}

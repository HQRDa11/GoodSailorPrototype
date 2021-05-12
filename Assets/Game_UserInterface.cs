using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UserInterface : MonoBehaviour
{
    public Logic logic;

    public Text navigationPoints;
    public Text playerMoney;

    public List<int> transactions;
    public Text transactionDisplay;
    public int transactionDisplay_timer;
    public float transactionDisplay_timer_current;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.Find("Logic").GetComponent<Logic>();
        navigationPoints = GameObject.Find("NavPoints Display").GetComponent<Text>();
        playerMoney = GameObject.Find("PlayerMoney Display").GetComponentInChildren<Text>();

        transactions = new List<int>();
        transactionDisplay_timer = 1;
        transactionDisplay_timer_current = 0;

        transactionDisplay = GameObject.Find("Transaction Display").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int nps = (int)logic.navPoints;
        navigationPoints.text = nps.ToString() + "NP";
        int money = (int)logic.PlayerMoney;
        playerMoney.text =  money.ToString()+"€";

        transactionDisplay_timer_current += Time.deltaTime;
        switch (transactionDisplay_timer_current > transactionDisplay_timer)
        {
            case true:
                Update_TransactionDisplay();
                transactionDisplay_timer_current = 0;
                break;
        }
    }

    public void DisplayTransaction(int moneyAmount)
    {
        transactions.Add(moneyAmount);
        transactionDisplay_timer_current = transactionDisplay_timer;
    }

    private void Update_TransactionDisplay()
    {
        switch ( transactions.Count != 0 )
        {
            case true:
                // make go active
                if (!transactionDisplay.gameObject.activeSelf)
                {
                    transactionDisplay.gameObject.SetActive(true);
                }
                transactionDisplay.text = transactions[0].ToString();
                transactionDisplay.transform.position += Vector3.up;
                transactions.RemoveAt(0);
                break;

            case false:
                transactionDisplay.gameObject.SetActive(false);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UserInterface : MonoBehaviour
{
    public Logic logic;

    public Text navigationPoints;
    public Text playerMoney;
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.Find("Logic").GetComponent<Logic>();
        navigationPoints = GameObject.Find("NavPoints Display").GetComponent<Text>();
        playerMoney = GameObject.Find("PlayerMoney Display").GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int nps = (int)logic.navPoints;
        navigationPoints.text = nps.ToString() + "NP";
        int money = (int)logic.PlayerMoney;
        playerMoney.text =  money.ToString()+"€";
    }
}

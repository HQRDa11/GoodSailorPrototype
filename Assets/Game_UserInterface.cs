using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UserInterface : MonoBehaviour
{
    public Logic logic;

    public Text navPoints_Display;
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.Find("Logic").GetComponent<Logic>();
        navPoints_Display = GameObject.Find("NavPoints Display").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int nps = (int)logic.navPoints;
        navPoints_Display.text = "NPs.:" + nps.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AreaName_Display : MonoBehaviour
{
    public string toDisplay;
    public int displayedTime;
    public float flowTimer;
    public float flowTimer_current;
    private bool isInitialised;
    public int stepsLeft;
    private int currentStep;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        flowTimer = 0.12f;
    }

    public void Initialise (string name)
    {
        flowTimer_current = 0;
        currentStep = 0;
        this.gameObject.transform.position += Vector3.up * 1 / 4 * Screen.height;
        text = gameObject.GetComponent<Text>();
        isInitialised = true;
        toDisplay = name;
        displayedTime = 28;
        stepsLeft = toDisplay.Length + displayedTime;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(isInitialised);
        if (isInitialised)
        {
            flowTimer_current += Time.deltaTime;
            if (flowTimer_current > flowTimer)
            {
                if (currentStep < toDisplay.Length)
                {
                    NextStep();
                }
                else if (stepsLeft == 0)
                {
                    GameObject.Destroy(this.gameObject);
                }
                stepsLeft--;
                flowTimer_current = 0;
            }
        }
    }

    private void NextStep()
    {
        text.text = "";
        for (int i = 0; i < currentStep + 1; i++)
        {
            text.text += toDisplay[i];
        }
        currentStep++;
    }
}

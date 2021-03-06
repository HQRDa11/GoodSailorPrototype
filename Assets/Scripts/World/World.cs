using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldState { WEST_PENINSULA = -3, WEST_CONTINENTS = -2, WEST_OCEAN, MIDDLE_ISLANDS = 0 ,EAST_OCEAN, EAST_CONTINENT};
public class World 
{
    private WorldState      m_currentState;
    private IWorldDecorator<List<GameObject>> m_decorator;


    // Start is called before the first frame update
    public World( WorldState startZone)
    {
        m_decorator = new WorldDecorator_Ocean();
        SwitchState(startZone);
    }

    // Update is called once per frame
    public void Update()
    {
        m_decorator.Update();
    }

    public void SwitchState(WorldState newState)
    {
        List<GameObject> listOfDecors = this.m_decorator.GetTotalModulesList();
        switch (newState)
        {
            case WorldState.WEST_PENINSULA:
                m_currentState = WorldState.WEST_PENINSULA;
                this.m_decorator = new WorldDecorator_WestPeninsula();
                m_decorator.SetTotalModulesList(listOfDecors);
                return;

            case WorldState.WEST_CONTINENTS:
                m_currentState = WorldState.WEST_CONTINENTS;
                this.m_decorator = new WorldDecorator_WesternContinents();
                m_decorator.SetTotalModulesList(listOfDecors);
                return;

            case WorldState.WEST_OCEAN:
                m_currentState = WorldState.WEST_OCEAN;
                this.m_decorator = new WorldDecorator_Ocean();
                m_decorator.SetTotalModulesList(listOfDecors);
                return;

            case WorldState.MIDDLE_ISLANDS:
                m_currentState = WorldState.MIDDLE_ISLANDS;
                this.m_decorator = new WorldDecorator_MiddleIslands();
                m_decorator.SetTotalModulesList(listOfDecors);
                return;

            case WorldState.EAST_OCEAN:
                m_currentState = WorldState.EAST_OCEAN;
                this.m_decorator = new WorldDecorator_Ocean();
                m_decorator.SetTotalModulesList(listOfDecors);
                return;

            case WorldState.EAST_CONTINENT:
                m_currentState = WorldState.EAST_CONTINENT;
                this.m_decorator = new WorldDecorator_EasternContinents();
                m_decorator.SetTotalModulesList(listOfDecors);
                return;

            default:
                Debug.LogWarning(" WorldState: not implemented yet");
                return;
        }
    }

    public void NextState_East()
    {
        switch (m_currentState)
        {
            case WorldState.WEST_PENINSULA:
                SwitchState(WorldState.WEST_CONTINENTS);
                return;
            case WorldState.WEST_CONTINENTS:
                SwitchState(WorldState.WEST_OCEAN);
                return;
            case WorldState.WEST_OCEAN:
                SwitchState(WorldState.MIDDLE_ISLANDS);
                return;
            case WorldState.MIDDLE_ISLANDS:
                SwitchState(WorldState.EAST_OCEAN);
                return;
            case WorldState.EAST_OCEAN:
                SwitchState(WorldState.EAST_CONTINENT);
                return;
            case WorldState.EAST_CONTINENT:
                Debug.LogWarning("There is nothing beyond Eastern Continents");
                return;
        }
    }
    public void NextState_West()
    {
        switch (m_currentState)
        {
            case WorldState.WEST_PENINSULA:
                Debug.LogWarning("There is nothing beyond West Peninsula");
                return;
            case WorldState.WEST_CONTINENTS:
                SwitchState(WorldState.WEST_PENINSULA);
                return;
            case WorldState.WEST_OCEAN:
                SwitchState(WorldState.WEST_CONTINENTS);
                return;
            case WorldState.MIDDLE_ISLANDS:
                SwitchState(WorldState.WEST_OCEAN);
                return;
            case WorldState.EAST_OCEAN:
                SwitchState(WorldState.MIDDLE_ISLANDS);
                return;
            case WorldState.EAST_CONTINENT:
                SwitchState(WorldState.EAST_OCEAN);
                return;
        }
    }

    public string GetAreaName()
    {
        switch (m_currentState)
        {
            case WorldState.WEST_PENINSULA:
                return "West Peninsula";
            case WorldState.WEST_CONTINENTS:
                return "Western Continents";
            case WorldState.WEST_OCEAN:
                return "West Ocean";
            case WorldState.MIDDLE_ISLANDS:
                return "Middle Islands";
            case WorldState.EAST_OCEAN:
                return "East Ocean";
            case WorldState.EAST_CONTINENT:
                return "Eastern Continents";
        }
        return "no name yet";
    }
}

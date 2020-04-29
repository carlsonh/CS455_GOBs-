using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string name;
    public float value;

    public Goal (string incomingGoalName, float incomingGoalValue)
    {
        name = incomingGoalName;
        value = incomingGoalValue;
    }

}

public class Action
{
    public string name;
    public List<Goal> targetGoals;

    public Action (string actionName)
    {
        name = actionName;
        targetGoals = new List<Goal>();
    }


    public float getGoalChange(Goal goal)
    {
        foreach (Goal target in targetGoals)
        {
            if (target.name == goal.name)
            {
                return target.value;
            }
        }
        return 0.0f;
    }
}


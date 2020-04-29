using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chooseAction : MonoBehaviour
{
    Goal[] goals;
    Action[] actions;
    Action delta;


    Scrollbar finalsBar;
    Scrollbar homeworkBar;
    Scrollbar sleepBar;

    Text actionName;
    Text actionFinals;
    Text actionHomework;
    Text actionSleep;

    void Start()
    {

        //Setup Debug Bars
        finalsBar       = GameObject.Find("Finals").GetComponent<Scrollbar>();
        homeworkBar     = GameObject.Find("Homework").GetComponent<Scrollbar>();
        sleepBar        = GameObject.Find("Sleep").GetComponent<Scrollbar>();
        
        actionName      = GameObject.Find("actionName").GetComponent<Text>();
        actionFinals    = GameObject.Find("actionFinals").GetComponent<Text>();
        actionHomework  = GameObject.Find("actionHomework").GetComponent<Text>();
        actionSleep     = GameObject.Find("actionSleep").GetComponent<Text>();


        //Setup goals
        goals = new Goal[3];
        goals[0] = new Goal("Finals", 5);
        goals[1] = new Goal("Homework", 4);
        goals[2] = new Goal("Sleep", 3);


        //Setup actions
        actions = new Action[5];

        actions[0] = new Action("Work on GOBs");
        actions[0].targetGoals.Add(new Goal("Finals",   -.1f));
        actions[0].targetGoals.Add(new Goal("Homework", -.4f));
        actions[0].targetGoals.Add(new Goal("Sleep",    +.1f));

        actions[1] = new Action("Work on Reels");
        actions[1].targetGoals.Add(new Goal("Finals",   -.4f));
        actions[1].targetGoals.Add(new Goal("Homework", -.1f));
        actions[1].targetGoals.Add(new Goal("Sleep",    +.1f));
        
        actions[2] = new Action("Work on Pages");
        actions[2].targetGoals.Add(new Goal("Finals",   -.3f));
        actions[2].targetGoals.Add(new Goal("Homework", -.2f));
        actions[2].targetGoals.Add(new Goal("Sleep",    +.1f));
        
        actions[3] = new Action("Nap");
        actions[3].targetGoals.Add(new Goal("Finals",  +.1f));
        actions[3].targetGoals.Add(new Goal("Homework",  0f));
        actions[3].targetGoals.Add(new Goal("Sleep",   -.3f));
        
        actions[4] = new Action("Sleep");
        actions[4].targetGoals.Add(new Goal("Finals",   +.1f));
        actions[4].targetGoals.Add(new Goal("Homework", +.1f));
        actions[4].targetGoals.Add(new Goal("Sleep",   -0.8f));


        //Passive gain
        delta = new Action("Delta");
        delta.targetGoals.Add(new Goal("Finals",   +0.1f));
        delta.targetGoals.Add(new Goal("Homework", +0.1f));
        delta.targetGoals.Add(new Goal("Sleep",    +0.25f));

        //Tick, Pass time and update
        InvokeRepeating("TickUpdate", 0f, 0.30f);
        
    }


    void TickUpdate()
    {
        foreach (Goal _g in goals)
        {//Add delta, passive gain to each goal
            _g.value += delta.getGoalChange(_g);
            _g.value = Mathf.Max(_g.value, 0);
        }

        DoAction();
        
    }

    void DoAction()
    {
        Action immediateBest = simpleChooseAction(actions, goals);
        foreach(Goal _g in goals)
        {
            _g.value += immediateBest.getGoalChange(_g);
            //No negative goals, just like life
            _g.value = Mathf.Max(_g.value, 0);
        }
    }


    void BarUpdate()
    {//Not perfect, but the system will tend towards zero anyways
        //Set the fill value of the bars to the current weight of the 
        finalsBar.size      = goals[0].value/5f;
        homeworkBar.size    = goals[1].value/5f;
        sleepBar.size       = goals[2].value/5f;
    }


    public Action simpleChooseAction(Action[] actions, Goal[] goals)
    {///Find the highest priority goal, and find the action that impacts it the most

        Goal topGoal = goals[0];
        foreach (Goal _g in goals)
        {//Find highest priority goal
            if (_g.value > topGoal.value)
            {
                topGoal = _g;
            }
        } 
        Debug.Log("Highest Priority Goal: " + topGoal.name + " with power " + topGoal.value);



        Action bestAction = actions[0];
        float bestUtility = -actions[0].getGoalChange(topGoal);

        foreach (Action _act in actions)
        {//Loop through each action and find the one with the strongest impact
            float _utility = -_act.getGoalChange(topGoal);

            if (_utility > bestUtility)
            {//A better action was found, set that as top
                bestUtility = _utility;
                bestAction = _act;
            }
        }
        Debug.Log("Best Response: " + bestAction.name);




        //Update debug info
        BarUpdate();
        //Don't want to bother passing the action around
        actionName.text = bestAction.name;
        actionFinals.text = bestAction.targetGoals[0].value.ToString();
        actionHomework.text = bestAction.targetGoals[1].value.ToString();
        actionSleep.text = bestAction.targetGoals[2].value.ToString();

        return bestAction;
    }
}
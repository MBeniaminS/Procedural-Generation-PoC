using System.Collections.Generic;
using UnityEngine;

public class NPCwithFSM : MonoBehaviour
{

    private string mCurrentState;
    //private List<NPCState> statesList = new List<NPCState>();
    private Dictionary<string, NPCState> statesList = new Dictionary<string, NPCState>();

    // Use this for initialization
    void Start()
    {
        mCurrentState = "";
        DiscoverBehaviors();
        SetState("Patrol");
    }


    //find any behaviors which are subclasses of NPCState which have been attached to current game object
    void DiscoverBehaviors()
    {
        Component[] tempList;
        tempList = GetComponents<NPCState>();
        int statesLoaded = 0;
        //transfer states from array into the Dictionary, using the text name as the key
        foreach (NPCState state in tempList)
        {
            Debug.Log("//component found:" + state);
            statesList.Add(state.strStateName, state as NPCState);
            statesLoaded++;
            //disable all states at beginning
            state.enabled = false;
            Debug.Log("//NPCFSM found behavior:" + state.strStateName);
            state.myParentFSM = this;
        }
        Debug.Log("//NPCFSM:behaviors found: " + statesLoaded);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            SetState("Patrol");
        }
        if (Input.GetKey("2"))
        {
            SetState("Persue");
        }
        if (Input.GetKey("3"))
        {
            SetState("Evade");
        }
    }


    public void SetState(string strState)
    {
        //disable previous state
        if (mCurrentState != "") (statesList[mCurrentState] as MonoBehaviour).enabled = false;
        mCurrentState = strState;
        (statesList[strState] as MonoBehaviour).enabled = true;

    }

    void OnGUI()
    {
        //display current target
        GUI.Label(new Rect(10, 30, 300, 20), "NPC Current State:" + (statesList[mCurrentState] as NPCState).strStateName);
    }
}

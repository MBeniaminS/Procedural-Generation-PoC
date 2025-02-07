using UnityEngine;
using System.Collections;


public class PatrolState : NPCState {

	public Transform pathToFollow;
	private int currentTargetWP;

	private Vector3 currentTargetWPPos;
	private Path ptf;
	private bool bInitialised = false;
	private const float CLOSEENOUGH = 0.1f;
	// Use this for initialization
	void Start () {
		strStateName = "Patrol";
	}

	void OnEnable()
	{
		pl= GameObject.FindWithTag("Player").transform;
		Debug.Log ("++OnEnable function for PathFollow");
		bInitialised = false;
	}

	void Initialise()
	{
		ptf = pathToFollow.GetComponent<Path> ();
		//Debug.Log ("PathFollower:following path specified in " +ptf);
		
		currentTargetWP = -1;
		NextWayPoint ();
		bInitialised = true;
	}

	// Update is called once per frame
	void Update () {

		if (! bInitialised)
			Initialise ();

		//head towards target waypoint
		//if closer than small number then switch to next waypoint
		FaceTowards (currentTargetWPPos);
		MoveForwards ();

		//check if we are there yet
		if (GetDistanceToTarget (currentTargetWPPos) < CLOSEENOUGH) 
		{
			//Debug.Log("PathFollower:arrived at waypoint#" + currentTargetWP);
			NextWayPoint();
		}
	

		if ((GetIfPlayerWithinRange (5)) && (Mathf.Abs(GetAngleToPlayer()) < 45))
		{
			myParentFSM.SetState("Persue");
		}
	}

	void NextWayPoint()
	{
		currentTargetWP++;
		ptf = pathToFollow.GetComponent<Path> ();
		int numWayPoints = ptf.GetNumWaypoints();
		Debug.Log ("PathFollower:waypoints found:" + numWayPoints);
		if (currentTargetWP >= numWayPoints) 
		{
			//wrap back to initial waypoint
			currentTargetWP=0;
		}
		//Debug.Log ("PathFollower:attempting to retreive wp#" + currentTargetWP);
		currentTargetWPPos = ptf.GetPositionOfWP(currentTargetWP);
		//Debug.Log ("PathFollower:aiming for target#" + currentTargetWPPos);
	}



	void OnGUI()
	{
		//display current target
		GUI.Label(new Rect(10, 10, 300, 20), "Waypoint Target:" +currentTargetWP + " at " + currentTargetWPPos );
	}
}

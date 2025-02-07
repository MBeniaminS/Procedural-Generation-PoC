public class EvadeState : NPCState
{

    // Use this for initialization
    void Start()
    {
        strStateName = "Evade";
    }

    // Update is called once per frame
    void Update()
    {
        if (pl)
        {
            FaceAwayFromPlayer();
        }
        //move forwards
        MoveForwards();
    }
}

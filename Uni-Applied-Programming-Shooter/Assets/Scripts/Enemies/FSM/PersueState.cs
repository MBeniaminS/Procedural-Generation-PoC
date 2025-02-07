public class PersueState : NPCState
{


    // Use this for initialization
    void Start()
    {
        strStateName = "Persue";
    }


    // Update is called once per frame
    void Update()
    {
        if (pl)
        {
            //transform.LookAt (pl);
            FaceTowards(pl.position);
        }
        //move forwards
        MoveForwards();
        //this.transform.Translate (0, 0, mSpeed * Time.deltaTime, Space.Self);
    }
}

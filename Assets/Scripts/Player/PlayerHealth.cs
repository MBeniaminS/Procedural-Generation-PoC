using UnityEngine;

public class PlayerHealth : CharacterHealth
{
    [SerializeField] float delayForSceneRestart;

    #region Unity Callbacks
    // UNCOMMENT BELOW IF METHODS ARE TO BE EXTENDED/CHANGED

    //protected override void Start()
    //{
    //    base.Start();
    //}

    // UNCOMMENT BELOW IF METHODS ARE TO BE EXTENDED/CHANGED

    //protected override void Update()
    //{
    //    base.Update();
    //}

    #endregion Unity Callbacks

    #region Methods

    //protected override void Damage(float damage)
    //{
    //    base.Damage(damage);
    //}

    public override void DestroySelf()
    {
        MapGenerationManager.Instance.StartDelayedSceneRestart(delayForSceneRestart);
        base.DestroySelf();
    }

    #endregion Interface Methods

}

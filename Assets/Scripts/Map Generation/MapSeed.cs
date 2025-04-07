//using UnityEngine;

//public class MapSeed : MonoBehaviour
//{
//    [SerializeField] bool useFixedSeed = false;
//    [SerializeField] int fixedSeed;

//    [SerializeField] int usedSeed;

//    public static MapSeed Instance;

//    #region Unity Methods
//    private void Awake()
//    {
//        Instance = this;
//        if (useFixedSeed)
//        {
//            usedSeed = fixedSeed;
//        }
//        else
//        {
//            usedSeed = UnityEngine.Random.Range(0, int.MaxValue);
//        }
//        // Sets the random seed.
//        UnityEngine.Random.InitState(usedSeed);
//    }
//    #endregion
//}

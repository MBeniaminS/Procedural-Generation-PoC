using UnityEngine;

public class MapSeed : MonoBehaviour
{
    [SerializeField] bool useFixedSeed = false;
    [SerializeField] int fixedSeed;

    [SerializeField] int usedSeed;

    public static MapSeed Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (useFixedSeed)
        {
            usedSeed = fixedSeed;
        }
        else
        {
            usedSeed = UnityEngine.Random.Range(0, int.MaxValue);
        }

        UnityEngine.Random.InitState(usedSeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

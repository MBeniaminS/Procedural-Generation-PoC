using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float startingHealth = 100f;
    public float m_currentHealth;

    // Particle prefabs that are instantiated when damaged and destroyed respectively
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject destroyPrefab;

    private InputAction jumpAction;


    protected virtual void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");

        m_currentHealth = startingHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void Damage(float damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth <= 0)
        {
            DestroySelf();
        }
    }

    public virtual void DestroySelf()
    {
        SpawnPrefabAtPoint.instance.SpawnPrefab(destroyPrefab, transform.position, null);
        Destroy(gameObject);
    }
}

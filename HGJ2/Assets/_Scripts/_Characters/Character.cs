using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Character : MonoBehaviour, IDamageable
{
    public CharacterControler controller { get; protected set; }
    public CharacterInventory inventory { get; protected set; }
    public Rigidbody rb { get; protected set; }

    public MeshRenderer mainMesh;

    protected UIHealthPanel healthPanel;

    [HideInPlayMode]
    public int maxHealth = 2;
    [HideInEditorMode]
    public int currentHealth;
    public bool immortal;

    public Vector3 hitDirection { get; protected set; }
    [HideInInspector]
    public bool gotHit;
    public float timeOfStunEnd { get; protected set; }

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterControler>();
        inventory = GetComponent<CharacterInventory>();
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        timeOfStunEnd = 0;

        CreateHealthPanel();
    }

    protected virtual void CreateHealthPanel()
    {
        healthPanel.SetUp(this);
    }

    public virtual void GetDamage(int power)
    {
        if (immortal) return;
        ParticleSystem bloodEffect = Instantiate(GameManager.Prefabs.particles.bloodEffect,
            transform).GetComponent<ParticleSystem>();
        bloodEffect.transform.ResetLocal();
        Destroy(bloodEffect.gameObject, bloodEffect.main.duration);

        SetHealth(Mathf.Clamp(currentHealth - power, 0, currentHealth));
    }
    
    public virtual void GetHealed(int power)
    {
        SetHealth(Mathf.Clamp(currentHealth + power, currentHealth, maxHealth));
    }

    public void GetHit(Vector3 point, bool melee)
    {
        Stun(melee);
        gotHit = true;
        hitDirection = (transform.position - point).normalized;
    }

    public virtual void Stun(bool melee = false)
    {
    }

    protected void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        healthPanel.UpdatePanel();
        if (currentHealth == 0) Die();
    }

    protected virtual void Die()
    {
        if (immortal) return;
        inventory.Drop();

        if(mainMesh)
        {
            mainMesh.transform.SetParent(GameManager.orphanParent);
            Rigidbody meshRB = mainMesh.GetComponent<Rigidbody>();
            if (meshRB)
            {
                meshRB.isKinematic = false;
                meshRB.useGravity = true;
                meshRB.angularVelocity = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20),
                    Random.Range(-20, 20));
                meshRB.GetComponent<Collider>().enabled = true;
                meshRB.gameObject.AddComponent<MaterialColorBlinker>();
            }
            Destroy(mainMesh.gameObject, 1f);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterInventory : MonoBehaviour, ITriggerCollectorReaction<ItemObject>
{
    public Character character { get; protected set; }
    public CharacterControler controller { get; protected set; }
    public AudioSource punchSoundAudioSource { get; protected set; }

    public ItemTriggerCollector itemTriggerCollector { get; private set; }

    public Transform itemPoint;

    [HideInPlayMode][AssetsOnly]
    public GameObject weaponPrefab;
    [ShowIf("weaponPrefab")]
    public bool respawnWeaponOnceLost;
    private bool respawningWeapon;
    private float timeOfLastWeaponSpawn;

    public ItemObject grabbedItem { get; private set; }
    protected float timeOfLastItemGrab;
    [HideIf("respawnWeaponOnceLost")]
    public bool canGrabItems = true;
    public bool canDropItems = true;

    public float meleeAttackCooldown = 1f;
    private float timeOfNextMeleeAttack;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        controller = GetComponent<CharacterControler>();
        punchSoundAudioSource = GetComponent<AudioSource>();
        
        itemTriggerCollector = GetComponentInChildren<ItemTriggerCollector>();
    }

    private void Start()
    {
        if (weaponPrefab) SpawnItemAtStart();

        timeOfNextMeleeAttack = -meleeAttackCooldown;
        timeOfLastItemGrab = -1;
    }

    protected void SpawnItemAtStart()
    {
        timeOfLastItemGrab = Time.timeSinceLevelLoad;
        respawningWeapon = false;
        ItemObject item = Instantiate(weaponPrefab).GetComponent<ItemObject>();
        DoGrabObject(item);
    }

    private void SetGrabbedItem(ItemObject item)
    {
        grabbedItem = item;
        if(!grabbedItem)
            if (respawnWeaponOnceLost) RespawnWeapon();
    }

    public bool TryToGrabObject()
    {
        if (!canGrabItems || respawnWeaponOnceLost) return false;
        ItemObject itemToGrab = itemTriggerCollector.LastObjectInTrigger();
        if (!itemToGrab) return false;

        DoGrabObject(itemToGrab);
        timeOfLastItemGrab = Time.timeSinceLevelLoad;
        return true;
    }

    protected void DoGrabObject(ItemObject item)
    {
        if (item.grabbed) return;
        itemTriggerCollector.RemoveFromTrigger(item);

        SetGrabbedItem(item);
        item.OnGrab(character);

        grabbedItem.transform.SetParent(itemPoint);
        grabbedItem.transform.localPosition = Vector3.zero;
        grabbedItem.transform.localRotation = Quaternion.identity;
    }

    public virtual void Use()
    {
        if (grabbedItem && Time.timeSinceLevelLoad > timeOfLastWeaponSpawn + 1)
        {
            bool used = grabbedItem.TryUse(controller.target ? controller.target.transform.position :
                Vector3.zero);
            if (!used) SetGrabbedItem(null);
        }
        else if (controller.TargetInMeleeDistance())
            MeleeAttack();
        else 
            TryToGrabObject();
    }

    public virtual void Throw()
    {
        if (!grabbedItem || grabbedItem.thrown) return;
        grabbedItem.OnThrow();
        SetGrabbedItem(null);
    }

    public virtual void Drop()
    {
        if (!canDropItems || !grabbedItem) return;
        grabbedItem.OnDrop();
        SetGrabbedItem(null);
    }

    public void DropStrong()
    {
        if (!grabbedItem) return;
        ItemObject itemToDrop = grabbedItem;
        Drop();
        itemToDrop.ThrowWithPower(.4f);
    }

    private void RespawnWeapon()
    {
        if (respawningWeapon) return;
        respawningWeapon = true;
        Invoke("SpawnItemAtStart", 1);
    }

    public virtual bool MeleeAttack()
    {
        if (timeOfNextMeleeAttack > Time.timeSinceLevelLoad) return false;

        punchSoundAudioSource.Play();
        controller.target.GetDamage(1);
        controller.target.GetHit(transform.position, true);
        timeOfNextMeleeAttack = Time.timeSinceLevelLoad + meleeAttackCooldown;
        return true;
    }

    public virtual void OnTriggerEnterReaction(ItemObject t) {}
    public virtual void OnTriggerExitReaction(ItemObject t) {}
}

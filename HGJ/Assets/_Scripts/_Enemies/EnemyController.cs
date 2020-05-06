using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterControler
{
    private static bool paused;
    public static List<EnemyController> Enemies;

    public Dictionary<EnemyBehaviour.EnemyBehaviourType, EnemyBehaviour> behaviours =
        new Dictionary<EnemyBehaviour.EnemyBehaviourType, EnemyBehaviour>();
    private EnemyBehaviour currentBehaviour;

    public float viewRange = 15;

    public GameObject targetAcquiredIcon;

    protected override void Awake()
    {
        base.Awake();

        foreach (EnemyBehaviour behaviour in GetComponents<EnemyBehaviour>())
            behaviours.Add(behaviour.behaviourType, behaviour);

        SetBehaviour(EnemyBehaviour.EnemyBehaviourType.Regular);
    }

    private void Start()
    {
        HideTargetAcquiredIcon();
    }

    private void OnEnable()
    {
        if (Enemies == null) Enemies = new List<EnemyController>();
        if (!Enemies.Contains(this)) Enemies.Add(this);
    }

    private void OnDisable()
    {
        if (Enemies.Contains(this)) Enemies.Remove(this);
    }

    protected void Update()
    {
        if (Time.timeSinceLevelLoad < character.timeOfStunEnd || paused) return;

        currentBehaviour.OnBehaviourUpdate();
    }

    public void LookForTarget()
    {
        if(GameManager.playerController && Vector3.Distance(transform.position, 
            GameManager.playerController.character.transform.position) < viewRange)
            SetTarget(GameManager.playerController.character);
    }

    public override void SetTarget(Character newTarget)
    {
        base.SetTarget(newTarget);
        if (target)
        {
            SetBehaviour(EnemyBehaviour.EnemyBehaviourType.Triggered);
            CallAllies();
            StartCoroutine(ShowTargetAcquiredIcon());
        }
        else SetBehaviour(EnemyBehaviour.EnemyBehaviourType.Regular);
    }

    protected IEnumerator ShowTargetAcquiredIcon()
    {
        targetAcquiredIcon.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        HideTargetAcquiredIcon();
    }

    protected void HideTargetAcquiredIcon()
    {
        targetAcquiredIcon.SetActive(false);
    }

    public void SetBehaviour(EnemyBehaviour.EnemyBehaviourType behaviourType)
    {
        if(currentBehaviour)
        {
            if (behaviourType == currentBehaviour.behaviourType) return;
            currentBehaviour.OnBehaviourEnd();
        }

        currentBehaviour = behaviours[behaviourType];
        currentBehaviour.OnBehaviourStart();
    }

    public static void PauseAll()
    {
        paused = true;
    }

    public static void UnpauseAll()
    {
        paused = false;
    }

    public static void ResetTargets()
    {
        foreach (EnemyController enemy in Enemies)
            enemy.SetTarget(null);
    }

    public static void KillAll()
    {
        for(int i = Enemies.Count - 1; i >= 0; i--)
        {
            Enemies[i].character.GetDamage(100);
        }
    }

    public void CallAllies()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}

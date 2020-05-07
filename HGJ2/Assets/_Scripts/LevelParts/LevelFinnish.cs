using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinnish : MonoBehaviour
{
    public enum LevelFinnishCondition { None, KillAll };
    public Transform nextPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.playerController.transform)
        {
            if (!nextPoint)
                LevelFlowControl.NextLevel();
            else
                StartCoroutine(NextPartCoroutine());
        }
    }

    private IEnumerator NextPartCoroutine()
    {
        EnemyController.PauseAll();
        yield return StartCoroutine(GameManager.OnLevelPartFinnishCoroutine());
        GameManager.playerController.movement.agent.Warp(nextPoint.position);
        GameManager.playerController.transform.rotation = nextPoint.rotation;
        EnemyController.ResetTargets();
        yield return StartCoroutine(GameManager.OnLevelStartCoroutine());
        EnemyController.UnpauseAll();
    }
}

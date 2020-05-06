using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "_Kathlar/_Databases/Layers")]
public class GameManagerDatabaseLayers : GameManagerDatabase
{
    [SerializeField]
    public LayerMask TargetLayer;
    [SerializeField]
    public LayerMask GroundLayer;
    [SerializeField]
    public LayerMask itemsLayer;
}

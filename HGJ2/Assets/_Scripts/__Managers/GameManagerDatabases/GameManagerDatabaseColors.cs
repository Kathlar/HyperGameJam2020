using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "_Kathlar/_Databases/Colors")]
public class GameManagerDatabaseColors : GameManagerDatabase
{
    [SerializeField]
    public Color itemColor = Color.white;
    [SerializeField]
    public Color enemyColorDistant = Color.red;
    [SerializeField]
    public Color enemyColorClose = Color.red;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "_Kathlar/_Databases/Prefabs")]
public class GameManagerDatabasePrefab : GameManagerDatabase
{
    [System.Serializable]
    public class Particles
    {
        [SerializeField]
        public GameObject stunEffect;
        [SerializeField]
        public GameObject bloodEffect;
    }

    public Particles particles;

    [System.Serializable]
    public class UI
    {
        [SerializeField]
        public GameObject healthPanel;
    }

    public UI ui;
}
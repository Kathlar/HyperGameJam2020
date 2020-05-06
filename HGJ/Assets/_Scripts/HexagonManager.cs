using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : Singleton<HexagonManager>
{
    protected Dictionary<Vector3, Hexagon> hexagons =
        new Dictionary<Vector3, Hexagon>();
    public static Dictionary<Vector3, Hexagon> Hexagons { get { return Instance.hexagons; } }

    public static List<Vector3> cubeDirections = new List<Vector3>()
    {
        new Vector3(1,-1,0), new Vector3(1,0,-1),
        new Vector3(0,1,-1), new Vector3(-1,1,0),
        new Vector3(-1,0,1), new Vector3(0,-1,1)
    };

    protected override void Awake()
    {
        base.Awake();
        foreach(Hexagon hexagon in GetComponentsInChildren<Hexagon>())
            AddHexagon(hexagon);
    }

    public static void AddHexagon(Hexagon hex)
    {
        hex.SetUpCoordinates();
        Hexagons.Add(hex.cubeCoordinates, hex);
    }

    public static List<Hexagon> GetNeighbours(Hexagon hex)
    {
        List<Hexagon> neighbours = new List<Hexagon>();
        foreach (Vector3 direction in cubeDirections)
        {
            if (Hexagons.ContainsKey(hex.cubeCoordinates + direction))
                neighbours.Add(Hexagons[hex.cubeCoordinates + direction]);
        }

        return neighbours;
    }
}

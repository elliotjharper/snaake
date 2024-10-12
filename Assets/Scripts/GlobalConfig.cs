using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "Scriptable Menu/Global Menu", order = 1)]
public class GlobalConfig : ScriptableObject
{
    public float GridSizeAbsolute;
    public int GridColumns;
    public float CellSize;

    public Vector3 GetRandomPosition()
    {
        var x = GetRandomGridCoordinate();
        var y = GetRandomGridCoordinate();
        return GridToWorldPosition(x, y);
    }

    public int GetRandomGridCoordinate()
    {
        return Random.Range(0, GridColumns - 1);
    }

    public Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3((x + 0.5f) * CellSize, (y + 0.5f) * CellSize);
    }
}
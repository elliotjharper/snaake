using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "Scriptable Menu/Global Menu", order = 1)]
public class GlobalConfig : ScriptableObject {
    public int GridSizeAbsolute;
    public int GridColumns;
    public int CellSize;
}
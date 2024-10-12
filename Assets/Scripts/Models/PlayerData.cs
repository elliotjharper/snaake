using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int Id;
    public bool Alive;
    public Vector3 HeadPosition;
    public List<Vector3> SegmentPositions;
    public int Score;
    public Vector3 Bearing;
    public Vector3 NextBearing;
    public int PlayerSpeed;
    public Color32 Colour;
}

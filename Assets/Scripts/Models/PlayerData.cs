using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : IEquatable<PlayerData>
{
    public int Id;
    public bool Alive = false;
    public Vector3 HeadPosition;
    public List<Vector3> SegmentPositions = new List<Vector3>();
    public int Score = 0;
    public Vector3 Bearing;
    public Vector3 NextBearing;
    //public int PlayerSpeed;
    public Color32 Colour;

    public bool Equals(PlayerData other)
    {
        if (Id != other.Id) return false;
        if (Alive != other.Alive) return false;
        if (HeadPosition != other.HeadPosition) return false;
        if (!SegmentPositions.Equals(other.SegmentPositions)) return false;
        if (Score != other.Score) return false;
        if (Bearing != other.Bearing) return false;
        if (NextBearing != other.NextBearing) return false;
        //if (PlayerSpeed != other.PlayerSpeed) return false;
        if (Colour.ToString() != other.Colour.ToString()) return false;

        return true;
    }

}

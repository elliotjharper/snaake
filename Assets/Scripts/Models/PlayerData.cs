using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerData : IEquatable<PlayerData>, INetworkSerializable {
    public int Id;
    public bool Alive = true;
    public Vector3 HeadPosition;
    public List<Vector3> SegmentPositions = new();
    public int Score = 0;
    public Vector3 Bearing;
    public Vector3 NextBearing;
    //public int PlayerSpeed;
    public Color32 Colour;

    public bool Equals(PlayerData other) {
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

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref Alive);
        serializer.SerializeValue(ref HeadPosition);
        serializer.SerializeValue(ref Score);
        serializer.SerializeValue(ref Bearing);
        serializer.SerializeValue(ref NextBearing);
        serializer.SerializeValue(ref Colour);

        var count = SegmentPositions.Count;
        serializer.SerializeValue(ref count);

        for (int i = 0; i < count; i++) {
            var position = SegmentPositions[i];
            serializer.SerializeValue(ref position);
        }
    }
}
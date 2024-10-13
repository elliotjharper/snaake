using System;
using Unity.Netcode;

public class GameSettings : IEquatable<GameSettings>, INetworkSerializable {
    public float GridSize;
    //public int PlayerDefaultSpeed;

    public bool Equals(GameSettings other) {
        if (GridSize != other.GridSize) return false;
        //if (PlayerDefaultSpeed != other.PlayerDefaultSpeed) return false;

        return true;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref GridSize);
    }
}
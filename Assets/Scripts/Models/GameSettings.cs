using System;

public class GameSettings : IEquatable<GameSettings>
{
    public int GridSize;
    public int PlayerDefaultSpeed;

    public bool Equals(GameSettings other)
    {
        if (GridSize != other.GridSize) return false;
        if (PlayerDefaultSpeed != other.PlayerDefaultSpeed) return false;

        return true;
    }
}
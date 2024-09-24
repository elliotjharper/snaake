using System;
using System.Collections.Generic;

public struct CoOrd : IEquatable<CoOrd>
{
    public int X;
    public int Y;

    public bool Equals(CoOrd other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj is CoOrd)
        {
            return Equals((CoOrd)obj);
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Use a prime number strategy for better distribution of hash values
        int hash = 17;
        hash = hash * 23 + X.GetHashCode();
        hash = hash * 23 + Y.GetHashCode();
        return hash;
    }

    public static bool operator ==(CoOrd left, CoOrd right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CoOrd left, CoOrd right)
    {
        return !(left == right);
    }
}

public class PlayerData
{
    public CoOrd Head;
    public List<CoOrd> Segments;
}

public class DataStore
{
    public CoOrd Food;
    public Dictionary<int, PlayerData> Players;
    public void Save()
    {
        // persist wherever...
    }
}

interface IHasData<T>
{
    T ReadFromStore();
    void WriteToStore(T data);
}

public class Player
{
    public int Index;
    private DataStore Store;
    private PlayerData Data
    {
        get
        {
            return Store.Players[Index];
        }
    }

    public Player(int index)
    {
        Index = index;
    }

    public void Move(CoOrd newCoOrd)
    {
        // TODO: decide whether to pass ateFood in or look at the Store and do the work here....
        // TODO: also check if we would be hitting a wall or another player and decide where that should occur
        Data.Head = newCoOrd;
        Data.Segments.Insert(0, newCoOrd);
        Store.Save();
    }

    public bool CoversCoOrd(CoOrd testCoOrd)
    {
        if (Data.Head == testCoOrd)
        {
            return true;
        }

        if (Data.Segments.Contains(testCoOrd))
        {
            return true;
        }

        return false;
    }
}
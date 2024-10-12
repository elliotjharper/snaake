using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// public enum HostState {
//     WaitingForPlayers,


// }

public class HostManager : MonoBehaviour
{
    public bool IsOwner = true;
    public float TickInterval;
    public DataStore Store;
    public GlobalConfig Config;

    private float timeSinceLastTick = 0;

    void Awake()
    {
        // startup the data store

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastTick += Time.deltaTime;

        if (timeSinceLastTick >= TickInterval)
        {
            Tick();
            timeSinceLastTick -= TickInterval;
        }
    }

    void Tick()
    {
        if (!AllPlayersReady())
        {
            return;
        }

        MoveAllLivingPlayers();
        HandlePlayerCrashes();
        HandleFoodConsumption();
        HandleGameOver();

        // has anyone who is alive died

        // if anyone is on top of the food
        // award the player a score
        // move the food to a new location not colliding 
    }

    private bool AllPlayersReady()
    {
        return Store.Players.All(p => p.NextBearing != Vector3.zero);
    }

    private void MoveAllLivingPlayers()
    {
        foreach (var player in Store.Players.Where(p => p.Alive))
        {
            player.Bearing = player.NextBearing;
            var newPosition = transform.position + player.NextBearing * Config.CellSize;

            player.SegmentPositions.Insert(0, newPosition);

            if (!CollidesWithFood(newPosition))
            {
                player.SegmentPositions.RemoveAt(player.SegmentPositions.Count() - 1);
            }
        }
    }

    private void HandlePlayerCrashes()
    {
        foreach (var player in Store.Players.Where(p => p.Alive))
        {
            if (IsOutsideGrid(player.HeadPosition) || CollidesWithSelfOrOtherPlayers(player))
            {
                player.Alive = false;
            }
        }
    }

    private void HandleFoodConsumption()
    {
        foreach (var player in Store.Players.Where(p => p.Alive))
        {
            if (player.HeadPosition == Store.FoodPosition)
            {
                player.Score++;

                while (CollidesWithAnyPlayer(Store.FoodPosition))
                {
                    Store.FoodPosition = Config.GetRandomPosition();
                }
            }
        }
    }

    private void HandleGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    private bool CollidesWithFood(Vector3 position)
    {
        return Store.FoodPosition == position;
    }

    private bool IsOutsideGrid(Vector3 position)
    {
        if (position.x < 0 || position.y < 0)
        {
            return true;
        }

        var firstInvalidPosition = Config.GridSizeAbsolute;
        if (position.x >= firstInvalidPosition || position.y >= firstInvalidPosition)
        {
            return true;
        }

        return false;
    }

    private bool CollidesWithPlayer(PlayerData target, Vector3 arrow, bool checkHead)
    {
        if (checkHead && target.HeadPosition == arrow)
        {
            return true;
        }

        if (target.SegmentPositions.Contains(arrow))
        {
            return true;
        }

        return false;
    }

    private bool CollidesWithSelfOrOtherPlayers(PlayerData movingPlayer)
    {
        foreach (var player in Store.Players)
        {
            var isMovingPlayer = player.Id == movingPlayer.Id;
            if (CollidesWithPlayer(player, movingPlayer.HeadPosition, !isMovingPlayer))
            {
                return true;
            }
        }

        return false;
    }

    private bool CollidesWithAnyPlayer(Vector3 targetPosition)
    {
        foreach (var player in Store.Players)
        {
            if (CollidesWithPlayer(player, targetPosition, true))
            {
                return true;
            }
        }

        return false;
    }
}

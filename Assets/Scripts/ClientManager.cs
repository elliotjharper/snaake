using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using Unity.Netcode;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : NetworkBehaviour
{
    public GlobalConfig Config;
    public DataStore Store;
    public HostManager HostManager;

    public GameObject SquarePrefab;
    public GameObject PlayerPrefab;
    public GameObject SegmentPrefab;
    public GameObject FoodPrefab;
    public TextMeshProUGUI Player1Score;
    public TextMeshProUGUI Player2Score;

    List<GameObject> playersInstances = new();
    List<GameObject> segmentInstances = new();
    int currentSegmentsIndex;
    GameObject foodInstance;

    bool clientInitialised = false;

    bool TryGetDependecies()
    {
        HostManager = FindObjectOfType<HostManager>();
        if (HostManager)
        {
            Store = HostManager.Store;
            return true;
        }

        return false;
    }

    void Initialise()
    {
        // Calculate the cell size
        Config.CellSize = Config.GridSizeAbsolute / Config.GridColumns;

        // Create the grid
        for (var xIndex = 0; xIndex < Config.GridColumns; xIndex++)
        {
            for (var yIndex = 0; yIndex < Config.GridColumns; yIndex++)
            {
                var instance = Instantiate(SquarePrefab, position: GridToWorldPosition(xIndex, yIndex), Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
            }
        }

        foreach (var playerData in Store.Players.Value)
        {
            var player = Instantiate(PlayerPrefab, position: playerData.HeadPosition, Quaternion.identity, transform);
            player.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
            player.GetComponent<SpriteRenderer>().color = playerData.Colour;
            playersInstances.Add(player);
        }

        // Create the food
        foodInstance = Instantiate(FoodPrefab, position: Store.FoodPosition.Value, Quaternion.identity, transform);
        foodInstance.transform.localScale = new Vector3(Config.CellSize * 0.6f, Config.CellSize * 0.6f);

        clientInitialised = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Check this is the client manager we own
        if (!IsOwner) return;

        if (!HostManager && !TryGetDependecies()) return;

        // Check we have a valid host manager and store
        if (!HostManager.IsSpawned || !Store.IsSpawned) return;

        // Check the store has been initialised
        if (Store.Players.Value == null || Store.Players.Value.Count == 0) return;

        if (!clientInitialised)
        {
            Initialise();
        }

        var playerId = NetworkManager.Singleton.IsHost ? 0 : 1;
        var bearing = Store.Players.Value[playerId].Bearing;

        if (Input.GetKey(KeyCode.W) && bearing != Vector3.down)
        {
            SendBearing(Vector3.up, playerId);
        }
        else if (Input.GetKey(KeyCode.S) && bearing != Vector3.up)
        {
            SendBearing(Vector3.down, playerId);
        }
        else if (Input.GetKey(KeyCode.A) && bearing != Vector3.right)
        {
            SendBearing(Vector3.left, playerId);
        }
        else if (Input.GetKey(KeyCode.D) && bearing != Vector3.left)
        {
            SendBearing(Vector3.right, playerId);
        }

        foodInstance.transform.position = Store.FoodPosition.Value;

        // Player1Score.text = store.Players.Value[0].Score.ToString();
        // Player2Score.text = store.Players.Value[1].Score.ToString();

        currentSegmentsIndex = 0;
        segmentInstances.ForEach(segment => segment.SetActive(false));

        //Debug.Log(Store.Players.Value[0].Id.ToString() + Store.Players.Value[0].HeadPosition.ToString());

        foreach (var playerData in Store.Players.Value.Where(p => p.Alive))
        {
            var player = playersInstances[playerData.Id];
            player.transform.position = playerData.HeadPosition;

            foreach (var segmentPosition in playerData.SegmentPositions)
            {
                if (currentSegmentsIndex >= segmentInstances.Count)
                {
                    var newSegment = Instantiate(SegmentPrefab);
                    segmentInstances.Add(newSegment);
                }

                var segment = segmentInstances[currentSegmentsIndex];
                segment.transform.position = segmentPosition;
                segment.GetComponent<SpriteRenderer>().color = playerData.Colour;
                segment.SetActive(true);

                currentSegmentsIndex++;
            }
        }
    }

    Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3((x + 0.5f) * Config.CellSize, (y + 0.5f) * Config.CellSize);
    }

    void SendBearing(Vector3 bearing, int playerId)
    {
        HostManager.SetPlayerBearingServerRpc(new PlayerBearing() { PlayerId = playerId, Bearing = bearing });
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"ClientManager - OnNetworkSpawn - IsHost={NetworkManager.Singleton.IsHost}");
    }
}
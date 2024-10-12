using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class ClientManager : NetworkBehaviour
{
    public DataStore Store;
    public GlobalConfig Config;

    public GameObject SquarePrefab;
    public GameObject PlayerPrefab;
    public GameObject SegmentPrefab;
    public GameObject FoodPrefab;
    public TextMeshProUGUI Player1Score;
    public TextMeshProUGUI Player2Score;

    List<GameObject> playersInstances;
    List<GameObject> segmentInstances;
    int currentSegmentsIndex;
    GameObject foodInstance;

    void Start()
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
            player.GetComponent<Player>().Store = Store;
            playersInstances.Add(player);
        }

        // Create the food
        foodInstance = Instantiate(FoodPrefab, position: Store.FoodPosition.Value, Quaternion.identity, transform);
        foodInstance.transform.localScale = new Vector3(Config.CellSize * 0.6f, Config.CellSize * 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        foodInstance.transform.position = Store.FoodPosition.Value;

        Player1Score.text = Store.Players.Value[0].Score.ToString();
        Player2Score.text = Store.Players.Value[1].Score.ToString();

        currentSegmentsIndex = 0;
        segmentInstances.ForEach(segment => segment.SetActive(false));

        foreach (var playerData in Store.Players.Value)
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
}
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region GameObjects
    public GameObject Square;
    public GameObject PlayerHead;
    public GameObject Food;
    #endregion

    public GlobalConfig Config;
    public float TickInterval;
    float timeSinceLastTick = 0;
    PlayerHead player1;
    GameObject foodInstance;

    Vector3 foodPosition;

    // Start is called before the first frame update
    void Start() {
        // Calculate the cell size
        Config.CellSize = Config.GridSizeAbsolute / Config.GridColumns;

        // Create the grid
        for (var xIndex = 0; xIndex < Config.GridColumns; xIndex++) {
            for (var yIndex = 0; yIndex < Config.GridColumns; yIndex++) {
                var instance = Instantiate(Square, position : GridToWorldPosition(xIndex, yIndex), Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
            }
        }

        //create the player

        var player = Instantiate(PlayerHead, position : GetRandomPosition(), Quaternion.identity, transform);
        player.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
        player1 = player.GetComponent<PlayerHead>();

        // Create the food
        foodInstance = Instantiate(Food, position : GetRandomPosition(), Quaternion.identity, transform);
        foodInstance.transform.localScale = new Vector3(Config.CellSize * 0.6f, Config.CellSize * 0.6f);
    }

    // Update is called once per frame
    void Update() {
        timeSinceLastTick += Time.deltaTime;

        if (timeSinceLastTick >= TickInterval) {
            Tick();
            timeSinceLastTick -= TickInterval;
        }
    }

    void Tick() {
        player1.Tick();

        if (player1.transform.position == foodInstance.transform.position) {
            // Add tail to snake

            RepositionFood();
        }
    }

    void RepositionFood() {
        foodInstance.transform.position = GetRandomPosition();
    }

    Vector3 GetRandomPosition() {
        var x = GetRandomGridCoordinate();
        var y = GetRandomGridCoordinate();
        return GridToWorldPosition(x, y);
    }

    int GetRandomGridCoordinate() {
        return Random.Range(0, Config.GridColumns - 1);
    }

    Vector3 GridToWorldPosition(int x, int y) {
        return new Vector3((x + 0.5f) * Config.CellSize, (y + 0.5f) * Config.CellSize);
    }
}
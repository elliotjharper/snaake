using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GameObjects
    public GameObject Square;
    public GameObject Player1Template;
    public GameObject Player2Template;
    public GameObject Food;
    #endregion

    public GlobalConfig Config;
    public float TickInterval;
    float timeSinceLastTick = 0;
    PlayerHead player1;
    PlayerHead player2;
    GameObject foodInstance;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the cell size
        Config.CellSize = Config.GridSizeAbsolute / Config.GridColumns;

        // Create the grid
        for (var xIndex = 0; xIndex < Config.GridColumns; xIndex++)
        {
            for (var yIndex = 0; yIndex < Config.GridColumns; yIndex++)
            {
                var instance = Instantiate(Square, position: GridToWorldPosition(xIndex, yIndex), Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
            }
        }

        //create the player
        var player1GameObject = Instantiate(Player1Template, position: GetRandomPosition(), Quaternion.identity, transform);
        player1GameObject.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
        player1 = player1GameObject.GetComponent<PlayerHead>();

        var player2GameObject = Instantiate(Player2Template, position: GetRandomPosition(), Quaternion.identity, transform);
        player2GameObject.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
        player2 = player2GameObject.GetComponent<PlayerHead>();

        // Create the food
        foodInstance = Instantiate(Food, position: GetRandomPosition(), Quaternion.identity, transform);
        foodInstance.transform.localScale = new Vector3(Config.CellSize * 0.6f, Config.CellSize * 0.6f);
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
        if (player1.MoveOnTick(foodInstance.transform.position))
        {
            RepositionFood();
        }
        if (player2.MoveOnTick(foodInstance.transform.position))
        {
            RepositionFood();
        }
    }

    void RepositionFood()
    {
        foodInstance.transform.position = GetRandomPosition();
    }

    Vector3 GetRandomPosition()
    {
        var x = GetRandomGridCoordinate();
        var y = GetRandomGridCoordinate();
        return GridToWorldPosition(x, y);
    }

    int GetRandomGridCoordinate()
    {
        return Random.Range(0, Config.GridColumns - 1);
    }

    Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3((x + 0.5f) * Config.CellSize, (y + 0.5f) * Config.CellSize);
    }
}
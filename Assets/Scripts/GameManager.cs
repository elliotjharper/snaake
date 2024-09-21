using UnityEngine;

public class GameManager : MonoBehaviour {
    #region GameObjects
    public GameObject Square;
    public GameObject PlayerHead;
    #endregion

    public GlobalConfig Config;
    public Vector3 foodPosition;
    public float TickInterval;
    float timeSinceLastTick = 0;
    PlayerHead player1;

    // Start is called before the first frame update
    void Start() {
        // Calculate the cell size
        Config.CellSize = Config.GridSizeAbsolute / Config.GridColumns;

        // Create the grid
        for (var xIndex = 0; xIndex < Config.GridColumns; xIndex++) {
            for (var yIndex = 0; yIndex < Config.GridColumns; yIndex++) {
                var instance = Instantiate(Square, position : new Vector3((xIndex + 0.5f) * Config.CellSize, (yIndex + 0.5f) * Config.CellSize), Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
            }
        }

        //create the player
        var middle = (Mathf.Floor(Config.GridColumns / 2f) + 0.5f) * Config.CellSize;
        var player = Instantiate(PlayerHead, position : new Vector3(middle, middle, -1), Quaternion.identity, transform);
        player.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
        player1 = player.GetComponent<PlayerHead>();
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
    }
}
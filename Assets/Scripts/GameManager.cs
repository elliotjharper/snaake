using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region GameObjects
    public GameObject Square;
    public GameObject PlayerHead;
    #endregion

    public GlobalConfig Config;

    // Start is called before the first frame update
    void Start() {
        // Create the grid
        for (var xIndex = 0; xIndex < Config.GridSize; xIndex++) {
            for (var yIndex = 0; yIndex < Config.GridSize; yIndex++) {
                var instance = Instantiate(Square, position : new Vector3((xIndex + 0.5f) * Config.CellSize, (yIndex + 0.5f) * Config.CellSize), Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);
            }
        }

        //create the player
        var middle = (Config.GridSize / 2 + 0.5f) * Config.CellSize;
        var player = Instantiate(PlayerHead, position : new Vector3(middle, middle, -1), Quaternion.identity, transform);
        player.transform.localScale = new Vector3(Config.CellSize, Config.CellSize);

        // Scale to fit in viewport
        var scale = 10f / (Config.CellSize * Config.GridSize);
        transform.localScale = new Vector3(scale, scale, 1);
    }

    // Update is called once per frame
    void Update() {

    }
}
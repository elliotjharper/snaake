using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHead : MonoBehaviour {
    #region Config
    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public GlobalConfig Config;
    public string GameOverSceneName;

    #endregion

    Vector3 bearing = Vector3.zero;
    Vector3 nextBearing = Vector3.zero;

    // Start is called before the first frame update
    void Start() {

    }

    public void Tick() {
        bearing = nextBearing;

        if (bearing != Vector3.zero) {
            var newPosition = transform.position + bearing * Config.CellSize;

            if (isOutside(newPosition)) {
                // Also check we haven't collided with ourself
                SceneManager.LoadScene(GameOverSceneName);
                return;
            } else {
                transform.position += bearing * Config.CellSize;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(UpKey) && bearing != Vector3.down) {
            nextBearing = Vector3.up;
        } else if (Input.GetKey(DownKey) && bearing != Vector3.up) {
            nextBearing = Vector3.down;
        } else if (Input.GetKey(LeftKey) && bearing != Vector3.right) {
            nextBearing = Vector3.left;
        } else if (Input.GetKey(RightKey) && bearing != Vector3.left) {
            nextBearing = Vector3.right;
        }

    }

    bool isOutside(Vector3 newPosition) {
        if (newPosition.x < 0 || newPosition.y < 0) {
            return true;
        }

        var firstInvalidPosition = Config.GridSizeAbsolute;
        if (newPosition.x >= firstInvalidPosition || newPosition.y >= firstInvalidPosition) {
            return true;
        }

        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHead : MonoBehaviour
{
    #region Config
    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public GlobalConfig Config;
    public string GameOverSceneName;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3? directionVector = null;
        if (Input.GetKeyDown(UpKey))
        {
            directionVector = Vector3.up;
        }
        else if (Input.GetKeyDown(DownKey))
        {
            directionVector = Vector3.down;
        }
        else if (Input.GetKeyDown(LeftKey))
        {
            directionVector = Vector3.left;
        }
        else if (Input.GetKeyDown(RightKey))
        {
            directionVector = Vector3.right;
        }

        if (directionVector != null)
        {
            var newPosition = transform.position + directionVector.Value * Config.CellSize;

            if (isOutside(newPosition))
            {
                SceneManager.LoadScene(GameOverSceneName);
                return;
            }
            else
            {
                transform.position += directionVector.Value * Config.CellSize;
            }
        }
    }

    bool isOutside(Vector3 newPosition)
    {
        if (newPosition.x < 0 || newPosition.y < 0)
        {
            return true;
        }

        var firstInvalidPosition = Config.CellSize * Config.GridSize;
        if (newPosition.x >= firstInvalidPosition || newPosition.y >= firstInvalidPosition)
        {
            return true;
        }

        return false;
    }
}

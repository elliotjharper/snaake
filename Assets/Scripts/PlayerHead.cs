using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHead : MonoBehaviour
{
    public GameObject SnakeTail;

    #region Config
    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public GlobalConfig Config;
    public string GameOverSceneName;

    #endregion

    Vector3 bearing = Vector3.zero;
    public Vector3 nextBearing = Vector3.zero;
    SnakeTail child = null;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool MoveOnTick(Vector3 foodPosition)
    {
        if (dead)
        {
            return false;
        }

        bearing = nextBearing;

        if (bearing != Vector3.zero)
        {
            var newPosition = transform.position + bearing * Config.CellSize;

            if (isOutside(newPosition) || child?.NewPositionHitsTail(newPosition) == true)
            {
                dead = true;
                return false;
            }
            else
            {
                // store current position
                var lastPosition = transform.position;

                // move the head
                transform.position += bearing * Config.CellSize;

                // figure out if we ate da food
                var ateFood = transform.position == foodPosition;

                if (child)
                {
                    // start the recursive tail movements
                    child.Move(lastPosition, ateFood);
                }
                else if (ateFood)
                {
                    // create the first tail segment
                    var snakeTailGameObject = Instantiate(SnakeTail, position: lastPosition, quaternion.identity);
                    snakeTailGameObject.transform.localScale = new Vector3(Config.CellSize * 0.6f, Config.CellSize * 0.6f);
                    child = snakeTailGameObject.GetComponent<SnakeTail>();
                    var ourRenderer = GetComponent<SpriteRenderer>();
                    var childRenderer = snakeTailGameObject.GetComponent<SpriteRenderer>();
                    childRenderer.color = ourRenderer.color;
                }

                return ateFood;
            }
        }

        return false;
    }

    public int GetScore()
    {
        if (child != null)
        {
            return child.GetScore();
        }

        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(UpKey) && bearing != Vector3.down)
        {
            nextBearing = Vector3.up;
        }
        else if (Input.GetKey(DownKey) && bearing != Vector3.up)
        {
            nextBearing = Vector3.down;
        }
        else if (Input.GetKey(LeftKey) && bearing != Vector3.right)
        {
            nextBearing = Vector3.left;
        }
        else if (Input.GetKey(RightKey) && bearing != Vector3.left)
        {
            nextBearing = Vector3.right;
        }

    }

    bool isOutside(Vector3 newPosition)
    {
        if (newPosition.x < 0 || newPosition.y < 0)
        {
            return true;
        }

        var firstInvalidPosition = Config.GridSizeAbsolute;
        if (newPosition.x >= firstInvalidPosition || newPosition.y >= firstInvalidPosition)
        {
            return true;
        }

        return false;
    }
}
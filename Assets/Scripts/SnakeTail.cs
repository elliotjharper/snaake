using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeTail : MonoBehaviour
{
    private SnakeTail Child;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 newPosition, bool grow)
    {
        var lastPosition = transform.position;

        // move to new position
        transform.position = newPosition;

        if (Child != null)
        {
            Child.Move(lastPosition, grow);
            return;
        }

        if (grow)
        {
            var childGameObject = Instantiate(gameObject, position: lastPosition, quaternion.identity);
            Child = childGameObject.GetComponent<SnakeTail>();
        }
    }

    public bool NewPositionHitsTail(Vector3 newPosition)
    {
        if (newPosition == transform.position)
        {
            return true;
        }

        if (Child != null)
        {
            return Child.NewPositionHitsTail(newPosition);
        }

        return false;
    }

    public int GetScore(int current = 0)
    {
        if (Child != null)
        {
            return Child.GetScore(current + 1);
        }

        return current + 1;
    }
}

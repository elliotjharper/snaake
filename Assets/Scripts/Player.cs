using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public DataStore Store;
    public int PlayerId;
    NetworkVariable<Vector3> NextBearing = new(writePerm: NetworkVariableWritePermission.Owner);

    void Start()
    {
        PlayerId = IsOwner && IsHost ? 0 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        var bearing = Store.Players.Value[PlayerId].Bearing;

        if (Input.GetKey(KeyCode.W) && bearing != Vector3.down)
        {
            NextBearing.Value = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S) && bearing != Vector3.up)
        {
            NextBearing.Value = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.A) && bearing != Vector3.right)
        {
            NextBearing.Value = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) && bearing != Vector3.left)
        {
            NextBearing.Value = Vector3.right;
        }
    }
}
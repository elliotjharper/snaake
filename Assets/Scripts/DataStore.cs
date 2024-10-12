using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DataStore : NetworkBehaviour
{
    public NetworkVariable<Vector3> FoodPosition = new(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<List<PlayerData>> Players = new(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<GameSettings> GameSettings = new(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> GameOver = new(writePerm: NetworkVariableWritePermission.Server);
}

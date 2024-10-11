using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour {
    readonly NetworkVariable<Vector3> networkPosition = new(writePerm: NetworkVariableWritePermission.Owner);

    // Update is called once per frame
    void Update() {
        if (IsOwner) {
            networkPosition.Value = transform.position;
        } else {
            transform.position = networkPosition.Value;
        }
    }
}
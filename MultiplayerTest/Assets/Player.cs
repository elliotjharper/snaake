using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public float speed;

    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }

        var newVelocity = Vector3.zero;

        if (Input.anyKey) {

            if (Input.GetKey(KeyCode.W)) {
                newVelocity += Vector3.up * speed;
            }

            if (Input.GetKey(KeyCode.S)) {
                newVelocity += Vector3.down * speed;
            }

            if (Input.GetKey(KeyCode.A)) {
                newVelocity += Vector3.left * speed;
            }

            if (Input.GetKey(KeyCode.D)) {
                newVelocity += Vector3.right * speed;
            }

        }

        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = newVelocity;
    }
}
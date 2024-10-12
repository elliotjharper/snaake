using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum LobbyState
{
    ModeSelect,
    HostWaiting,
    ClientJoining
}

public class LobbyManager : MonoBehaviour
{
    public GameObject ModeSelectGroup;
    public GameObject StatusGroup;
    public TextMeshProUGUI StatusTb;

    public LobbyState State;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(LobbyState.ModeSelect);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HostGame()
    {
        ChangeState(LobbyState.HostWaiting);
        Debug.LogError("Hosting game...");
    }

    public void JoinGame()
    {
        ChangeState(LobbyState.ClientJoining);
        Debug.LogError("Joining game...");
    }

    private void ChangeState(LobbyState state)
    {
        State = state;

        switch (state)
        {
            case LobbyState.ModeSelect:
                ModeSelectGroup.SetActive(true);
                StatusGroup.SetActive(false);
                return;


            case LobbyState.HostWaiting:
            case LobbyState.ClientJoining:
                StatusTb.text = state == LobbyState.HostWaiting ? "Waiting for players..." : "Joining session...";
                ModeSelectGroup.SetActive(false);
                StatusGroup.SetActive(true);
                return;

        }
    }
}

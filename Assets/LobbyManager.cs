using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
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
    public TMP_Text StatusTb;
    public TMP_InputField JoinCodeInput;

    public LobbyState State;

    async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(LobbyState.ModeSelect);
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

    private async void HostGame()
    {
        ChangeState(LobbyState.HostWaiting);
        Debug.LogError("Hosting game...");

        var a = await RelayService.Instance.CreateAllocationAsync(2);
        StatusTb.text += " " + await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        var transport = FindObjectOfType<UnityTransport>();
        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
    }

    private async void JoinGame()
    {
        ChangeState(LobbyState.ClientJoining);
        Debug.LogError("Joining game...");

        var a = await RelayService.Instance.JoinAllocationAsync(JoinCodeInput.text);

        var transport = FindObjectOfType<UnityTransport>();
        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}

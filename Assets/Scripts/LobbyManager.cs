using TMPro;

using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;

using UnityEngine;
using UnityEngine.SceneManagement;

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
                StatusTb.text =
                    state == LobbyState.HostWaiting ?
                    "Starting session..." :
                    "Joining session...";
                ModeSelectGroup.SetActive(false);
                StatusGroup.SetActive(true);
                return;

        }
    }

    public async void HostGame()
    {
        ChangeState(LobbyState.HostWaiting);
        Debug.Log("Hosting game...");

        var a = await RelayService.Instance.CreateAllocationAsync(2);
        var code = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        StatusTb.text = $"Hosting session: {code} (copied to clipboard)";
        ClipboardUtility.CopyToClipboard(code);

        var transport = FindObjectOfType<UnityTransport>();
        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.OnClientConnectedCallback += (_) =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        };
    }

    public async void JoinGame()
    {
        ChangeState(LobbyState.ClientJoining);
        Debug.LogError("Joining game...");

        var a = await RelayService.Instance.JoinAllocationAsync(JoinCodeInput.text);

        var transport = FindObjectOfType<UnityTransport>();
        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
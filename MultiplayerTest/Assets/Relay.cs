using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour {

    public GameObject Panel;
    public TMP_Text JoinCodeText;
    public TMP_InputField JoinCodeInput;

    async void Awake() {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateGame() {
        Panel.SetActive(false);

        var a = await RelayService.Instance.CreateAllocationAsync(2);
        JoinCodeText.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        var transport = FindObjectOfType<UnityTransport>();
        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
    }

    public async void JoinGame() {
        Panel.SetActive(false);

        var a = await RelayService.Instance.JoinAllocationAsync(JoinCodeInput.text);

        var transport = FindObjectOfType<UnityTransport>();
        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
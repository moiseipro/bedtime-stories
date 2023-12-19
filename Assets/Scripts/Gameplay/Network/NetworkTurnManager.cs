using System;
using CardManagement;
using Source;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Network
{
    public class NetworkTurnManager : SingletonNetwork<NetworkTurnManager>
    {
        [SerializeField] private NetworkVariable<int> turnCount;
        [SerializeField] private NetworkVariable<ulong> clientIdTurn = new NetworkVariable<ulong>(0);

        private ClientRpcParams _clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams()
        };
        
        public Action OnStartGame;
        public Action OnEndTurn;
        public Action OnStartTurn;
        public Action<int> OnTurn;

        public bool CanTurn(ulong clientId)
        {
            return clientIdTurn.Value == clientId;
        }
        
        public void StartGame()
        {
            if (!IsHost && !IsServer) return;
            
            var clientIds = NetworkManager.Singleton.ConnectedClientsIds;
            if (clientIdTurn.Value == 0)
            {
                clientIdTurn.Value = clientIds[Random.Range(0, clientIds.Count)];
            }
            StartGameClientRpc();
        }

        [ClientRpc]
        public void StartGameClientRpc()
        {
            OnStartGame?.Invoke();
        }
        
        public void EndTurn(ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (clientId != clientIdTurn.Value) return;

            var clientIds = NetworkManager.Singleton.ConnectedClientsIds;
            for (int i = 0; i < clientIds.Count; i++)
            {
                if (clientIds[i] == clientIdTurn.Value)
                {
                    clientIdTurn.Value = i + 1 < clientIds.Count ? clientIds[i + 1] : clientIds[0];
                    break;
                }
            }

            _clientRpcParams.Send.TargetClientIds = new ulong[] { clientId };
            EndTurnClientRpc(_clientRpcParams);
            _clientRpcParams.Send.TargetClientIds = new ulong[] { clientIdTurn.Value };
            StartTurnClientRpc(_clientRpcParams);
        }
        
        public bool Turn(ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (clientId != clientIdTurn.Value) return false;
            
            bool canTurn = false;
            Debug.Log("Turned!: " + turnCount.Value);
            if (turnCount.Value > 0)
            {
                turnCount.Value -= 1;
                canTurn = true;
            }
            if (turnCount.Value == 0)
            {
                EndTurn(serverRpcParams);
                canTurn = true;
            }
            OnTurn?.Invoke(turnCount.Value);
            return canTurn;
        }

        [ClientRpc]
        private void EndTurnClientRpc(ClientRpcParams clientRpcParams = default)
        {
            Debug.Log("End your turn!");
            OnEndTurn?.Invoke();
        }
        
        [ClientRpc]
        private void StartTurnClientRpc(ClientRpcParams clientRpcParams = default)
        {
            Debug.Log("Start Your turn!");
            OnStartTurn?.Invoke();
        }

        [ServerRpc]
        public void SetTurnValueServerRpc(int value)
        {
            turnCount.Value = value;
        }
    }
}
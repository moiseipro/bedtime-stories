using System;
using System.Collections.Generic;
using CardManagement;
using Gameplay.Network;
using MapManagement.Generator;
using UI.Test;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public interface ICardTarget
    {
        
        void ReduceReason(int value);
        void TakeHorror(int value);
    }
    public class Player : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> _maxTurnCount = new NetworkVariable<int>(2);
        [SerializeField] private PlayerStats _playerStats = new PlayerStats();
        private PlayerCards _playerCards = new PlayerCards();

        [SerializeField] private PlayerChip playerChipPrefab;
        private PlayerChip _playerChip;
        [SerializeField] private HandView handViewPrefab;
        private HandView _handView;

        public static Player LocalInstance;
        
        private ClientRpcParams _clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams()
        };

        private GameMenuView _gameMenuView;

        public override void OnNetworkSpawn()
        {
            if (IsServer || IsHost)
            {
                _maxTurnCount.Value = 2;
                PlayerChip newPlayerChip = Instantiate(playerChipPrefab, transform);
                NetworkObject playerChipNetworkObject = newPlayerChip.GetComponent<NetworkObject>();
                playerChipNetworkObject.SpawnWithOwnership(OwnerClientId);
                
                InitializePlayerChipClientRpc(playerChipNetworkObject);
            }

            if (IsOwner)
            {
                LocalInstance = this;
                _handView = Instantiate(handViewPrefab, transform);
                NetworkTurnManager.Instance.OnStartTurn += () =>
                {
                    SendTurnValue(_maxTurnCount.Value);
                };
            }
        }
        
        private void SendTurnValue(int value)
        {
            NetworkTurnManager.Instance.SetTurnValueServerRpc(value);
        }

        [ClientRpc]
        public void InitializePlayerChipClientRpc(NetworkObjectReference networkObjectReference)
        {
            networkObjectReference.TryGet(out NetworkObject playerChipNetworkObject);
            playerChipNetworkObject.TryGetComponent(out _playerChip);
            _playerChip.Initialize(_playerStats);
        }
        
        public void RequestMovePlayer(ServerRpcParams serverRpcParams = default)
        {
            _playerChip.MoveNextTileServerRpc();
        }
        
        [ServerRpc]
        public void RequestСardServerRpc(ServerRpcParams serverRpcParams = default)
        {
            if (NetworkTurnManager.Instance.Turn())
            {
                int randomCard = Random.Range(0, Gameplay.CardLoader.Instance.ActionCardsCount);
                _clientRpcParams.Send.TargetClientIds = new ulong[] { serverRpcParams.Receive.SenderClientId };
            
                TakeCardClientRpc(randomCard, _clientRpcParams);
            }
            
        }
        
        [ClientRpc]
        public void TakeCardClientRpc(int cardId, ClientRpcParams clientRpcParams = default)
        {
            if (!IsOwner) return;
            _handView.TakeCard(Gameplay.CardLoader.Instance.GetActionCard(cardId));
        }

    }

    public struct ReasonData
    {
        public int Reason;
        public int MaxReason;

        public ReasonData(int maxReason)
        {
            MaxReason = maxReason;
            Reason = MaxReason;
        }
    }

    public struct TurnData
    {
        public int MaxTurns;
        public int MaxCardDistance;
    }
    
    [Serializable]
    public class PlayerStats
    {
        private ReasonData _reasonData;
        private TurnData _turnData;
        public int Reason => _reasonData.Reason;
        public int MaxReason => _reasonData.MaxReason;
        public int MaxTurns => _turnData.MaxTurns;

        private List<Features> _featuresPlayer = new List<Features>();

        public PlayerStats()
        {
            _reasonData = new ReasonData(1);
            _turnData = new TurnData
            {
                MaxCardDistance = 2,
                MaxTurns = 1
            };
        }
        
        public void AddReason(int value)
        {
            _reasonData.Reason += value;
            Debug.Log("Получено рассудка: " + value);
        }

        public void ReduceReason(int value)
        {
            _reasonData.Reason -= value;
            Debug.Log("Забрано рассудка: " + value);
        }
    }

    public class PlayerCards
    {
        private List<Card> _cards = new List<Card>();
    }
    
}
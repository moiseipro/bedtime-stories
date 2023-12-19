using System;
using CardManagement;
using Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects;

namespace MapManagement.Generator
{
    public interface ITileObject
    {
        public Vector3 Position { get; }
        public Vector3Int TilePosition { get; }
        public NetworkObject NetworkObject { get; }

        public void OccupyCell();
        public void ToFreeCell();

    }
    
    public class TileView : NetworkBehaviour, ITileObject, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private NetworkVariable<int> _tileNum;
        public int TileNum => _tileNum.Value;
        [SerializeField] private NetworkVariable<Vector3Int> _tilePosition;
        public Vector3Int TilePosition => _tilePosition.Value;
        [SerializeField] private NetworkVariable<bool> _isBusy;
        public bool IsBusy => _isBusy.Value;
       
        public Vector3 Position => transform.position;

        private NetworkObject _networkObject;

        [SerializeField] private GameObject[] _tileVisual;

        [SerializeField] private LocationCard _locationCard;
        private GameCard _gameCard = null;

        private ClientRpcParams _clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams()
        };

        private void Awake()
        {
            _networkObject = GetComponent<NetworkObject>();
        }

        public void Initialize(LocationCard locationCard, Vector3Int tilePosition, int tileNum)
        {
            _locationCard = locationCard;
            _tilePosition.Value = tilePosition;
            _tileNum.Value = tileNum;
        }

        public override void OnNetworkSpawn()
        {
            if (IsHost || IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
            }
            
        }

        private void SingletonOnOnClientConnectedCallback(ulong obj)
        {
            if (_gameCard != null)
            {
                DropCardClientRpc(_gameCard.ID);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DraggableCard dragCard))
            {
                dragCard.SetCardState(DragState.Target);
                dragCard.SetTargetLineCard(transform);
                Debug.Log ("Enter object was: "  + eventData.pointerDrag);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DraggableCard dragCard))
            {
                dragCard.SetCardState(DragState.Drag);
                dragCard.HideLineTarget();
                Debug.Log ("Exit object was: "  + eventData.pointerDrag);
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent(out ICardDrag cardDrag))
            {
                if (_gameCard == null)
                {
                    DropCardServerRpc(cardDrag.Card.ID);
                    cardDrag.DestroyCard();
                }
            }
        }
        
        [ServerRpc (RequireOwnership = false)]
        public void DropCardServerRpc(int index)
        {
            DropCardClientRpc(index);
        }
        
        [ClientRpc]
        public void DropCardClientRpc(int index)
        {
            _gameCard = Gameplay.CardLoader.Instance.GetActionCard(index);
        }

        public void OccupyCell()
        {
            _isBusy.Value = true;
        }
        
        public void ToFreeCell()
        {
            _isBusy.Value = false;
        }
    }
}
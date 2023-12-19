using System;
using CardManagement;
using Gameplay.Network;
using MapManagement.Generator;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    
    public interface IMoveObject
    {
        //public Vector3Int Position { get; }
    }
    
    public class PlayerChip : NetworkBehaviour, ICardTarget, IMoveObject, IDragHandler, IEndDragHandler, 
        IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private ITileObject _currentTile;
        
        private Collider _collider;
        private Transform _transform;
        private MapGenerator _mapGenerator;
        private NetworkObject _networkObject;

        [SerializeField] private PlayerStats _playerStats;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _transform = transform;
            _networkObject = GetComponent<NetworkObject>();
        }

        public void Initialize(PlayerStats playerStats)
        {
            _playerStats = playerStats;
        }

        public override void OnNetworkSpawn()
        {
            if (IsHost || IsServer)
            {
                if (NetworkGameManager.Instance.IsMapGenerated)
                {
                    SetMapGenerate(NetworkGameManager.Instance.MapGenerator);
                }
                else
                {
                    NetworkGameManager.Instance.OnMapGenerated += SetMapGenerate;
                }
                NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
            }
        }

        private void SingletonOnOnClientConnectedCallback(ulong obj)
        {
            PlaceToTileClientRpc(_mapGenerator.TileViewArray[(int)OwnerClientId].GetComponent<NetworkObject>());
        }

        private void SetMapGenerate(MapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;
            NetworkGameManager.Instance.OnMapGenerated -= SetMapGenerate;
        }

        public void ReduceReason(int value)
        {
            //_playerStats.ReduceReason(value);
        }

        public void TakeHorror(int value)
        {
            
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DraggableCard dragCard))
            {
                Debug.Log ("Dropped object was: "  + eventData.pointerDrag);
                Destroy(eventData.pointerDrag);
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

        public void OnDrag(PointerEventData eventData)
        { 
            if (!IsOwner) return;
            if (eventData.pointerEnter != null && eventData.pointerEnter.TryGetComponent(out TileView tileView))
            {
                Vector3 newPosition = eventData.pointerEnter.transform.position + eventData.pointerEnter.transform.up * 2f;
                _transform.position = newPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsOwner) return;
            if (eventData.pointerEnter == null || !eventData.pointerEnter.TryGetComponent(out TileView tileView) || tileView.IsBusy)
            {
                PlaceToTile();
            }
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsOwner) return;
            if (eventData.pointerDrag == null)
            {
                _collider.enabled = false;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsOwner) return;
            _collider.enabled = true;
        }
        
        [ServerRpc]
        public void MoveNextTileServerRpc()
        {
            if (NetworkTurnManager.Instance.Turn())
            {
                Debug.Log("Player move next tile");
                Vector3Int newPosition = _currentTile.TilePosition;
                newPosition.z++;
                ITileObject tileObject = _mapGenerator.GetTileByPosition(newPosition);

                if (tileObject != null)
                {
                    _currentTile?.ToFreeCell();
                    tileObject.OccupyCell();
                    PlaceToTileClientRpc(tileObject.NetworkObject);
                }
            }
        }

        // [ServerRpc]
        // public void PlaceToTileServerRpc(NetworkObjectReference networkObjectReference)
        // {
        //     Debug.Log("Place Player to Tile");
        //     networkObjectReference.TryGet(out NetworkObject networkObject);
        //     networkObject.TryGetComponent(out ITileObject tileObject);
        //     
        //     _currentTile?.ToFreeCell();
        //     tileObject.OccupyCell();
        //     PlaceToTileClientRpc(networkObjectReference);
        // }
        
        [ClientRpc]
        private void PlaceToTileClientRpc(NetworkObjectReference networkObjectReference)
        {
            networkObjectReference.TryGet(out NetworkObject networkObject);
            networkObject.TryGetComponent(out ITileObject tileObject);
            
            _currentTile = tileObject;

            PlaceToTile();
        }
        
        private void PlaceToTile()
        {
            Debug.Log("Игрок на тайле "+_currentTile+": x:" + _currentTile.TilePosition.x + " y:" + _currentTile.TilePosition.z);
            Vector3 newPos = _currentTile.Position + Vector3.up;
            _transform.position = newPos;
        }
    }
}
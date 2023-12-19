using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

namespace MapManagement.Generator
{
    public class MapGenerator : NetworkBehaviour
    {
        [SerializeField] private float tileSize;
        [SerializeField] private Vector2Int chunkSize;
        [SerializeField] private TileView tilePrefab;

        [SerializeField] private List<TileView> _tileViewArray = new List<TileView>();
        public List<TileView> TileViewArray => _tileViewArray;

        public override void OnNetworkSpawn()
        {
            if (IsHost || IsServer)
            {
                for (int k = 0; k < chunkSize.y; k++)
                {
                    for (int j = 0; j < chunkSize.x; j++)
                    {
                        Vector3 tilePosition = new Vector3(j * tileSize, 0, k * tileSize);
                        TileView newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);
                        newTile.GetComponent<NetworkObject>().Spawn(true);
                        newTile.Initialize(Gameplay.CardLoader.Instance.GetLocationCard(0), 
                            new Vector3Int(j, 0, k),
                                _tileViewArray.Count
                        );
                        _tileViewArray.Add(newTile);
                    }
                }
            }
        }

        public ITileObject GetTileByPosition(Vector3Int position)
        {
            return _tileViewArray.Find(x => x.TilePosition == position);
        }
    }
}
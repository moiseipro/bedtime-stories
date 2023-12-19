using System;
using Game;
using MapManagement.Generator;
using Source;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Network
{
    public class NetworkGameManager : SingletonNetwork<NetworkGameManager>
    {
        [SerializeField] private MapGenerator _mapGeneratorPrefab;
        private MapGenerator _mapGenerator;
        public MapGenerator MapGenerator => _mapGenerator;
        public bool IsMapGenerated => _mapGenerator != null;
        
        public Action<MapGenerator> OnMapGenerated;

        public override void OnNetworkSpawn()
        {
            if (IsServer || IsHost)
            {
                _mapGenerator = Instantiate(_mapGeneratorPrefab, transform);
                _mapGenerator.GetComponent<NetworkObject>().Spawn(true);
                OnMapGenerated?.Invoke(_mapGenerator);
            }
        }
    }
}
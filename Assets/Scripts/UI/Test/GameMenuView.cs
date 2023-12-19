using System;
using Gameplay.Network;
using TMPro;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.UI;
using Player = Game.Player;

namespace UI.Test
{
    public class GameMenuView : MonoBehaviour
    {
        [SerializeField] private Button turnButton;
        [SerializeField] private Button addCardButton;
        [SerializeField] private Button startGameButton;
        [SerializeField] private TMP_Text turnText;

        private void Start()
        {
            startGameButton.onClick.AddListener(() =>
            {
                NetworkTurnManager.Instance.StartGame();
            });
            addCardButton.onClick.AddListener((() =>
            {
                Player.LocalInstance.RequestСardServerRpc();
            }));
            turnButton.onClick.AddListener((() =>
            {
                Player.LocalInstance.RequestMovePlayer();
            }));
            
            NetworkTurnManager.Instance.OnTurn += OnTurn;
            
        }

        private void OnTurn(int val)
        {
            turnText.text = val.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace UnityTemplateProjects.Game
{
    public interface IPlayer
    {
        
        void TakeDamage(int value);
        void TakeHorror(int value);
    }
    public class Player : MonoBehaviour, IPlayer
    {
        private PlayerStats _playerStats = new PlayerStats();
        private PlayerCards _playerCards = new PlayerCards();

        private void Awake()
        {
            Debug.Log(_playerStats.Reason);
        }

        public void TakeDamage(int value)
        {
            throw new NotImplementedException();
        }

        public void TakeHorror(int value)
        {
            throw new NotImplementedException();
        }
    }

    public class PlayerStats
    {
        private int _reason;
        public int Reason => _reason;
        private int _horror;
        public int Horror => _horror;

        public PlayerStats()
        {
            _reason = 5;
            _horror = 0;
        }
        
        public void AddHealth(int value)
        {
            _reason += value;
            Debug.Log("Получено здоровье: " + value);
        }

        public void ReduceHealth(int value)
        {
            _reason -= value;
            Debug.Log("Забрано здоровье: " + value);
        }

        public void AddHorror(int value)
        {
            _horror += value;
            Debug.Log("Получено ужаса: " + value);
        }
    }

    public class PlayerCards
    {
        private List<EventCard> _eventCards = new List<EventCard>();
        private List<StoryCard> _storyCards = new List<StoryCard>();
        private List<CharacterCard> _characterCards = new List<CharacterCard>();
        private List<ActionCard> _actionCards = new List<ActionCard>();

        private List<Card> _cardInHand = new List<Card>();
    }
    
}
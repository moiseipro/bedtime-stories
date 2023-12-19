using System;
using Game;
using UnityEngine;
using UnityTemplateProjects;

namespace CardManagement.CardEvents
{
    [Serializable]
    public class DamageReceivingEvent : CardEvent
    {
        [SerializeField] private int _damageValue;

        DamageReceivingEvent(int damage)
        {
            _damageValue = damage;
        }
        
        public override void Activate(ICardTarget player)
        {
            player.ReduceReason(_damageValue);
        }
    }
}
using UnityTemplateProjects.Game;

namespace UnityTemplateProjects.CardEvents
{
    public class DamageReceivingEvent : CardEvent
    {
        private int _damageValue;

        DamageReceivingEvent(int damage)
        {
            _damageValue = damage;
        }
        
        public override void Activate(IPlayer player)
        {
            player.TakeDamage(_damageValue);
        }
    }
}
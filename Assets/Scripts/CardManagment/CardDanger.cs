using UnityTemplateProjects.Game;

namespace UnityTemplateProjects
{
    public class CardDanger
    {
        private string _description;
        private int _danger;
        private readonly CardEvent[] _cardEvents;

        CardDanger(CardEvent[] cardEvents)
        {
            _cardEvents = cardEvents;
        }

        public void ActivateAllEvents(IPlayer player)
        {
            foreach (var cardEvent in _cardEvents)
            {
                cardEvent.Activate(player);
            }
        }
    }
}
namespace UnityTemplateProjects
{
    public class CardDanger
    {
        private string _description;
        private int _danger;
        private CardEvent[] _cardEvents;

        public void ActivateAllEvents()
        {
            foreach (var cardEvent in _cardEvents)
            {
                cardEvent.Activate();
            }
        }
    }
}
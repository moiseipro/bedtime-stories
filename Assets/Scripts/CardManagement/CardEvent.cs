using Game;

namespace CardManagement
{
    public abstract class CardEvent
    {
        public abstract void Activate(ICardTarget player);
    }
}
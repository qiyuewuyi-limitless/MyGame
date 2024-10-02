namespace Gyvr.Mythril2D
{
    public interface IInteraction
    {
        public abstract bool TryExecute(CharacterBase source, IInteractionTarget target);
    }
}

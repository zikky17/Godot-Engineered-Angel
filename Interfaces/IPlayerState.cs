using Godot;

namespace EngineeredAngel.Interfaces
{
    public interface IPlayerState
    {
        public void HandleInput(Zikky player, Vector2 direction, bool isAttacking);
        public void Update(Zikky player);
    }
}

using Godot;

namespace EngineeredAngel.Interfaces
{
    public interface IPlayerState
    {
        public void HandleInput(Player player, Vector2 direction, bool isAttacking);
        public void Update(Player player);
    }
}

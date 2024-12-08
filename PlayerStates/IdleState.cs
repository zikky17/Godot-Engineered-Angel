using EngineeredAngel.Interfaces;
using Godot;

namespace EngineeredAngel.PlayerStates
{
    public class IdleState : IPlayerState
    {
        public void HandleInput(Zikky player, Vector2 direction, bool isAttacking)
        {
            if (direction != Vector2.Zero)
            {
                player.SetState(new WalkingState());
            }
            else if (isAttacking)
            {
                player.SetState(new AttackingState());
            }
            else
            {
                player.AnimatedSprite.Play("idle_right");

                player.AnimatedSprite.FlipH = player.LastDirection.X < 0;
            }
        }

        public void Update(Zikky player)
        {

        }
    }
}

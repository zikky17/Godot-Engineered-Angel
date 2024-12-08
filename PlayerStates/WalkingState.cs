using EngineeredAngel.Interfaces;
using Godot;

namespace EngineeredAngel.PlayerStates
{
    public class WalkingState : IPlayerState
    {
        public void HandleInput(Zikky player, Vector2 direction, bool isAttacking)
        {
            if (direction == Vector2.Zero)
            {
                player.SetState(new IdleState());
            }
            else if (isAttacking)
            {
                player.SetState(new AttackingState());
            }
            else
            {
                player.AnimatedSprite.Play("walk_right");

                player.AnimatedSprite.FlipH = direction.X < 0;

                player.LastDirection = direction;
            }
        }

        public void Update(Zikky player)
        {

        }
    }
}

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
                if (player.LastDirection == Vector2.Zero || player.LastDirection.X != 0)
                {
                    player.AnimatedSprite.Play("idle_right");
                }
                else if (player.LastDirection.Y < 0)
                {
                    player.AnimatedSprite.Play("idle_up");
                }
                else if (player.LastDirection.Y > 0)
                {
                    player.AnimatedSprite.Play("idle_down");
                }
                else
                {
                    player.AnimatedSprite.Play("idle_left");
                }
            }
        }

        public void Update(Zikky player)
        {
        }
    }
}

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
                if (direction.X != 0)
                {
                    player.AnimatedSprite.Play("walk_right");
                    player.AnimatedSprite.FlipH = direction.X < 0;
                }
                else if (direction.Y < 0)
                {
                    player.AnimatedSprite.Play("walk_up");
                    player.AnimatedSprite.FlipH = false;
                }
                else if (direction.Y > 0)
                {
                    player.AnimatedSprite.Play("walk_down");
                    player.AnimatedSprite.FlipH = false;
                }

                player.LastDirection = direction;
            }
        }

        public void Update(Zikky player)
        {

        }
    }
}

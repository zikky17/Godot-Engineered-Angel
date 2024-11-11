using EngineeredAngel.Interfaces;
using Godot;
using System.Threading.Tasks;

namespace EngineeredAngel.PlayerStates
{
    public class AttackingState : IPlayerState
    {
        public void HandleInput(Zikky player, Vector2 direction, bool isAttacking)
        {
            if (direction != Vector2.Zero)
            {
                player.SetState(new WalkingState());
            }
            else
            {
                player.SetState(new IdleState());
            }
        }

        public async void Update(Zikky player)
        {
            if (player.LastDirection.Y > 0)
            {
                player.AnimatedSprite.Play("attack_down");
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }
            else if (player.LastDirection.X != 0)
            {
                player.AnimatedSprite.Play("attack_right");
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }
            else
            {
                player.AnimatedSprite.Play("attack_up");
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }

            await Task.Delay(500);
            player.IsAttacking = false;

        }

    }
}

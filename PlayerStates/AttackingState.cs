using EngineeredAngel.Interfaces;
using Godot;
using System.Threading.Tasks;

namespace EngineeredAngel.PlayerStates
{
    public class AttackingState : IPlayerState
    {
        public void HandleInput(Player player, Vector2 direction, bool isAttacking)
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

        public async void Update(Player player)
        {
            player.IsAttacking = true;

            if (player.LastDirection.X != 0)
            {
                player.AnimatedSprite.Play("attack_right");
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }
            else
            {
                player.AnimatedSprite.Play("attack_right");
                player.AnimatedSprite.FlipH = player.LastDirection.X < 0;
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }

            await Task.Delay(1500); 

            player.IsAttacking = false;
        }
    }
}

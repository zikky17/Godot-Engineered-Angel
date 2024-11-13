using EngineeredAngel.Interfaces;
using Godot;
using System.Threading.Tasks;
using static Godot.TextServer;

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

            if (player.LastDirection.X != 0)
            {
                player.AnimatedSprite.Play("attack_left");
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }
            if (player.LastDirection.X == 0)
            {
                player.AnimatedSprite.Play("attack_left");
                player.AnimatedSprite.FlipH = player.LastDirection.X < 0;
                player._audioPlayer.Stream = player.attackSound;
                //player._audioPlayer.Play();
            }


            await Task.Delay(2000);
            player.IsAttacking = false;

        }

    }
}

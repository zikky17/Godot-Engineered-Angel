using EngineeredAngel.Database.DbServices;
using Godot;

namespace EngineeredAngel.GameManager
{
    public partial class GameLoader : Node
    {
        private readonly PlayerDataRepository _playerDataRepository = new PlayerDataRepository();

        public override async void _Ready()
        {
            var playerData = await _playerDataRepository.GetPlayerDataAsync(1);

            if (playerData != null)
            {
                CallDeferred(nameof(LoadMainScene));
            }
            else
            {
                CallDeferred(nameof(LoadStartScreen));
            }
        }

        private void LoadMainScene()
        {
            GetTree().ChangeSceneToFile("res://Scenes/start_town.tscn");
        }

        private void LoadStartScreen()
        {
            GetTree().ChangeSceneToFile("res://Scenes/StartScreen/StartScreen.tscn");
        }


    }
}

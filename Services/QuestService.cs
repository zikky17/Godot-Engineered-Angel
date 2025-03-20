using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Models.QuestModels;
using Godot;
using System.Collections.Generic;
using System.Text.Json;

namespace EngineeredAngel.Services
{
    public partial class QuestService
    {
        private ConfigFile configFile = new ConfigFile();
        private string filePath = "user://quests.cfg";

        public void SaveQuest(string questName, string questText, int? killCount, string monster, string npc, bool isCompleted)
        {
            var questData = new QuestData
            {
                Name = questName,
                Description = questText,
                KillCount = killCount,
                Monster = monster,
                NPC = npc,
                IsCompleted = isCompleted
            };

            string json = JsonSerializer.Serialize(questData);
            configFile.SetValue("Quests", questName, json);
            configFile.Save(filePath);
        }

        public Dictionary<string, QuestData> LoadAllQuests()
        {
            Dictionary<string, QuestData> quests = new Dictionary<string, QuestData>();

            Error err = configFile.Load(filePath);
            if (err != Error.Ok)
            {
                GD.Print("Could not load quest config file.");
                return quests;
            }

            if (!configFile.HasSection("Quests"))
            {
                GD.Print("No quests found.");
                return quests;
            }

            foreach (string key in configFile.GetSectionKeys("Quests"))
            {
                string jsonData = configFile.GetValue("Quests", key, "").ToString();
                QuestData quest = JsonSerializer.Deserialize<QuestData>(jsonData);
                quests[key] = quest;
            }

            return quests;
        }
    }
}

using EngineeredAngel.Stats;

namespace EngineeredAngel.Services;

public class LevelUpService
{
    public int Experience { get; set; }
    public int Level { get; set; }
    public int ExpForNextLevel { get; set; }
    public PlayerStats PlayerStats { get; set; } = new();


    public void GetExperience(int experience)
    {
        Experience = experience;
    }
    public void LevelUp()
    {
        if(Experience == 100)
        {
            PlayerStats.Level += 1;
        }
    }

}

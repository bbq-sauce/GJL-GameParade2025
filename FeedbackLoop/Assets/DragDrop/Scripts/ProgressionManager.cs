using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance { get; private set; }

    public CharacterData warlockData;
    public CharacterData clericData;

    public CharacterRuntimeStats WarlockStats { get; private set; }
    public CharacterRuntimeStats ClericStats { get; private set; }
    public enum KingReactionType { None, Angry, Happy }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WarlockStats = new CharacterRuntimeStats { baseData = warlockData };
            ClericStats = new CharacterRuntimeStats { baseData = clericData };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ShouldRestartWeek(int totalScore)
    {
        return totalScore < 50;
    }

    public void RecordTaskSuccess(string character)
    {
        GetStats(character).RecordSuccess();
    }

    public void RecordTaskFailure(string character)
    {
        GetStats(character).RecordFailure();
    }

    public void ApplyWeeklyBonuses()
    {
        WarlockStats.ApplyWeeklyModifiers();
        ClericStats.ApplyWeeklyModifiers();
    }

    private CharacterRuntimeStats GetStats(string character)
    {
        return character == "Warlock" ? WarlockStats : ClericStats;
    }
}

using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public CharacterRuntimeStats warlockStats;
    public CharacterRuntimeStats clericStats;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public enum KingReactionType { None, Angry, Happy }

    public KingReactionType GetKingReaction(int warlockFails)
    {
        if (warlockFails >= 2) return KingReactionType.Angry;
        if (warlockFails == 0) return KingReactionType.Happy;
        return KingReactionType.None;
    }

    public bool ShouldRestartWeek(int totalScore)
    {
        return totalScore < 50;
    }

    public void ApplyWeeklyProgression()
    {
        Debug.Log("Weekly Progrssion Applied");
        warlockStats.ApplyWeeklyModifiers();
        clericStats.ApplyWeeklyModifiers();
    }

    public void ResetWeeklyProgression()
    {
        warlockStats.ResetWeeklyProgressionOnNewGame();
        clericStats.ResetWeeklyProgressionOnNewGame();
    }
}

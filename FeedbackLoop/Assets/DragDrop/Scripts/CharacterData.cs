using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "DragDrop/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public float baseTime;
    public float baseLuck;
    public float currTimeToDoTask; // in seconds
    public float currSuccessRate; // 0-100
    public int pointsPerSuccess;
    public int pointsPerFailure;
}

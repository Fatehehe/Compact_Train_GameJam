using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "App/Data/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();

    public LevelData GetLevel(int index)
    {
        if (index >= 0 && index < levels.Count)
        {
            return levels[index];
        }
        return null;
    }

    public int GetTotalLevels()
    {
        return levels.Count;
    }
}
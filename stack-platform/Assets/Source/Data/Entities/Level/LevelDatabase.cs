using System.Collections.Generic;
using UnityEngine;

namespace Source.Data.Entities.Level
{
    [CreateAssetMenu(fileName = "Level Database", menuName = "Game/Level Database", order = 2)]
    public class LevelDatabase : ScriptableObject
    {
        public List<LevelEntity> levels;
    }
}
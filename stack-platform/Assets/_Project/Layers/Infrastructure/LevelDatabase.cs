using System.Collections.Generic;
using _Project.Layers.Data.Entities;
using UnityEngine;

namespace _Project.Layers.Infrastructure
{
    [CreateAssetMenu(fileName = "Level Database", menuName = "Game/Level Database", order = 2)]
    public class LevelDatabase : ScriptableObject
    {
        public List<LevelEntity> levels;
    }
}
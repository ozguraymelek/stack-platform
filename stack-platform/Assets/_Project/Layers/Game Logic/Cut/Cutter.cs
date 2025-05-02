using UnityEngine;

namespace _Project.Layers.Game_Logic.Cut
{
    // 2. Concrete implementation of the cutter
    public class Cutter : MonoBehaviour, ICutter
    {
        public void ExternalCut(GameObject target)
        {
            // Example: simply logs the cut operation
            Debug.Log($"Cutting {target.name}...");
            // TODO: Add real mesh-splitting or slicing logic here
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
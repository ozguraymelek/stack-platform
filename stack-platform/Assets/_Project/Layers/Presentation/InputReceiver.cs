using UnityEngine;

namespace _Project.Layers.Presentation
{
    public class InputReceiver : MonoBehaviour, IInputProvider
    {
        public bool ClickedLeftMouse()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}

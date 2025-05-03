using UnityEngine;

namespace _Project.Layers.Presentation
{
    public interface IInputProvider
    {
        bool InputEnabled { get; set; }
        bool ClickedLeftMouse();
    }
}

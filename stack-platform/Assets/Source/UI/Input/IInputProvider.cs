namespace Source.UI.Input
{
    public interface IInputProvider
    {
        bool InputEnabled { get; set; }
        bool ClickedLeftMouse();
    }
}

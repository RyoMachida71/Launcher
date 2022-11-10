namespace Launcher
{
    public interface IItem
    {
        Icon Icon { get; }
        string Path { get; }
        Point Location { get; }
        bool Start();
    }
}

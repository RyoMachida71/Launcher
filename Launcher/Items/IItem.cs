namespace Launcher.Items
{
    public interface IItem
    {
        Icon Icon { get; }
        string Path { get; }
        string Name { get; }
        Point Location { get; }
        bool Start();
    }
}

using ControlsLibrary.VirtualizingCollection;

namespace VideoStreamTagControlsLibrary.Models
{
    public interface IUser : IVirtualItem
    {
        int Idx { get; }
        string Name { get; }
        string Password { get; }
        object Data { get; }
    }
}
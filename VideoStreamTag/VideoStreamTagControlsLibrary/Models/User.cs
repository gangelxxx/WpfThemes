namespace VideoStreamTagControlsLibrary.Models
{
    public class User : IUser
    {
        public User(int idx, string name, string password, object data)
        {
            Name = name;
            Password = password;
            Idx = idx;
            Data = data;
        }

        public int Idx { get; }
        public string Name { get; }
        public string Password { get; }
        public object Data { get; }

        public override string ToString()
        {
            return Name;
        }

        public string UpperString { get; }
    }
}
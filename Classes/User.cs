using System;

namespace Branch.Classes
{
    public class User
    {
        internal int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        internal bool Enabled { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Name == user.Name &&
                   Username == user.Username &&
                   Password == user.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Username, Password);
        }
    }
}

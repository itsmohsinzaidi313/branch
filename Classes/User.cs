using System;

namespace Branch.Classes
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Name == user.Name &&
                   Username == user.Username &&
                   Password == user.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Username, Password);
        }
    }
}

using MyBffProject.Models;

namespace BFF_GameMatch.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public List<Sports> FavoriteSports { get; set; }
        public List<Team> Teams { get; set; }

        public List<User> Friends { get; set; }

    }
}

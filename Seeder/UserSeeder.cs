using Microsoft.EntityFrameworkCore;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.Entity;

namespace thu6_pvo_dictionary.Seeder
{
    class UserSeeder
    {
        private readonly ModelBuilder _modelBuilder;
        public UserSeeder(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        /// <summary>
        /// Excute data
        /// </summary>
        public void SeedData()
        {
            _modelBuilder.Entity<User>().HasData(
                new User
                {
                    user_id = 1,
                    user_name = "test",
                    password = "test",
                    email = "test@gmail.com",
                    display_name = "test",
                    full_name = "test",
                    birthday = new DateTime(2000, 06, 06),
                    position = "test",
                    avatar = "",
                    status = 1,
                }
                );
        }
    }
}

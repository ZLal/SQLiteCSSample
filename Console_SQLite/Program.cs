
namespace Console_SQLite
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to DB");
            try
            {
                string filePath = @"C:\SQL\SQLite\TestDB.sqlite";

                IUserRepo userRepo = new UserRepo_ADO(filePath);
                //IUserRepo userRepo = new UserRepo_Dapper(filePath);
                //IUserRepo userRepo = new UserRepo_EF(filePath);

                Console.WriteLine("Inserting data");
                List<User> users = new()
                {
                    new User() { Id = 0, Name = "User " + DateTime.Now.Millisecond },
                    new User() { Id = 0, Name = "User " + DateTime.Now.Millisecond },
                };
                await userRepo.CreateAsync(users);

                Console.WriteLine("Reading data");
                users = await userRepo.GetAllAsync();
                users.ForEach(x => Console.WriteLine($"{x.Id}, {x.Name}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error : ");
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
            }
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}

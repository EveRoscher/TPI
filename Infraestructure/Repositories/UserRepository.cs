using TPI.Aplication.Abstractions;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Repositories
{
    public class UserRepository : IUserService

    {
        public UserRepository()
        {
        }
        
        public List<User> getAll()
        {
            throw new NotImplementedException();
        }




        public User create(User user)
        {
            throw new NotImplementedException();
        }

        public bool delete(User user)
        {
            throw new NotImplementedException();
        }
      
        public User getById(Guid id)
        {
            throw new NotImplementedException();
        }

        public User update(User user)
        {
            throw new NotImplementedException();
        }

        public object? GetAll()
        {
            throw new NotImplementedException();
        }

    }
}

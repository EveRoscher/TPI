using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IUserService
    {
        List<User> getAll();
        User getById(Guid id);
        User create(User user);
        User update(User user);
        bool delete(User user);
        object? GetAll();
    }
}


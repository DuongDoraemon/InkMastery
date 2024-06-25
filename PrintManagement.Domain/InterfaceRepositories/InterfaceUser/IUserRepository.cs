using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.InterfaceRepositories.InterfaceUser
{
    public interface IUserRepository<TEntity> where TEntity : class
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByPhoneNumber(string phoneNumber);

        #region Get Role, Add Roles
        Task AddUserToRoleAsync(User user, IEnumerable<string> listRoles);
        Task<IEnumerable<string>> GetRolesOfUserAsync(User user);
        Task DeleteRolesOfUserAsync(User user, List<string> listRoles);
        #endregion
    }
}

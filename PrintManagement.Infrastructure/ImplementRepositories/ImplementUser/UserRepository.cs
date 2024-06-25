using IdentityModel;
using Microsoft.EntityFrameworkCore;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceUser;
using PrintManagement.Infrastructure.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Duende.IdentityServer.Models.IdentityResources;

namespace PrintManagement.Infrastructure.ImplementRepositories.ImplementUser
{
    public class UserRepository<TEntity> : IUserRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        protected IDbContext _IDbContext;
        protected DbSet<TEntity> _dbset;
        protected DbContext _dbContext;
        protected DbSet<TEntity> DBSet
        {
            get
            {
                if (_dbset == null)
                {
                    _dbset = _dbContext.Set<TEntity>() as DbSet<TEntity>;
                }
                return _dbset;
            }
        }
        public UserRepository(ApplicationDbContext context, IDbContext idbContext)
        {
            _context = context;
            _IDbContext = idbContext;
            _dbContext = (DbContext)idbContext;
        }
        #region Thêm danh sách quyền cho một người dùng
        public virtual async Task AddUserToRoleAsync(User user, IEnumerable<string> listRoles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (listRoles == null)
            {
                throw new ArgumentNullException(nameof(listRoles));
            }
            foreach (var role in listRoles.Distinct())
            {
                var rolesOfUser = await GetRolesOfUserAsync(user);
                if (await IsStringInListAsync(role, rolesOfUser.ToList()))
                {
                    var roleItem = await _context.Roles.SingleOrDefaultAsync(x => x.RoleCode.Equals(role));
                    var permission = await _context.Permissions.Where(record => record.UserId == user.Id && record.RoleId == roleItem.Id).SingleOrDefaultAsync();
                    permission.RoleId = roleItem.Id;
                    _context.Permissions.Update(permission);
                }
                else
                {
                    var roleItem = await _context.Roles.SingleOrDefaultAsync(x => x.RoleCode.Equals(role));
                    if (roleItem == null)
                    {
                        throw new ArgumentNullException("Không có quyền này");
                    }
                    _context.Permissions.Add(new Permissions { UserId = Guid.Parse((user.Id).ToString()), RoleId = Guid.Parse((roleItem.Id).ToString()) });
                }
            }
            _context.SaveChanges();
        }
        public async Task<IEnumerable<string>> GetRolesOfUserAsync(User user)
        {
            List<string> roles = new List<string>();
            var listRoles = _context.Permissions.Where(x => x.UserId == user.Id).AsQueryable();
            foreach (var item in listRoles.Distinct())
            {
                var role = _context.Roles.SingleOrDefault(x => x.Id == item.RoleId);
                roles.Add(role.RoleCode);
            }
            return roles.AsEnumerable();
        }

        public async Task DeleteRolesOfUserAsync(User user, List<string> listRoles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (listRoles == null)
            {
                throw new ArgumentNullException(nameof(listRoles));
            }
            foreach (var role in listRoles.Distinct())
            {
                var rolesOfUser = await GetRolesOfUserAsync(user);
                var listPermission = new List<Permissions>();
                if (await IsStringInListAsync(role, rolesOfUser.ToList()))
                {
                    var roleItem = await _context.Roles.SingleOrDefaultAsync(x => x.RoleCode.Equals(role));
                    var permission = await _context.Permissions.SingleOrDefaultAsync(x => x.UserId == user.Id && x.RoleId == roleItem.Id);
                    if (permission != null)
                    {
                        listPermission.Add(permission);
                    }
                }
                else
                {

                    throw new ArgumentNullException("Không có quyền này");

                }
                _context.Permissions.RemoveRange(listPermission);
            }
            _context.SaveChanges();
        }

        #endregion


        #region Xử lí chuỗi
        private Task<bool> CompareStringAsync(string str1, string str2)
        {
            return Task.FromResult(string.Equals(str1.ToLowerInvariant(), str2.ToLowerInvariant()));
        }



        private async Task<bool> IsStringInListAsync(string inputString, List<string> listString)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            if (listString == null)
            {
                throw new ArgumentNullException(nameof(listString));
            }

            foreach (var str in listString)
            {
                if (await CompareStringAsync(inputString, str))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower().Equals(username.ToLower()));
            return user;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.PhoneNumber.ToLower().Equals(phoneNumber.ToLower()));
            return user;
        }
    }
}

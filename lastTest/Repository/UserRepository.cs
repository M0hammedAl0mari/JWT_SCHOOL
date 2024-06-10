using lastTest.DataBase;
using lastTest.Models;

namespace lastTest.Repository
{
    public class UserRepository
    {
        private readonly MyContext _context;

        public UserRepository(MyContext context)
        {
            _context = context;
        }

        
        public User LoginUser(VMLogin model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            var r =_context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            user.Role = r;
            return user;
            
        }
    }
}

using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Services.BasicCRUD
{
    public class UserService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task CreateAsync(UserSignUpVM createUserVM)
        {
            var user = mapper.Map<User>(createUserVM);
            unitOfWork.DbContext.Users.Add(user);
            await unitOfWork.DbContext.SaveChangesAsync();
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            var user = await unitOfWork.DbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return unitOfWork.DbContext.Users;
        }
    }
}
using Commons;
using Cqrs.Commons;
using System;
using TasksManager.Implementation.UsersContext.Repositories.Entities;
using TasksManager.SharedContext.Events;
using TasksManager.UsersContext.Commands;

namespace TasksManager.Implementation.UsersContext
{
    public class UsersCommandHandler :
        ICommandHandler<CreateUser>,
        ICommandHandler<CreateCompany>,
        ICommandHandler<ChangeUserPassword>,
        ICommandHandler<ModifyUser>,
        ICommandHandler<ModifyCompany>
    {
        private IBus _bus;
        private IRepository<Company, Guid> _companies;
        private IRepository<User, Guid> _users;
        private IValidatorService _validator;

        public UsersCommandHandler(
            IRepository<Company, Guid> companies, IRepository<User, Guid> users,
            IBus bus, IValidatorService validator)
        {
            _bus = bus;
            _validator = validator;
            _companies = companies;
            _users = users;
        }

        public void Handle(CreateCompany message)
        {
            _validator.Validate(message);
            var company = new Company
            {
                IsEnabled = true,
                Name = message.Name
            };
            var newCompany = _companies.Save(company);
            _bus.SendAsync(new CompanyCreated
            {
                Id = newCompany.Id,
                CompanyName = newCompany.Name,
                IsEnabled = newCompany.IsEnabled
            });
        }

        public void Handle(CreateUser message)
        {
            _validator.Validate(message);
            var user = new User
            {
                CompanyId = message.CompanyId,
                Password = message.Password,
                UserName = message.UserName,
                IsAdmin = message.IsAdmin
            };
            var newUser = _users.Save(user);
            var company = _companies.GetById(newUser.CompanyId);
            _bus.SendAsync(new UserCreated
            {
                Id = newUser.Id,
                CompanyName = company.Name,
                CompanyId = message.CompanyId,
                Password = message.Password,
                UserName = message.UserName,
                IsAdmin = message.IsAdmin
            });
        }

        public void Handle(ChangeUserPassword message)
        {
            _validator.Validate(message);
            var user = _users.GetById(message.UserId);
            user.Password = message.NewPassword;
            _users.Update(user);
            var company = _companies.GetById(user.CompanyId);
            _bus.SendAsync(new UserPasswordModified
            {
                Id = user.Id,
                CompanyName = company.Name,
                CompanyId = user.CompanyId,
                Password = message.NewPassword,
                UserName = user.UserName,
            });
        }

        public void Handle(ModifyCompany message)
        {
            _validator.Validate(message);
            var company = _companies.GetById(message.Id);
            company.IsEnabled = message.IsEnabled;
            _companies.Update(company);
            _bus.SendAsync(new CompanyModified
            {
                Id = company.Id,
                CompanyName = company.Name,
                IsEnabled = company.IsEnabled
            });
        }

        public void Handle(ModifyUser message)
        {
            _validator.Validate(message);
            var user = _users.GetById(message.UserId);
            user.IsAdmin = message.IsAdmin;
            user.IsEnabled = message.IsEnabled;
            _users.Update(user);
            var company = _companies.GetById(user.CompanyId);
            _bus.SendAsync(new UserModified
            {
                Id = user.Id,
                CompanyName = company.Name,
                CompanyId = user.CompanyId,
                UserName = user.UserName,
                IsEnabled = user.IsEnabled,
                IsAdmin = user.IsAdmin,
                Password = user.Password
            });
        }
    }
}

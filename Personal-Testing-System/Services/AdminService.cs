using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class AdminService
    {
        private IAdminRepo AdminRepo;
        public AdminService(IAdminRepo _AdminRepo)
        {
            this.AdminRepo = _AdminRepo;
        }
        private AdminDto ConvertToAdminDto(Admin Admin)
        {
            return new AdminDto
            {
                Id = Admin.Id,
                FirstName = Admin.FirstName,
                SecondName = Admin.SecondName,
                LastName = Admin.LastName,  
                Login = Admin.Login,
                Password = Admin.Password,
                IdSubdivision = Admin.IdSubdivision
            };
        }
        private Admin ConvertToAdmin(AdminDto AdminDto)
        {
            return new Admin
            {
                Id = AdminDto.Id,
                FirstName = AdminDto.FirstName,
                SecondName = AdminDto.SecondName,
                LastName = AdminDto.LastName,
                Login = AdminDto.Login,
                Password = AdminDto.Password,
                IdSubdivision = AdminDto.IdSubdivision
            };
        }
        private Admin ConvertToAdmin(AddAdminModel admin)
        {
            return new Admin
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = admin.FirstName,
                SecondName = admin.SecondName,
                LastName = admin.LastName,
                Login = admin.Login,
                Password = admin.Password,
                IdSubdivision = admin.IdSubdivision
            };
        }
        public void DeleteAdminById(string id)
        {
            AdminRepo.DeleteAdminById(id);
        }

        public List<Admin> GetAllAdmins()
        {
            return AdminRepo.GetAllAdmins();
        }

        public List<AdminDto> GetAllAdminDtos()
        {
            List<AdminDto> Admins = new List<AdminDto>();
            AdminRepo.GetAllAdmins().ForEach(x => Admins.Add(ConvertToAdminDto(x)));
            return Admins;
        }

        public Admin GetAdminById(string id)
        {
            return AdminRepo.GetAdminById(id);
        }

        public AdminDto GetAdminDtoById(string id)
        {
            return ConvertToAdminDto(AdminRepo.GetAdminById(id));
        }

        public void SaveAdmin(Admin AdminToSave)
        {
            AdminRepo.SaveAdmin(AdminToSave);
        }

        public void SaveAdmin(AdminDto AdminDtoToSave)
        {
            AdminRepo.SaveAdmin(ConvertToAdmin(AdminDtoToSave));
        }

        public void SaveAdmin(AddAdminModel AdminDtoToAdd)
        {
            AdminRepo.SaveAdmin(ConvertToAdmin(AdminDtoToAdd));
        }
    }
}

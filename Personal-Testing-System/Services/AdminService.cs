using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

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

        public List<AdminDto> GetAdminDtosByQuestionId(int id)
        {
            return GetAllAdminDtos().Where(x => x.Id.Equals(id)).ToList();
        }

        public Admin GetAdminById(string id)
        {
            return AdminRepo.GetAdminById(id);
        }

        public void SaveAdmin(Admin AdminToSave)
        {
            AdminRepo.SaveAdmin(AdminToSave);
        }
    }
}

using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using System.Collections.Generic;

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
        public async Task<bool> DeleteAdminById(string id)
        {
            return await AdminRepo.DeleteAdminById(id);
        }

        public async Task<List<Admin>> GetAllAdmins()
        {
            return await AdminRepo.GetAllAdmins();
        }

        public async Task<List<AdminDto>> GetAllAdminDtos()
        {
            List<AdminDto> Admins = new List<AdminDto>();
            List<Admin> list = await AdminRepo.GetAllAdmins();
            list.ForEach(x => Admins.Add(ConvertToAdminDto(x)));
            return Admins;
        }

        public async Task<Admin> GetAdminById(string id)
        {
            return await AdminRepo.GetAdminById(id);
        }

        public async Task<AdminDto> GetAdminDtoById(string id)
        {
            return ConvertToAdminDto(await AdminRepo.GetAdminById(id));
        }

        public async Task<bool> SaveAdmin(Admin AdminToSave)
        {
            return await AdminRepo.SaveAdmin(AdminToSave);
        }

        public async Task<bool> SaveAdmin(AdminDto AdminDtoToSave)
        {
            return await AdminRepo.SaveAdmin(ConvertToAdmin(AdminDtoToSave));
        }

        public async Task<bool> SaveAdmin(AddAdminModel AdminDtoToAdd)
        {
            return await AdminRepo.SaveAdmin(ConvertToAdmin(AdminDtoToAdd));
        }
    }
}

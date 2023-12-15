using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class EmployeeResultSubcompetenceRepo : IEmployeeResultSubcompetenceRepo
    {
        private readonly EFDbContext context;
        public EmployeeResultSubcompetenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteEmployeeResultSubcompetenceById(int id)
        {
            context.ElployeeResultSubcompetences.Remove((await GetAllEmployeeResultSubcompetences()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ElployeeResultSubcompetence>> GetAllEmployeeResultSubcompetences()
        {
            return await context.ElployeeResultSubcompetences.ToListAsync();
        }

        public async Task<ElployeeResultSubcompetence> GetEmployeeResultSubcompetenceById(int id)
        {
            return await context.ElployeeResultSubcompetences.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<ElployeeResultSubcompetence> GetTrackEmployeeResultSubcompetenceById(int id)
        {
            return await context.ElployeeResultSubcompetences.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveEmployeeResultSubcompetence(ElployeeResultSubcompetence ElployeeResultSubcompetenceToSave)
        {
            ElployeeResultSubcompetence? ElployeeResultSubcompetence = await GetTrackEmployeeResultSubcompetenceById(ElployeeResultSubcompetenceToSave.Id);
            if (ElployeeResultSubcompetence != null && ElployeeResultSubcompetenceToSave.Id != 0)
            {
                /*context.ElployeeResultSubcompetences.Entry(ElployeeResultSubcompetenceToSave).State = EntityState.Detached;
                context.Set<ElployeeResultSubcompetence>().Update(ElployeeResultSubcompetenceToSave);*/
                ElployeeResultSubcompetence.IdResult = ElployeeResultSubcompetenceToSave.IdResult;
                ElployeeResultSubcompetence.IdSubcompetence = ElployeeResultSubcompetenceToSave.IdSubcompetence;
                ElployeeResultSubcompetence.Result = ElployeeResultSubcompetenceToSave.Result;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.ElployeeResultSubcompetences.AddAsync(ElployeeResultSubcompetenceToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}

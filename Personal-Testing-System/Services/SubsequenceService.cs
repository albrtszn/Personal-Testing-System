using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class SubsequenceService
    {
        private ISubsequenceRepo subsequenceRepo;
        public SubsequenceService(ISubsequenceRepo _subsequenceRepo)
        {
            this.subsequenceRepo = _subsequenceRepo;
        }
        public void DeleteSubsequenceById(int id)
        {
            subsequenceRepo.DeleteSubsequenceById(id);
        }

        public List<Subsequence> GetAllSubsequences()
        {
            return subsequenceRepo.GetAllSubSequences();
        }

        public Subsequence GetSubsequenceById(int id)
        {
            return subsequenceRepo.GetSubSequenceById(id);
        }

        public void SaveSubsequence(Subsequence SubsequenceToSave)
        {
            subsequenceRepo.SaveSubsequence(SubsequenceToSave);
        }
    }
}

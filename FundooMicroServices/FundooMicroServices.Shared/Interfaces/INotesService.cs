using FundooMicroServices.Models.RequestModels;
using FundooMicroServices.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Interfaces
{
    public interface INotesService
    {
        Task<List<NoteResponseModel>> GetAllFundooNotes(string email);

        Task<NoteResponseModel> GetFundooNoteById(string email, string id);

        Task<NoteResponseModel> CreateFundooNote(string email, NewNoteModel newFundooNote);

        Task<NoteResponseModel> UpdateFundooNoteById(string id, UpdateNoteModel updatedFundooNote);

        Task<NoteResponseModel> DeleteFundooNoteById(string id);
    }
}

using Compentio.Example.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.App.Repositories
{
    public interface INotesRepository
    {
        Task<IEnumerable<NoteDao>> GetNotes();
        Task<NoteDao> GetNote(long noteId);
        Task<NoteDao> CreateNote(NoteDao note);
        Task<NoteDao> UpdateNote(NoteDao note);
    }

    public class NotesRepository : INotesRepository
    {
        public async Task<NoteDao> CreateNote(NoteDao note)
        {
            return await Task.FromResult(noteDao);
        }

        public async Task<NoteDao> GetNote(long noteId)
        {
            return await Task.FromResult(noteDao);
        }

        public async Task<IEnumerable<NoteDao>> GetNotes()
        {
            var result = new List<NoteDao>() { noteDao };
            return await Task.FromResult(result);
        }

        public async Task <NoteDao> UpdateNote(NoteDao note)
        {
            return await Task.FromResult(noteDao);
        }

        private readonly NoteDao noteDao = new()
        {
            Id = 5,
            Created = DateTime.UtcNow,
            CreatedBy = "Admin",
            Description = "Description",
            Modified = DateTime.Now,
            PageTitle = "This is a title of the page",
            ValidFrom = DateTime.MinValue,
            ValidTo = DateTime.MaxValue
        };
    }
}

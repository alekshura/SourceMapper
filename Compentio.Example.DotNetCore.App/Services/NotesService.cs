using Compentio.Example.DotNetCore.App.Entities;
using Compentio.Example.DotNetCore.App.Mappers;
using Compentio.Example.DotNetCore.App.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compentio.Example.DotNetCore.App.Services
{
    public interface INotesService
    {
        Task<IEnumerable<NoteDto>> GetNotes();
        Task<NoteDto> GetNote(long noteId);
        Task<NoteDto> CreateNote(NoteDto note);
        Task<NoteDto> UpdateNote(NoteDto note);
    }

    [ExcludeFromCodeCoverage]
    public class NotesService : INotesService
    {
        private readonly INotesRepository _repository;
        private readonly INotesMapper _mapper;

        public NotesService(INotesRepository repository, INotesMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NoteDto> CreateNote(NoteDto note)
        {
            //var noteDao = _mapper.MapToDao(note);
            //var result = await _repository.CreateNote(noteDao);
            //return _mapper.MapToDto(result);
            return null;
        }

        public async Task<NoteDto> GetNote(long noteId)
        {
            var result = await _repository.GetNote(noteId);
            return _mapper.MapToDto(result);
        }

        public async Task<IEnumerable<NoteDto>> GetNotes()
        {
            var result = await _repository.GetNotes();
            return result.Select(noteDao => _mapper.MapToDto(noteDao));
        }

        public async Task<NoteDto> UpdateNote(NoteDto note)
        {
            //var noteDao = _mapper.MapToDao(note);
            //var result = await _repository.UpdateNote(noteDao);
            //return _mapper.MapToDto(result);
            return null;
        }
    }
}

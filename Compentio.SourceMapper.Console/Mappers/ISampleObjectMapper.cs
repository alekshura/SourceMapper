using Compentio.SourceMapper.Attributes;

namespace Compentio.ConsoleApp.Mappers
{
    [Mapper]
    interface ISampleObjectMapper
    {
        NoteDto MapToRest(NoteDao source);
    }
}

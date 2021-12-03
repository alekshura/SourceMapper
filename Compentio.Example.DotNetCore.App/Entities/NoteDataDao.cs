namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDataDao
    {
        public long Id { get; set; }

        public NoteDocumentDao Document { get; set; }
    }
}
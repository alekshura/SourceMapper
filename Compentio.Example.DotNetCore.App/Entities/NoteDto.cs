namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NoteDocumentDto Document { get; set; }
    }
}

namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDocumentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Base64String { get; set; }
        public NoteDocumentMetadataDto Metadata { get; set; }
        public string Autor { get; set; }
        public bool IsSigned { get; set; }
    }
}

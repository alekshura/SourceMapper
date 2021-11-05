namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDocumentMetadataDto
    {
        public long Id { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string SignatureBase64String { get; set; }
    }
}

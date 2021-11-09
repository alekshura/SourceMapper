namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDocumentMetadataDao
    {
        public long Id { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string SignatureBase64String { get; set; }
    }
}

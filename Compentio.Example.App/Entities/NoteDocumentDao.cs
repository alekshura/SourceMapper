namespace Compentio.Example.App.Entities
{
    public class NoteDocumentDao
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public byte[] DocumentData { get; set; }
        public string Metadata { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string SignatureBase64String { get; set; }
    }
}

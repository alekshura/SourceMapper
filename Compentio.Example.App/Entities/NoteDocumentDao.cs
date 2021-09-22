namespace Compentio.Example.App.Entities
{
    public class NoteDocumentDao
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public byte[] DocumentData { get; set; }
        public NoteDocumentMetadataDao Metadata { get; set; }
        public string SignatureBase64String { get; set; }
    }
}

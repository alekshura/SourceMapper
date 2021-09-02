namespace Compentio.ConsoleApp
{
    public record NoteDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

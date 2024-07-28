namespace IGB.DTOs
{
    public class TutorDocumentDto
    {
        public long Id { get; set; }
        public string? Information { get; set; }
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public byte[]? File { get; set; }
    }
}

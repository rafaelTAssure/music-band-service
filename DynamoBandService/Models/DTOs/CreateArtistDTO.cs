namespace DynamoBandService.Models.DTOs
{
    public class CreateArtistDTO
    {
        public string? BandSortId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Nationality { get; set; }

        public string? NickName { get; set; }

        public int? DebutYear { get; set; }
    }
}

using Amazon.DynamoDBv2.DataModel;

namespace DynamoBandService.Models
{
    [DynamoDBTable("bands")]
    public class Band
    {
        [DynamoDBHashKey("id")]
        public string? Id { get; set; }

        [DynamoDBRangeKey("sortId")]
        public string? SortId { get; set; }

        [DynamoDBProperty("name")]
        public string? Name { get; set; }

        [DynamoDBProperty("genre")]
        public string? Genre { get; set; }
/*
        [DynamoDBProperty("artists")]
        public List<Artist> Artists { get; set; } = new List<Artist>();*/
    }
}

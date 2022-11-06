using Amazon.DynamoDBv2.DataModel;

namespace DynamoBandService.Models
{
    [DynamoDBTable("bands")]
    public class Person
    {
        [DynamoDBHashKey("id")]
        public string? Id { get; set; }

        [DynamoDBRangeKey("sortId")]
        public string? SortId { get; set; }

        [DynamoDBProperty("name")]
        public string? Name { get; set; }

        [DynamoDBProperty("email")]
        public string? Email { get; set; }

        [DynamoDBProperty("dateOfBirth")]
        public string? DateOfBirth { get; set; }

        [DynamoDBProperty("nationality")]
        public string? Nationality { get; set; }
    }
}

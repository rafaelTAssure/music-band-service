using Amazon.DynamoDBv2.DataModel;

namespace DynamoBandService.Models
{
    public class Artist : Person
    {
        [DynamoDBProperty("nickName")]
        public string? NickName { get; set; }

        [DynamoDBProperty("debutYear")]
        public int? DebutYear { get; set; }
    }
}

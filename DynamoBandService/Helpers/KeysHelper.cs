namespace DynamoBandService.Helpers
{
    public class KeysHelper
    {

        public static string BuildKey(string prefix, string keyValue)
        {
            return $"{prefix}#{keyValue}";
        }

        public static string BuildThreePartKey(string prefix, string firstKeyValue, string secondKeyValue)
        {
            return $"{prefix}#{firstKeyValue}#{secondKeyValue}";
        }
    }
}

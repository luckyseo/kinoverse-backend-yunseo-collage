//Custom exception for invalid emotions
public class InvalidEmotionException : Exception
{
    public InvalidEmotionException(string message) : base(message)
    {
    }
}

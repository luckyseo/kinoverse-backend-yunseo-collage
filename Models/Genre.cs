using System.Runtime.Serialization;

namespace Models
{
    public enum Genre
    {
        Action, 
        Adventure, 
        Animation, 
        Comedy, 
        Crime, 
        Documentary, 
        Drama, 
        Family, 
        Fantasy, 
        History, 
        Horror, 
        Music, 
        Mystery, 
        Romance, 

        [EnumMember(Value = "Science Fiction")]
        ScienceFiction, 
        Thriller, 

        [EnumMember(Value = "TV Movie")]
        TVMovie, 
        War, 
        Western
    }
}
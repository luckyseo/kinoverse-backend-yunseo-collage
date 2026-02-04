using Models;
using DTOs;
namespace Collage.Backend.Induction.Starter.Services
{
    public class EmotionsService : IEmotionService
    {
        // Define emotion-related service methods here
        public Task<IEnumerable<EmotionType>> GetEmotionTypesAsync()
        {
            var emotions = Enum.GetValues<EmotionType>();
            return Task.FromResult(emotions.AsEnumerable());
        }
    }
}
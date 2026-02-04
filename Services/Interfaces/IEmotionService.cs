using Models;
using DTOs;
using System.Threading.Tasks;
namespace Collage.Backend.Induction.Starter.Services
{
    /*
    Validation and guard behaviour must be applied to all of the following (and any more that you have in your induction project):
    GET /api/emotions
    */
    public interface IEmotionService
    {
        // Define emotion-related service methods here
        Task<IEnumerable<EmotionType>> GetEmotionTypesAsync(); //read only
    }
}
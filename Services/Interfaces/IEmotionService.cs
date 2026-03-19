using Models;
using DTOs;
using System.Threading.Tasks;
//review
// BC: Interface functions are defined well according to the required endpoints
// it is fit for purpose and clear in its intent
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
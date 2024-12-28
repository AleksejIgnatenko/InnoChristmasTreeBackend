using InnoChristmasTree.Contracts;
using InnoChristmasTree.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLSubscription
    {
        [Subscribe]
        public async Task<CongratulationResponse> SubscribeToAddCongratulationAsync([Service] InnoChristmasTreeDbContext context, [EventMessage] CongratulationEntity congratulationEntity)
        {
            var countCongratulation = await context.Congratulations.CountAsync(c => c.Icon.Equals(congratulationEntity.Icon));
            return new CongratulationResponse(congratulationEntity.Id, congratulationEntity.Icon, congratulationEntity.CongratulationText, countCongratulation);
        }
    }
}

using InnoChristmasTree.Contracts;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLSubscription
    {
        [Subscribe]
        public CongratulationResponse SubscribeToAddCongratulation(
            [Service] InnoChristmasTreeDbContext context,
            [EventMessage] CongratulationEventDataModel eventData)
        {
            return new CongratulationResponse(
                eventData.CongratulationEntity.Id,
                eventData.CongratulationEntity.Icon,
                eventData.CongratulationEntity.CongratulationText,
                eventData.CountCongratulation);
        }
    }
}

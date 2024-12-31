using InnoChristmasTree.Contracts;
using InnoChristmasTree.Models;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLSubscription
    {
        // Подписка на добавление нового поздравления
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

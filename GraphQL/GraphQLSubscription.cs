using InnoChristmasTree.Contracts;
using InnoChristmasTree.Models;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLSubscription
    {
        // Подписка на добавление нового поздравления
        [Subscribe]
        public CongratulationResponse SubscribeToAddCongratulation(
            [EventMessage] CongratulationEventDataModel eventData)
        {
            return new CongratulationResponse(
                eventData.CongratulationModel.Id,
                eventData.CongratulationModel.Icon,
                eventData.CongratulationModel.CongratulationText,
                eventData.CountCongratulation);
        }
    }
}

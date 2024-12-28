using HotChocolate.Subscriptions;
using InnoChristmasTree.Contracts;
using InnoChristmasTree.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLMutation
    {
        private readonly ITopicEventSender _eventSender;
        public GraphQLMutation(ITopicEventSender eventSender)
        {
            _eventSender = eventSender;
        }

        public async Task<CongratulationResponse> CreateCongratulationAsync([Service] InnoChristmasTreeDbContext context, string icon, string congratulationText)
        {
            var congratulationEntity = new CongratulationEntity
            {
                Id = Guid.NewGuid(),
                Icon = icon,
                CongratulationText = congratulationText
            };

            await context.Congratulations.AddAsync(congratulationEntity);
            await context.SaveChangesAsync();

            int countCongratulation = await context.Congratulations.CountAsync(c => c.Icon.Equals(icon));

            await _eventSender.SendAsync(nameof(GraphQLSubscription.SubscribeToAddCongratulationAsync), congratulationEntity);

            return new CongratulationResponse(congratulationEntity.Id,
                congratulationEntity.Icon,
                congratulationEntity.CongratulationText,
                countCongratulation);
        }
    }
}

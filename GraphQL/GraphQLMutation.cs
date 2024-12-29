using HotChocolate.Subscriptions;
using InnoChristmasTree.Contracts;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;
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
            // Создаем новую сущность поздравления
            var congratulationEntity = new CongratulationEntity
            {
                Id = Guid.NewGuid(),
                Icon = icon,
                CongratulationText = congratulationText
            };

            // Добавляем новую сущность в контекст и сохраняем изменения
            await context.Congratulations.AddAsync(congratulationEntity);
            await context.SaveChangesAsync();

            var congratulationEntities = await context.Congratulations
                .OrderBy(c => c.Icon)
                .ToListAsync();

            var groupedCongratulations = congratulationEntities
                .GroupBy(c => c.Icon)
                .Select(g => new CongratulationGroupModel
                {
                    Icon = g.Key,
                    Count = g.Count(),
                    Congratulations = g.Select(entity => new CongratulationModel
                    {
                        Id = entity.Id,
                        Icon = entity.Icon,
                        CongratulationText = entity.CongratulationText
                    }).ToArray()
                })
                .ToList();

            // Получаем количество поздравлений с конкретной иконкой
            var countCongratulation = groupedCongratulations
                .FirstOrDefault(g => g.Icon == icon)?.Count ?? 0;

            // Вызов SendAsync с новым объектом
            var eventData = new CongratulationEventDataModel
            {
                CongratulationEntity = congratulationEntity,
                CountCongratulation = countCongratulation
            };

            await _eventSender.SendAsync(nameof(GraphQLSubscription.SubscribeToAddCongratulation), eventData);

            // Возвращаем ответ с информацией о созданном поздравлении и его количестве
            return new CongratulationResponse(congratulationEntity.Id,
                congratulationEntity.Icon,
                congratulationEntity.CongratulationText,
                countCongratulation);
        }
    }
}

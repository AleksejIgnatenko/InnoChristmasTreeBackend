using HotChocolate.Subscriptions;
using InnoChristmasTree.Contracts;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLMutation
    {
        private readonly ITopicEventSender _eventSender;
        public GraphQLMutation(ITopicEventSender eventSender)
        {
            _eventSender = eventSender;
        }

        //Создание нового поздравления
        public async Task<CongratulationResponse> CreateCongratulationAsync([Service] InnoChristmasTreeDbContext context, string icon, string congratulationText)
        {
            // Создание модели поздравления
            var (errors, congratulation) = CongratulationModel.Create(Guid.NewGuid(), icon, congratulationText);
            if (errors.Count > 0)
            {
                var result = JsonSerializer.Serialize(new { error = errors });

                throw new Exception(result);
            }

            // Создание сущности поздравления
            var congratulationEntity = new CongratulationEntity
            {
                Id = congratulation.Id,
                Icon = congratulation.Icon,
                CongratulationText = congratulation.CongratulationText,
            };

            // Добавление новой сущности в контекст и сохрание изменений
            await context.Congratulations.AddAsync(congratulationEntity);
            await context.SaveChangesAsync();

            // Получение всех поздравлений из бд
            var congratulationEntities = await context.Congratulations
                .OrderBy(c => c.Icon)
                .ToListAsync();

            // Группировка и создание модели поздравления
            var groupedCongratulations = congratulationEntities
                .GroupBy(c => c.Icon)
                .Select(g => new CongratulationGroupModel
                {
                    Icon = g.Key,
                    Count = g.Count(),
                    Congratulations = g.Select(entity =>
                    {
                        return CongratulationModel.Create(entity.Id, entity.Icon, entity.CongratulationText, false).congratulation;
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

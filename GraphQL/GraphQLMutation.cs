using HotChocolate.Subscriptions;
using InnoChristmasTree.Abstractions;
using InnoChristmasTree.Contracts;
using InnoChristmasTree.Data;
using InnoChristmasTree.Models;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLMutation
    {
        private readonly ITopicEventSender _eventSender;
        private readonly ICongratulationService _congratulationService;
        public GraphQLMutation(ITopicEventSender eventSender, ICongratulationService congratulationService)
        {
            _eventSender = eventSender;
            _congratulationService = congratulationService;
        }

        //Создание нового поздравления
        public async Task<CongratulationResponse> CreateCongratulationAsync([Service] InnoChristmasTreeDbContext context, string icon, string congratulationText)
        {
            // Создание поздравления
            var congratulation = await _congratulationService.CreateCongratulationAsync(context, icon, congratulationText);

            // Получение всех поздравлений из бд
            var congratulationEntities = await _congratulationService.GetAllCongratulationsAsync(context);

            // Группировка и создание модели поздравления
            var groupedCongratulations = _congratulationService.GetGroupedCongratulations(congratulationEntities);

            // Получаем количество поздравлений с конкретной иконкой
            var countCongratulation = groupedCongratulations
                .FirstOrDefault(g => g.Icon == icon)?.Count ?? 0;

            // Вызов SendAsync с новым объектом
            var eventData = new CongratulationEventDataModel
            {
                CongratulationModel = congratulation,
                CountCongratulation = countCongratulation
            };

            // Отправка в подписку
            await _eventSender.SendAsync(nameof(GraphQLSubscription.SubscribeToAddCongratulation), eventData);

            // Возвращаем ответ с информацией о созданном поздравлении и его количестве
            return new CongratulationResponse(congratulation.Id,
                congratulation.Icon,
                congratulation.CongratulationText,
                countCongratulation);
        }
    }
}
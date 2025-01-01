using InnoChristmasTree.Abstractions;
using InnoChristmasTree.Contracts;
using InnoChristmasTree.Data;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLQueries
    {
        private readonly ICongratulationService _congratulationService;

        public GraphQLQueries(ICongratulationService congratulationService)
        {
            _congratulationService = congratulationService;
        }

        // Получение всех поздравлений
        public async Task<CongratulationCollectionResponse> ReadCongratulationsAsync([Service] InnoChristmasTreeDbContext context)
        {
            // Получение поздравлений
            var congratulationEntities = await _congratulationService.GetAllCongratulationsAsync(context);

            // Создание ответа для отправки
            var groupedCongratulations = _congratulationService.GetGroupedCongratulations(congratulationEntities);

            return new CongratulationCollectionResponse(groupedCongratulations);
        }

        // Получение поздравлений с определенной иконкой
        public async Task<CongratulationCollectionResponse> GetCongratulationsByIconAsync([Service] InnoChristmasTreeDbContext context, string icon)
        {
            // Получение поздравлений
            var congratulationEntities = await _congratulationService.GetAllCongratulationsAsync(context);

            // Получение подходящих записей и создание ответа для отправки
            var groupedCongratulations = _congratulationService.GetGroupedCongratulationsByIcon(congratulationEntities, icon);

            return new CongratulationCollectionResponse(groupedCongratulations);
        }
    }
}
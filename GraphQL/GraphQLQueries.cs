using InnoChristmasTree.Contracts;
using InnoChristmasTree.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLQueries
    {
        // Получение всех поздравлений
        public async Task<CongratulationCollectionResponse> ReadCongratulationsAsync([Service] InnoChristmasTreeDbContext context)
        {
            // Получение поздравлений
            var congratulationEntities = await context.Congratulations
                .OrderBy(c => c.Icon)
                .ToListAsync();

            // Создание ответа для отправки
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

            return new CongratulationCollectionResponse(groupedCongratulations);
        }

        // Получение поздравлений с определенной иконкой
        public async Task<CongratulationCollectionResponse> GetCongratulationsByIconAsync([Service] InnoChristmasTreeDbContext context, string icon)
        {
            // Получение поздравлений
            var congratulationEntities = await context.Congratulations
                    .OrderBy(c => c.Icon)
                    .ToListAsync();

            // Получение подходящих записей и создание ответа для отправки
            var groupedCongratulations = congratulationEntities
                .Where(c => c.Icon == icon)
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

            return new CongratulationCollectionResponse(groupedCongratulations);
        }
    }
}

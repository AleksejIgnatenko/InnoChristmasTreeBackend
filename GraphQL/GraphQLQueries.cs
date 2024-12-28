using InnoChristmasTree.Contracts;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree.GraphQL
{
    public class GraphQLQueries
    {
        public async Task<CongratulationCollectionResponse> ReadCongratulations([Service] InnoChristmasTreeDbContext context)
        {
            var congratulationEntities = await context.Congratulations
                .OrderBy(c => c.Icon)
                .ToListAsync();

            // Группируем по иконке и создаем список для каждой группы
            var groupedCongratulations = congratulationEntities
                .GroupBy(c => c.Icon)
                .Select(g => new CongratulationGroupModel
                {
                    Count = g.Count(),
                    Congratulations = g.Select(entity => new CongratulationModel
                    {
                        Id = entity.Id,
                        Icon = entity.Icon,
                        CongratulationText = entity.CongratulationText
                    }).ToArray()
                })
                .ToList();

            return new CongratulationCollectionResponse(groupedCongratulations);
        }

    }
}

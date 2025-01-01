using InnoChristmasTree.Abstractions;
using InnoChristmasTree.Data;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;
using System.Text.Json;

namespace InnoChristmasTree.Services
{
    public class CongratulationService : ICongratulationService
    {
        private readonly ICongratulationRepository _congratulationRepository;

        public CongratulationService(ICongratulationRepository congratulationRepository)
        {
            _congratulationRepository = congratulationRepository;
        }

        public async Task<CongratulationModel> CreateCongratulationAsync(InnoChristmasTreeDbContext context, string icon, string congratulationText)
        {
            // Создание модели поздравления
            var (errors, congratulation) = CongratulationModel.Create(Guid.NewGuid(), icon, congratulationText);
            if (errors.Count > 0)
            {
                var result = JsonSerializer.Serialize(new { error = errors });

                throw new Exception(result);
            }

            var id = await _congratulationRepository.CreateCongratulationAsync(context, congratulation);

            return congratulation;
        }

        public List<CongratulationGroupModel> GetGroupedCongratulations(List<CongratulationEntity> congratulationEntities)
        {
            return congratulationEntities
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
        }

        public List<CongratulationGroupModel> GetGroupedCongratulationsByIcon(List<CongratulationEntity> congratulationEntities, string icon)
        {
            return congratulationEntities
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
        }

        public async Task<List<CongratulationEntity>> GetAllCongratulationsAsync(InnoChristmasTreeDbContext context)
        {
            return await _congratulationRepository.GetAllCongratulationsAsync(context);
        }
    }
}

using InnoChristmasTree.Abstractions;
using InnoChristmasTree.Data;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree.Repositories
{
    public class CongratulationRepository : ICongratulationRepository
    {
        public async Task<Guid> CreateCongratulationAsync(InnoChristmasTreeDbContext context, CongratulationModel congratulation)
        {
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

            return congratulationEntity.Id;
        }


        public async Task<List<CongratulationEntity>> GetAllCongratulationsAsync(InnoChristmasTreeDbContext context)
        {
            return await context.Congratulations.ToListAsync();
        }
    }
}

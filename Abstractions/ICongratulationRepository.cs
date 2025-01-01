using InnoChristmasTree.Data;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;

namespace InnoChristmasTree.Abstractions
{
    public interface ICongratulationRepository
    {
        Task<Guid> CreateCongratulationAsync([Service] InnoChristmasTreeDbContext context, CongratulationModel congratulation);
        Task<List<CongratulationEntity>> GetAllCongratulationsAsync(InnoChristmasTreeDbContext context);
    }
}
using InnoChristmasTree.Data;
using InnoChristmasTree.Entities;
using InnoChristmasTree.Models;

namespace InnoChristmasTree.Abstractions
{
    public interface ICongratulationService
    {
        Task<CongratulationModel> CreateCongratulationAsync([Service] InnoChristmasTreeDbContext context, string icon, string congratulationText);
        List<CongratulationGroupModel> GetGroupedCongratulations(List<CongratulationEntity> congratulationEntities);
        List<CongratulationGroupModel> GetGroupedCongratulationsByIcon(List<CongratulationEntity> congratulationEntities, string icon);
        Task<List<CongratulationEntity>> GetAllCongratulationsAsync(InnoChristmasTreeDbContext context);
    }
}
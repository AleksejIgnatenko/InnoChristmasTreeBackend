using InnoChristmasTree.Models;

namespace InnoChristmasTree.Contracts
{
    public record CongratulationCollectionResponse(List<CongratulationGroupModel> GroupedCongratulations);
}

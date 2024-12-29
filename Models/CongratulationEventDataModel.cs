using InnoChristmasTree.Entities;

namespace InnoChristmasTree.Models
{
    public class CongratulationEventDataModel
    {
        public CongratulationEntity CongratulationEntity { get; set; } = new CongratulationEntity();
        public int CountCongratulation { get; set; }
    }
}

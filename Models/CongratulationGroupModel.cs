namespace InnoChristmasTree.Models
{
    public class CongratulationGroupModel
    {
        public int Count { get; init; }
        public CongratulationModel[] Congratulations { get; init; } = Array.Empty<CongratulationModel>();
    }
}

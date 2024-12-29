namespace InnoChristmasTree.Models
{
    public class CongratulationGroupModel
    {
        public string Icon { get; set; } = string.Empty;
        public int Count { get; init; }
        public CongratulationModel[] Congratulations { get; init; } = Array.Empty<CongratulationModel>();
    }
}

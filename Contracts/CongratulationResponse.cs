namespace InnoChristmasTree.Contracts
{
    public record CongratulationResponse(
        Guid Id,
        string Icon,
        string congratulationText,
        int Count
        );
}
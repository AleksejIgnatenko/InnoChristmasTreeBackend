using FluentValidation.Results;
using InnoChristmasTree.Validations;

namespace InnoChristmasTree.Models
{
    public class CongratulationModel
    {
        public Guid Id { get; }
        public string Icon { get; } = string.Empty;
        public string CongratulationText { get; } = string.Empty;

        public CongratulationModel(Guid id, string icon, string congratulationText)
        {
            Id = id;
            Icon = icon;
            CongratulationText = congratulationText;
        }

        public CongratulationModel()
        {
        }

        // Метод для создания модели поздравления
        public static (Dictionary<string, string> errors, CongratulationModel congratulation) Create(Guid id, string icon, string congratulationText, bool useValidation = true)
        {
            // Для оишибок валидации
            Dictionary<string, string> errors = new Dictionary<string, string>();

            // Создание модели
            CongratulationModel congratulationModel = new CongratulationModel(id, icon, congratulationText);
            if (!useValidation) { return (errors, congratulationModel); }

            // Валидация и проверка результатов
            CongratulationModelValidation congratulationModelValidation = new CongratulationModelValidation();
            ValidationResult validationResult = congratulationModelValidation.Validate(congratulationModel);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, congratulationModel);
        }
    }
}

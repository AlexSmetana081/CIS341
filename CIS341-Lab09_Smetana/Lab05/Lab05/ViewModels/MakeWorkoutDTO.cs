using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lab05.Models;

namespace Lab05.ViewModels
{
    public class MakeWorkoutDTO
    {
        AccountDTO Account { get; set; } = new AccountDTO();

        List<ExerciseDTO> Exercises { get; set; } = new List<ExerciseDTO>();

        List<WorkoutDTO> Workouts { get; set; } = new List<WorkoutDTO>();

        List<WorkoutSetDTO> Sets { get; set; } = new List<WorkoutSetDTO>();
    }
}

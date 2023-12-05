using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lab05.Models;

namespace Lab05.ViewModels
{
    public class MakeWorkoutDTO
    {
        public AccountDTO Account { get; set; } = new AccountDTO();

        //public List<int> Exercises { get; set; } = new List<int>();
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

        public List<WorkoutDTO> Workouts { get; set; } = new List<WorkoutDTO>();

        public List<WorkoutSetDTO> Sets { get; set; } = new List<WorkoutSetDTO>();

        public int Total { get; set; }
    }
}

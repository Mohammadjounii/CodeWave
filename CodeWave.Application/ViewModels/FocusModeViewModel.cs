using System;
using System.Collections.Generic;
using System.Text;


    using CodeWave.Domain.Entities;

    namespace CodeWave.Application.ViewModels
    {
        public class FocusModeViewModel
        {
            public Lesson Lesson { get; set; }
            public Course Course { get; set; }
            public List<CodingExercise> Exercises { get; set; }
            public Dictionary<Guid, bool> SolvedLookup { get; set; }
            public Guid? PreviousLessonId { get; set; }
            public Guid? NextLessonId { get; set; }
        }
    }



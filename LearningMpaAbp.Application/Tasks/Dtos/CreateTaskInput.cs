using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks.Dtos
{
    public class CreateTaskInput
    {
        public int? AssignedPersonId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        public TaskState State { get; set; }

        public override string ToString()
        {
            return string.Format("[CreateTaskInput > AssignedPersonId = {0}, Description = {1}]", AssignedPersonId, Description);
        }
}

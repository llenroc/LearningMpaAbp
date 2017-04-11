﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks.Dtos
{
    /// <summary>
    ///  A DTO class that can be used in various application service methods when needed to send/receive Task objects.
    /// </summary>
    public class TaskDto:EntityDto
    {
        public long? AssignedPersonId { get; set; }
        public string AssignedPersonName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreationTime { get; set; }

        public TaskState State { get; set; }

        public override string ToString()
        {
            return string.Format("[Task Id={0},Description={1},CreationTime={2},AssignedName={3},Stste={4}]",
                Id,
                Description,
                CreationTime,
                AssignedPersonName,
                (TaskState)State
                );
        }
    }
}

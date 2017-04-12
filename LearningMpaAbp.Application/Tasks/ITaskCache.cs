using Abp.Domain.Entities.Caching;
using LearningMpaAbp.Tasks.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks
{
    public interface ITaskCache : IEntityCache<TaskCacheItem> {

    }
}

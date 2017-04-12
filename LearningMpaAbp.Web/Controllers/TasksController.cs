using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Controllers;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningMpaAbp.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TasksController : AbpController
    {
        private readonly ITaskAppService _taskAppService;
        private readonly IUserAppService _userAppService;

        public TasksController(ITaskAppService taskAppService, IUserAppService userAppService) {
            _taskAppService = taskAppService;
            _userAppService = userAppService;
        }
    }
}
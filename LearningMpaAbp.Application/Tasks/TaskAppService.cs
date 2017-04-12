using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Net.Mail.Smtp;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.Timing;
using AutoMapper;
using LearningMpaAbp.Authorization;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks
{
    /// <summary>
    /// Implements <see cref="ITaskAppService"/> to perform task related application functionality.
    /// 
    /// Inherits from <see cref="ApplicationService"/>.
    /// <see cref="ApplicationService"/> contains some basic functionality common for application services (such as logging and localization).
    /// </summary>
    public class TaskAppService : LearningMpaAbpAppServiceBase, ITaskAppService
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ISmtpEmailSenderConfiguration _smtpEmialSenderConfig;
        //These members set in constructor using constructor injection.

        private readonly IRepository<Task> _taskRepository;
        //private readonly IRepository<Person> _personRepository;
        private readonly IRepository<User, long> _userRepository;

        private readonly ITaskCache _taskCache;

        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
        /// </summary>
        //public TaskAppService(IRepository<Task> taskRepository, IRepository<Person> personRepository)
        //{
        //    _taskRepository = taskRepository;
        //    _personRepository = personRepository;
        //}

        /// <summary>
        ///     In constructor, we can get needed classes/interfaces.
        ///     They are sent here by dependency injection system automatically.
        /// </summary>
        public TaskAppService(IRepository<Task> taskRepository, IRepository<User, long> userRepository,
            ISmtpEmailSenderConfiguration smtpEmialSenderConfigtion, INotificationPublisher notificationPublisher, ITaskCache taskCache)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _smtpEmialSenderConfig = smtpEmialSenderConfigtion;
            _notificationPublisher = notificationPublisher;
            _taskCache = taskCache;
        }

        public IList<TaskDto> GetAllTasks()
        {
            var tasks = _taskRepository.GetAll().OrderByDescending(t => t.CreationTime).ToList();
            return Mapper.Map<IList<TaskDto>>(tasks);
        }

        public GetTasksOutput GetTasks(GetTasksInput input)
        {
            var query = _taskRepository.GetAll();

            if (input.AssignedPersonId.HasValue)
            {
                query = query.Where(t => t.AssignedPersonId == input.AssignedPersonId.Value);
            }

            if (input.State.HasValue)
            {
                query = query.Where(t => t.State == input.State.Value);
            }

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            return new GetTasksOutput
            {
                Tasks = Mapper.Map<List<TaskDto>>(query.ToList())
            };
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId)
        {
            //Called specific GetAllWithPeople method of task repository.
            var task = await _taskRepository.GetAsync(taskId);

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            return task.MapTo<TaskDto>();
        }

        public TaskDto GetTaskById(int taskId)
        {
            var task = _taskRepository.Get(taskId);

            return task.MapTo<TaskDto>();
        }

        //public void UpdateTask(UpdateTaskInput input)
        //{
        //    //We can use Logger, it's defined in ApplicationService base class.
        //    Logger.Info("Updating a task for input: " + input);

        //    //Retrieving a task entity with given id using standard Get method of repositories.
        //    var task = _taskRepository.Get(input.Id);

        //    //Updating changed properties of the retrieved task entity.

        //    if (input.State.HasValue)
        //    {
        //        task.State = input.State.Value;
        //    }

        //    if (input.AssignedPersonId.HasValue)
        //    {
        //        task.AssignedPerson = _personRepository.Load(input.AssignedPersonId.Value);
        //    }

        //    //We even do not call Update method of the repository.
        //    //Because an application service method is a 'unit of work' scope as default.
        //    //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        //}
        public void UpdateTask(UpdateTaskInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            Logger.Info("Updating a task for input: " + input);

            //获取是否有权限
            bool canAssignTaskToOther = PermissionChecker.IsGranted(PermissionNames.Pages_Tasks_AssignPerson);
            //如果任务已经分配且未分配给自己，且不具有分配任务权限，则抛出异常
            if (input.AssignedPersonId.HasValue && input.AssignedPersonId.Value != AbpSession.GetUserId() && !canAssignTaskToOther)
            {
                throw new AbpAuthorizationException("没有分配任务给他人的权限！");
            }

            var updateTask = Mapper.Map<Task>(input);
            _taskRepository.Update(updateTask);
        }

        //public int CreateTask(CreateTaskInput input)
        //{
        //    //We can use Logger, it's defined in ApplicationService class.
        //    Logger.Info("Creating a task for input: " + input);

        //    //Creating a new Task entity with given input's properties
        //    var task = new Task
        //    {
        //        Description = input.Description,
        //        Title = input.Title,
        //        State = input.State,
        //        CreationTime = Clock.Now
        //    };

        //    if (input.AssignedPersonId.HasValue)
        //    {
        //        task.AssignedPerson = _personRepository.Load(input.AssignedPersonId.Value);
        //    }

        //    //Saving entity with standard Insert method of repositories.
        //    return _taskRepository.InsertAndGetId(task);
        //}
        public int CreateTask(CreateTaskInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            Logger.Info("Creating a task for input: " + input);

            //判断用户是否有权限
            if (input.AssignedPersonId.HasValue && input.AssignedPersonId.Value != AbpSession.GetUserId())
                PermissionChecker.Authorize(PermissionNames.Pages_Tasks_AssignPerson);

            var task = Mapper.Map<Task>(input);

            int result = _taskRepository.InsertAndGetId(task);

            //只有创建成功才发送邮件和通知
            if (result > 0)
            {
                task.CreationTime = Clock.Now;

                if (input.AssignedPersonId.HasValue)
                {
                    task.AssignedPerson = _userRepository.Load(input.AssignedPersonId.Value);
                    var message = "You hava been assigned one task into your todo list.";

                    //TODO:需要重新配置QQ邮箱密码
                    //SmtpEmailSender emailSender = new SmtpEmailSender(_smtpEmialSenderConfig);
                    //emailSender.Send("ysjshengjie@qq.com", task.AssignedPerson.EmailAddress, "New Todo item", message);

                    _notificationPublisher.Publish("NewTask", new MessageNotificationData(message), null,
                        NotificationSeverity.Info, new[] { task.AssignedPerson.ToUserIdentifier() });
                }
            }

            return result;
        }

        public void DeleteTask(int taskId)
        {
            var task = _taskRepository.Get(taskId);
            if (task != null)
            {
                _taskRepository.Delete(task);
            }
        }
    }
}

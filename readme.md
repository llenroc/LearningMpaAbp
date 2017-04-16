# 目录
[01.学习ABP系列一——通过模板创建MPA版本项目](01.学习ABP系列一——通过模板创建MPA版本项目)  
[02.学习ABP系列二——领域层创建实体](02.学习ABP系列二——领域层创建实体)  
[03.学习ABP系列三——领域层定义仓储并实现](03.学习ABP系列三——领域层定义仓储并实现)  
[04.学习ABP系列四——创建应用服务](04.学习ABP系列四——创建应用服务)  
[05.学习ABP系列五——展现层实现增删改查](05.学习ABP系列五——展现层实现增删改查)  
[06.学习ABP系列六——定义导航菜单](06.学习ABP系列六——定义导航菜单)  
[07.学习ABP系列七——分页实现](07.学习ABP系列七——分页实现)  
[08.学习ABP系列八——Json格式化](08.学习ABP系列八——Json格式化)  
[09.学习ABP系列九——权限管理](09.学习ABP系列九——权限管理)  
[10.学习ABP系列十——扩展AbpSession](10.学习ABP系列十——扩展AbpSession)  
[11.学习ABP系列十一——编写单元测试](11.学习ABP系列十————编写单元测试)  
[12.学习ABP系列十二——如何升级Abp并调试源码](12.学习ABP系列十二——如何升级Abp并调试源码)  
[13.学习ABP系列十三——Redis缓存用起来](13.学习ABP系列十三——Redis缓存用起来)  
[14.学习ABP系列十四——应用BootstrapTable表格插件](14.学习ABP系列十四——应用BootstrapTable表格插件)  
[15.学习ABP系列十五——创建微信公众号模块](15.学习ABP系列十五——创建微信公众号模块)  


# 学习ABP系列一——通过模板创建MPA版本项目
## 一、从官网下载模板
[官网地址](https://aspnetboilerplate.com)  
选择多页面，包含zero,输入项目名称LearningMpaAbp,输入验证码下载

## 二、启动项目
### 1、用VS2013以上打开，还原Nuget包
### 2、设置以Web结尾的项目为启动项目
### 3、打开Web.config，修改连接字符串
```
<add name="Default" connectionString="Data Source=.; Database=LearningMpaAbp; User ID=sa; Password=sa;" providerName="System.Data.SqlClient" />
```
### 4、打开程序包管理器控制台，选择以EntityFramework结尾的项目，并执行Update-Database，以创建数据库
### 5、模板项目己完成


# 学习ABP系列二——领域层创建实体
## 一、ABP结构
领域层就是业务层，是一个项目的核心，所有业务规则都应该在领域层实现。
实体（Entity）： 实体代表业务领域的数据和操作，在实践中，通过用来映射成数据库表。
仓储（Repository）： 仓储用来操作数据库进行数据存取。仓储接口在领域层定义，而仓储的实现类应该写在基础设施层。
领域服务（Domain service）： 当处理的业务规则跨越两个（及以上）实体时，应该写在领域服务方法里面。
领域事件（Domain Event）： 在领域层有些特定情况发生时可以触发领域事件，并且在相应地方捕获并处理它们。
工作单元（Unit of Work）： 工作单元是一种设计模式，用于维护一个由已经被修改(如增加、删除和更新等)的业务对象组成的列表。它负责协调这些业务对象的持久化工作及并发问题。

## 二、解决方案
Application		应用服务层
Core			领域层
EntityFramework	基础设施层
Web、WebApi		Web与展现层

## 三创建Task实体
### 1、在领域层创建Tasks文件夹，并创建Task实体类
### 2、ABP中所有的实体类都继承自Entity，而Entity实现了IEntity接口；而IEntity接口是一个泛型接口，通过泛型指定主键Id类型，默认的Entity的主键类型是int类型。
创建Task，肯定需要保存创建时间，可以通过实现审计模块中的IHasCreationTime来实现这种通用功能。代码如下
```
namespace LearningMpaAbp.Tasks
{
    public class Task : Entity, IHasCreationTime
    {
        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024;//64kb

        public long? AssignedPersonId { get; set; }

        [ForeignKey("AssignedPersonId")]
        public User AssignedPerson { get; set; }

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public TaskState State { get; set; }

        public DateTime CreationTime { get; set; }

        public Task() {
            CreationTime = Clock.Now;
            State = TaskState.Open;
        }

        public Task(string title, string description = null) : this() {
            Title = title;
            Description = description;
        }
    }

    public enum TaskState:byte
    {
        Open=0,
        Completed=1
    }
}
```
其中定义了TaskState状态枚举。并添加了AssignedPerson导航属性，用来保存分配任务到某个用户。其中[Required]、[MaxLength]特性是用来进行输入校验的。
### 3、定义好实体之后，我们就要去DbContext中定义实体对应的DbSet，以应用Code First 数据迁移。找到我们的基础服务层，即以EntityFramework结尾的项目中，找到DbContext类，添加以下代码
```
//TODO: Define an IDbSet for your Entities...
 public IDbSet<Task> Tasks { get; set; }
```
### 4、执行Code First数据迁移。
打开程序包管理器控制台，默认项目选择Entityframework对应的项目后。执行Add-Migration Add_Task_Entity，创建迁移。
创建成功后，会在Migrations文件夹下创建时间_Add_Task_Entity格式的类文件。如果注意观察，我们会发现Migrations文件夹下有个SeedData文件夹，顾名思义，这个文件夹下的类主要是用来进行预置种子数据的。我们可以参照已有类的写法，来预置两条Task。创建DefaultTestDataForTask类，代码如下：
```
namespace LearningMpaAbp.Migrations.SeedData
{
  public class DefaultTestDataForTask
  {
      private readonly LearningMpaAbpDbContext _context;

      private static readonly List<Task> _tasks;

      public DefaultTestDataForTask(LearningMpaAbpDbContext context)
      {
          _context = context;
      }

      static DefaultTestDataForTask()
      {
          _tasks = new List<Task>()
          {
              new Task("Learning ABP deom", "Learning how to use abp framework to build a MPA application."),
              new Task("Make Lunch", "Cook 2 dishs")
          };
      }

      public void Create()
      {
          foreach (var task in _tasks)
          {
           if (_context.Tasks.FirstOrDefault(t => t.Title == task.Title) == null)
              {
                  _context.Tasks.Add(task);
              }
              _context.SaveChanges();
          }
      }

  }
}
```
然后在Configuration类中的Seed方法中，添加以下代码。
```
new DefaultTestDataForTask(context).Create();
```
在程序包管理器控制台，输入Update-Database，回车执行迁移。执行成功后，查看数据库，Tasks表创建成功，且表中已存在两条测试数据。
至此，Task实体类成功创建。
 

# 学习ABP系列三——领域层定义仓储并实现
## 一、仓储
仓储（Repository）： 仓储用来操作数据库进行数据存取。仓储接口在领域层定义，而仓储的实现类应该写在基础设施层。

在ABP中，仓储类要实现IRepository接口，接口定义了常用的增删改查以及聚合方法，其中包括同步及异步方法。主要包括以下方法

ABP针对不同的ORM框架对该接口给予了默认的实现；
针对EntityFramework，提供了EfRepositoryBase<TDbContext, TEntity, TPrimaryKey>的泛型版本的实现方式。
针对NHibernate，提供了NhRepositoryBase<TEntity, TPrimaryKey>的泛型版本的实现方式。

泛型版本的实现就意味着，大多数的时候，这些方法已足已应付一般实体的需要。如果这些方法对于实体来说已足够，我们便不需要再去创建这个实体所需的仓储接口/类。

直接通过在应用服务层定义仓储引用，然后通过构造函数注入即可。在我们的应用服务层即可按以下方式使用Task仓储：
```
public class TaskAppService : ITaskAppService { 
private readonly IRepository<Task> _taskRepository; 
public TaskAppService(IRepository<Task> taskRepository) 
{ 
    _taskRepository = taskRepository; 
}
```
## 二、实现自定义仓储
假设我们需要查找某个用户都分配哪些任务。

### 1、在领域层，创建IRepositories文件夹，然后定义IBackendTaskRepository。
```
namespace LearningMpaAbp.IRepositories
{
 /// <summary>
 /// 自定义仓储示例
 /// </summary>
 public interface IBackendTaskRepository : IRepository<Task>
 {
     /// <summary>
     /// 获取某个用户分配了哪些任务
     /// </summary>
     /// <param name="personId">用户Id</param>
     /// <returns>任务列表</returns>
     List<Task> GetTaskByAssignedPersonId(long personId);
 }
}
```
在基础架构层，实现该仓储。
```
namespace LearningMpaAbp.EntityFramework.Repositories
{
 public class BackendTaskRepository:LearningMpaAbpRepositoryBase<Task>,IBackendTaskRepository
 {
     public BackendTaskRepository(IDbContextProvider<LearningMpaAbpDbContext> dbContextProvider) : base(dbContextProvider)
     {
     }

     /// <summary>
     /// 获取某个用户分配了哪些任务
     /// </summary>
     /// <param name="personId">用户Id</param>
     /// <returns>任务列表</returns>
     public List<Task> GetTaskByAssignedPersonId(long personId)
     {
         var query = GetAll();

         if (personId>0)
         {
             query = query.Where(t => t.AssignedPersonId == personId);
         }

         return query.ToList();
     }
 }
}
```
该仓储实现，继承自模板生成的LearningMpaAbpRepositoryBase泛型抽象类，然后再实现IBackendTaskRepository接口。这里要显示声明实现类的有参构造函数，使用泛型的IDbContextProvider将数据库上下文的子类ChargeStationContext传给父类的构造函数。

## 三、仓储的注意事项

### 1、仓储方法中，ABP自动进行数据库连接的开启和关闭。
### 2、仓储方法被调用时，数据库连接自动开启且启动事务。
### 3、当仓储方法调用另外一个仓储的方法，它们实际上共享的是同一个数据库连接和事务。
### 4、仓储对象都是暂时性的，因为IRepository接口默认继承自ITransientDependency接口。所以，仓储对象只有在需要注入的时候，才会由Ioc容器自动创建新实例。
### 5、默认的泛型仓储能满足我们大部分的需求。只有在不满足的情况下，才创建定制化的仓储。

# 学习ABP系列四——创建应用服务
## 一、创建应用服务

# 学习ABP系列五——展现层实现增删改查

# 学习ABP系列六——定义导航菜单

# 学习ABP系列七——分页实现

# 学习ABP系列八——Json格式化

# 学习ABP系列九——权限管理

# 学习ABP系列十——扩展AbpSession

# 学习ABP系列十一——编写单元测试

# 学习ABP系列十二——如何升级Abp并调试源码

# 学习ABP系列十三——Redis缓存用起来

# 学习ABP系列十四——应用BootstrapTable表格插件

# 学习ABP系列十五——创建微信公众号模块


#[学习网址](http://www.jianshu.com/p/a6e9ace79345)

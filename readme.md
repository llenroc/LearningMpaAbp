# Ŀ¼
[01.ѧϰABPϵ��һ����ͨ��ģ�崴��MPA�汾��Ŀ](01.ѧϰABPϵ��һ����ͨ��ģ�崴��MPA�汾��Ŀ)  
[02.ѧϰABPϵ�ж���������㴴��ʵ��](02.ѧϰABPϵ�ж���������㴴��ʵ��)  
[03.ѧϰABPϵ������������㶨��ִ���ʵ��](03.ѧϰABPϵ������������㶨��ִ���ʵ��)  
[04.ѧϰABPϵ���ġ�������Ӧ�÷���](04.ѧϰABPϵ���ġ�������Ӧ�÷���)  
[05.ѧϰABPϵ���塪��չ�ֲ�ʵ����ɾ�Ĳ�](05.ѧϰABPϵ���塪��չ�ֲ�ʵ����ɾ�Ĳ�)  
[06.ѧϰABPϵ�����������嵼���˵�](06.ѧϰABPϵ�����������嵼���˵�)  
[07.ѧϰABPϵ���ߡ�����ҳʵ��](07.ѧϰABPϵ���ߡ�����ҳʵ��)  
[08.ѧϰABPϵ�аˡ���Json��ʽ��](08.ѧϰABPϵ�аˡ���Json��ʽ��)  
[09.ѧϰABPϵ�оš���Ȩ�޹���](09.ѧϰABPϵ�оš���Ȩ�޹���)  
[10.ѧϰABPϵ��ʮ������չAbpSession](10.ѧϰABPϵ��ʮ������չAbpSession)  
[11.ѧϰABPϵ��ʮһ������д��Ԫ����](11.ѧϰABPϵ��ʮ����������д��Ԫ����)  
[12.ѧϰABPϵ��ʮ�������������Abp������Դ��](12.ѧϰABPϵ��ʮ�������������Abp������Դ��)  
[13.ѧϰABPϵ��ʮ������Redis����������](13.ѧϰABPϵ��ʮ������Redis����������)  
[14.ѧϰABPϵ��ʮ�ġ���Ӧ��BootstrapTable�����](14.ѧϰABPϵ��ʮ�ġ���Ӧ��BootstrapTable�����)  
[15.ѧϰABPϵ��ʮ�塪������΢�Ź��ں�ģ��](15.ѧϰABPϵ��ʮ�塪������΢�Ź��ں�ģ��)  


# ѧϰABPϵ��һ����ͨ��ģ�崴��MPA�汾��Ŀ
## һ���ӹ�������ģ��
[������ַ](https://aspnetboilerplate.com)  
ѡ���ҳ�棬����zero,������Ŀ����LearningMpaAbp,������֤������

## ����������Ŀ
### 1����VS2013���ϴ򿪣���ԭNuget��
### 2��������Web��β����ĿΪ������Ŀ
### 3����Web.config���޸������ַ���
```
<add name="Default" connectionString="Data Source=.; Database=LearningMpaAbp; User ID=sa; Password=sa;" providerName="System.Data.SqlClient" />
```
### 4���򿪳��������������̨��ѡ����EntityFramework��β����Ŀ����ִ��Update-Database���Դ������ݿ�
### 5��ģ����Ŀ�����


# ѧϰABPϵ�ж���������㴴��ʵ��
## һ��ABP�ṹ
��������ҵ��㣬��һ����Ŀ�ĺ��ģ�����ҵ�����Ӧ���������ʵ�֡�
ʵ�壨Entity���� ʵ�����ҵ����������ݺͲ�������ʵ���У�ͨ������ӳ������ݿ��
�ִ���Repository���� �ִ������������ݿ�������ݴ�ȡ���ִ��ӿ�������㶨�壬���ִ���ʵ����Ӧ��д�ڻ�����ʩ�㡣
�������Domain service���� �������ҵ������Խ�����������ϣ�ʵ��ʱ��Ӧ��д��������񷽷����档
�����¼���Domain Event���� ���������Щ�ض��������ʱ���Դ��������¼�����������Ӧ�ط����񲢴������ǡ�
������Ԫ��Unit of Work���� ������Ԫ��һ�����ģʽ������ά��һ�����Ѿ����޸�(�����ӡ�ɾ���͸��µ�)��ҵ�������ɵ��б�������Э����Щҵ�����ĳ־û��������������⡣

## �����������
Application		Ӧ�÷����
Core			�����
EntityFramework	������ʩ��
Web��WebApi		Web��չ�ֲ�

## ������Taskʵ��
### 1��������㴴��Tasks�ļ��У�������Taskʵ����
### 2��ABP�����е�ʵ���඼�̳���Entity����Entityʵ����IEntity�ӿڣ���IEntity�ӿ���һ�����ͽӿڣ�ͨ������ָ������Id���ͣ�Ĭ�ϵ�Entity������������int���͡�
����Task���϶���Ҫ���洴��ʱ�䣬����ͨ��ʵ�����ģ���е�IHasCreationTime��ʵ������ͨ�ù��ܡ���������
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
���ж�����TaskState״̬ö�١��������AssignedPerson�������ԣ����������������ĳ���û�������[Required]��[MaxLength]������������������У��ġ�
### 3�������ʵ��֮�����Ǿ�ҪȥDbContext�ж���ʵ���Ӧ��DbSet����Ӧ��Code First ����Ǩ�ơ��ҵ����ǵĻ�������㣬����EntityFramework��β����Ŀ�У��ҵ�DbContext�࣬������´���
```
//TODO: Define an IDbSet for your Entities...
 public IDbSet<Task> Tasks { get; set; }
```
### 4��ִ��Code First����Ǩ�ơ�
�򿪳��������������̨��Ĭ����Ŀѡ��Entityframework��Ӧ����Ŀ��ִ��Add-Migration Add_Task_Entity������Ǩ�ơ�
�����ɹ��󣬻���Migrations�ļ����´���ʱ��_Add_Task_Entity��ʽ�����ļ������ע��۲죬���ǻᷢ��Migrations�ļ������и�SeedData�ļ��У�����˼�壬����ļ����µ�����Ҫ����������Ԥ���������ݵġ����ǿ��Բ����������д������Ԥ������Task������DefaultTestDataForTask�࣬�������£�
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
Ȼ����Configuration���е�Seed�����У�������´��롣
```
new DefaultTestDataForTask(context).Create();
```
�ڳ��������������̨������Update-Database���س�ִ��Ǩ�ơ�ִ�гɹ��󣬲鿴���ݿ⣬Tasks�����ɹ����ұ����Ѵ��������������ݡ�
���ˣ�Taskʵ����ɹ�������
 

# ѧϰABPϵ������������㶨��ִ���ʵ��
## һ���ִ�
�ִ���Repository���� �ִ������������ݿ�������ݴ�ȡ���ִ��ӿ�������㶨�壬���ִ���ʵ����Ӧ��д�ڻ�����ʩ�㡣

��ABP�У��ִ���Ҫʵ��IRepository�ӿڣ��ӿڶ����˳��õ���ɾ�Ĳ��Լ��ۺϷ��������а���ͬ�����첽��������Ҫ�������·���

ABP��Բ�ͬ��ORM��ܶԸýӿڸ�����Ĭ�ϵ�ʵ�֣�
���EntityFramework���ṩ��EfRepositoryBase<TDbContext, TEntity, TPrimaryKey>�ķ��Ͱ汾��ʵ�ַ�ʽ��
���NHibernate���ṩ��NhRepositoryBase<TEntity, TPrimaryKey>�ķ��Ͱ汾��ʵ�ַ�ʽ��

���Ͱ汾��ʵ�־���ζ�ţ��������ʱ����Щ����������Ӧ��һ��ʵ�����Ҫ�������Щ��������ʵ����˵���㹻�����Ǳ㲻��Ҫ��ȥ�������ʵ������Ĳִ��ӿ�/�ࡣ

ֱ��ͨ����Ӧ�÷���㶨��ִ����ã�Ȼ��ͨ�����캯��ע�뼴�ɡ������ǵ�Ӧ�÷���㼴�ɰ����·�ʽʹ��Task�ִ���
```
public class TaskAppService : ITaskAppService { 
private readonly IRepository<Task> _taskRepository; 
public TaskAppService(IRepository<Task> taskRepository) 
{ 
    _taskRepository = taskRepository; 
}
```
## ����ʵ���Զ���ִ�
����������Ҫ����ĳ���û���������Щ����

### 1��������㣬����IRepositories�ļ��У�Ȼ����IBackendTaskRepository��
```
namespace LearningMpaAbp.IRepositories
{
 /// <summary>
 /// �Զ���ִ�ʾ��
 /// </summary>
 public interface IBackendTaskRepository : IRepository<Task>
 {
     /// <summary>
     /// ��ȡĳ���û���������Щ����
     /// </summary>
     /// <param name="personId">�û�Id</param>
     /// <returns>�����б�</returns>
     List<Task> GetTaskByAssignedPersonId(long personId);
 }
}
```
�ڻ����ܹ��㣬ʵ�ָòִ���
```
namespace LearningMpaAbp.EntityFramework.Repositories
{
 public class BackendTaskRepository:LearningMpaAbpRepositoryBase<Task>,IBackendTaskRepository
 {
     public BackendTaskRepository(IDbContextProvider<LearningMpaAbpDbContext> dbContextProvider) : base(dbContextProvider)
     {
     }

     /// <summary>
     /// ��ȡĳ���û���������Щ����
     /// </summary>
     /// <param name="personId">�û�Id</param>
     /// <returns>�����б�</returns>
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
�òִ�ʵ�֣��̳���ģ�����ɵ�LearningMpaAbpRepositoryBase���ͳ����࣬Ȼ����ʵ��IBackendTaskRepository�ӿڡ�����Ҫ��ʾ����ʵ������вι��캯����ʹ�÷��͵�IDbContextProvider�����ݿ������ĵ�����ChargeStationContext��������Ĺ��캯����

## �����ִ���ע������

### 1���ִ������У�ABP�Զ��������ݿ����ӵĿ����͹رա�
### 2���ִ�����������ʱ�����ݿ������Զ���������������
### 3�����ִ�������������һ���ִ��ķ���������ʵ���Ϲ������ͬһ�����ݿ����Ӻ�����
### 4���ִ���������ʱ�Եģ���ΪIRepository�ӿ�Ĭ�ϼ̳���ITransientDependency�ӿڡ����ԣ��ִ�����ֻ������Ҫע���ʱ�򣬲Ż���Ioc�����Զ�������ʵ����
### 5��Ĭ�ϵķ��Ͳִ����������Ǵ󲿷ֵ�����ֻ���ڲ����������£��Ŵ������ƻ��Ĳִ���

# ѧϰABPϵ���ġ�������Ӧ�÷���
## һ������Ӧ�÷���

# ѧϰABPϵ���塪��չ�ֲ�ʵ����ɾ�Ĳ�

# ѧϰABPϵ�����������嵼���˵�

# ѧϰABPϵ���ߡ�����ҳʵ��

# ѧϰABPϵ�аˡ���Json��ʽ��

# ѧϰABPϵ�оš���Ȩ�޹���

# ѧϰABPϵ��ʮ������չAbpSession

# ѧϰABPϵ��ʮһ������д��Ԫ����

# ѧϰABPϵ��ʮ�������������Abp������Դ��

# ѧϰABPϵ��ʮ������Redis����������

# ѧϰABPϵ��ʮ�ġ���Ӧ��BootstrapTable�����

# ѧϰABPϵ��ʮ�塪������΢�Ź��ں�ģ��


#[ѧϰ��ַ](http://www.jianshu.com/p/a6e9ace79345)

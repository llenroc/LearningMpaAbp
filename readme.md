##学习ABP
##一、从官网下载模板
[官网地址](https://aspnetboilerplate.com)
选择多页面，包含zero,输入项目名称LearningMpaAbp,输入验证码下载

##二、启动项目
###1、用VS2013以上打开，还原Nuget包
###2、设置以Web结尾的项目为启动项目
###3、打开Web.config，修改连接字符串
<add name="Default" connectionString="Data Source=.; Database=LearningMpaAbp; User ID=sa; Password=sa;" providerName="System.Data.SqlClient" />
###4、打开程序包管理器控制台，选择以EntityFramework结尾的项目，并执行Update-Database，以创建数据库
###5、模板项目己完成

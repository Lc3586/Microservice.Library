达梦.Net驱动分为DmProvider、EFDmProvider、EFCore.Dm、DmDialect和DmConnect。

DmProvider可以在.NET框架和NETCore框架下使用，NETCore框架下需要用户安装System.Text.Encoding.CodePages包或者直接以NUGET包的形式安装DmProvider,可以自动依赖的System.Text.Encoding.CodePages包

其中EFDmProvider是支持Entity Framework框架的驱动，它与数据库交互的部分由DmProvider完成，所以如果程序中需要使用EFDmProvider，需要同时引用DmProvider
DmConnect是达梦提供给VS的DDEX驱动，它也引用了DmProvider。

EFCore.Dm已支持EFCore2.1版本

DmDialect方言包有for Nhibernate3、for Nhibernate4、for Nhibernate5分别对应NET3.5、NET4、NET4.6.1;用户可根据开发环境选择对应的方言包版本;
Nhibernate中App.config配置要求：
1、驱动名称
<property name="connection.driver_class">NHibernate.Driver.DmDriver, DmDialect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=072d25982b139bf8</property> 
2、方言包名称
<property name="dialect">NHibernate.Dialect.DmDialect, DmDialect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=072d25982b139bf8</property>


文件结构说明：
DmProvider文件夹中是完整的DmProvider驱动文件。使用DmProvider的DmBulkCopy对象，需要引用dmfldr_dll.dll以及此dll依赖的其他库。
EFDmProvider文件夹中是老版本的EFDmProvider，已不再更新版本。
EFDmProvider6.1.3-net40文件夹中是基于EntityFramework6.1.3及.Net4.0的EFDmProvider 2.0版本。
EFDmProvider6.1.3-net45文件夹中是基于EntityFramework6.1.3及.Net4.5的EFDmProvider 2.0版本。
DmConnect文件夹中是DmConnect驱动及所需文件。
DmDialect文件夹是不同版本NHibernate的方言包
gacutil.exe是全局程序集缓存工具,使用它可以将.Net驱动加载到程序集中。
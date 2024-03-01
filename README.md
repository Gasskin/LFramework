框架本体来自于E大的UGF：https://github.com/EllanJiang/UnityGameFramework

# 0.文件结构
```
Assets
├────LFramework
|    ├────Library                       // 框架层的第三方库
|    |    ├── ...
|    |    ├── GameFramework             // GF的源码
|    |    └── GameFrameworkEx           // 扩展GF源码，写在这里
|    |                   
|    ├────UnityGameFramework            // UGF
|    |    ├── Editor            
|    |    └── Runtime
|    |                
|    └────UnityGameFrameworkEx          // UGF扩展
|         ├── Editor      
|         └── Runtime
|
└────Game                               // 业务层
     ├── ...                   
     ├── Library                        // 业务层的第三方库
     ├── Module                         // 业务层框架
     └── Logic                          // 业务代码
```

# 替换GameFramwork的DLL为源码

源码地址：https://github.com/EllanJiang/GameFramework

# 配表工具LuBan7（next版）

Game/Module/Config

配表定义在：../Assets/LuBan7

LuBan修改过导表模板，导出的Tables可以异步初始化

```c#
// 首先需要初始化
var comp = GameEntry.GetComponent<LuBanComponent>();
await comp.InitAsync();
// 之后就可以直接使用了
var comp = GameEntry.GetComponent<LuBanComponent>();
comp.AllTabls.XXX
```

# 引入异步库UniTask

同时扩展了一些异步接口，比如资源加载

```c#
var asset = await comp.LoadAssetAsync<TextAsset>("xxx");
```

# 协程锁

参考ET实现了一遍，加入了GF的一些池子做缓存，使用方法和ET一致

```c#
var comp = GameEntry.GetComponent<CoroutineLockComponent>();
using(var coroutineLock = await comp.Wait(ECoroutineLockType.Test,1))
...
```
# 配合插件Rewired实现的InputModule

Game/Module/Input

# 实现了一套Entity-Component的EC框架

详见Game/Module/Entity

组件式编程

同时实现了一个属性系统组件，AttrCompoent

# 引入ScivoloCharacterController插件

一个辅助制作PlayerController的插件

配合EC框架制作角色控制器

# 角色控制器

Game/Logic/Character

## CharacterControllerEntity

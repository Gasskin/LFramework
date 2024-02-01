框架本体来自于E大的UGF：https://github.com/EllanJiang/UnityGameFramework

# 0.文件结构
```
Core 
├────LFramework              // 自己扩展的框架
|    ├── Editor            
|    └── Runtime
|          
├────Library                 // 第三方库，GameFramwork源码也在里面
|    ├── Editor      
|    └── Runtime
|        ├── GameFramework   // GF的源码
|        └── LFramework      // 扩展GF源码，写在这里
|      
└────UnityGameFramework      // UGF源码
     ├── Editor      
     └── Runtime       
```

# 1.替换GameFramwork的DLL为源码
源码地址：https://github.com/EllanJiang/GameFramework

# 2.配表工具LuBan7（next版）
LuBan修改过导表模板，导出的Tables可以异步初始化
```c#
// 首先需要在LuBanInitProcedure中初始化
var comp = GameEntry.GetComponent<LuBanComponent>();
await comp.InitAsync();
// 之后就可以直接使用了
var comp = GameEntry.GetComponent<LuBanComponent>();
comp.AllTabls.XXX
```

# 3.异步库UniTask
扩展了一些异步接口，比如资源加载
```c#
var asset = await comp.LoadAssetAsync<TextAsset>("xxx");
```

# 4.ET的协程锁机制
参考ET实现了一遍，加入了GF的一些池子做缓存，使用方法和ET一致
```c#
var comp = GameEntry.GetComponent<CoroutineLockComponent>();
using(var coroutineLock = await comp.Wait(ECoroutineLockType.Test,1))
...
```

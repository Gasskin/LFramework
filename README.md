框架本体来自于E大的UGF：https://github.com/EllanJiang/UnityGameFramework

# 0.文件结构
├── Core // 框架总入口
├──────LFramework           // 自己扩展的框架
|      ├── Editor            
|      └── Runtime          
├──────Library              // 第三方库，GameFramwork源码也在里面
|      ├── Editor      
|      └── Runtime      
└──────UnityGameFramewokr   // UGF源码
       ├── Editor      
       └── Runtime       


# 1.替换GameFramwork的DLL为源码
源码地址：https://github.com/EllanJiang/GameFramework

# 2.引入配表工具LuBan
LuBan修改过源码，导出的Tables可以异步初始胡
源码仓库：https://github.com/Me-Maped/Gameframework-at-FairyGUI
```c#
// 首先需要在LuBanInitProcedure中初始化
var comp = GameEntry.GetComponent<LuBanComponent>();
await comp.InitAsync();
// 之后就可以直接使用了
var comp = GameEntry.GetComponent<LuBanComponent>();
comp.AllTabls.XXX
```

# 3.引入异步框架UniTask
扩展了一些异步接口，比如资源加载
```c#
var asset = await comp.LoadAssetAsync<TextAsset>("xxx");
```

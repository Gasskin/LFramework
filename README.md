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


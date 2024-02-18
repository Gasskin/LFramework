using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Entitas.CodeGeneration.Plugins;
using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    public static class ResourcesPathTool
    {
        private static readonly string[] Filter =
        {
            "AssetsPackage\\Core",
            "AssetsPackage\\Scene",
            "AssetsPackage\\ModelPrefabs",
        };
        
        private static readonly string SearchPath = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "AssetsPackage"));
        private static readonly string TargetPath = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "LFramework/UnityGameFrameworkEx/Runtime/Utility/ResourcePathConfig/Generate/ResourcePathConfig.cs"));
        private static readonly Dictionary<string, List<string>> Classify = new();

        [MenuItem("LFramework/Custom Tools/Resources/Gen Resources Path Config")]
        public static void Gen()
        {
            var files = Directory.GetFiles(SearchPath,"*", SearchOption.AllDirectories);
            foreach (var filePath in files)
            {
                bool flag;
                
                if (filePath.EndsWith(".meta"))
                {
                    flag = false;
                }
                else
                {
                    flag = false;
                    foreach (var filter in Filter)
                    {
                        if (filePath.Contains(filter))
                        {
                            flag = true;
                            break;
                        }
                    }
                }

                if (!flag)
                {
                    continue;
                }


                var fileName = Path.GetFileName(filePath);
                var pattern = @"[-.]";
                if (Regex.Match(fileName.Split(".")[0],pattern).Success)
                {
                    Debug.LogError($"不符合要求的命名：{fileName}");
                    continue;
                }
                
                var className = filePath.Replace($"{SearchPath}\\", "");
                if (className == fileName)
                {
                    className = "Empty";
                }
                else
                {
                    className = className.Replace($"\\{fileName}", "");
                    className = className.Replace("\\", ".");
                }

                if (Classify.TryGetValue(className,out var list))
                {
                    list.Add(fileName);
                }
                else
                {
                    Classify.Add(className, new List<string> { fileName });
                }
            }
            
            var sb = new StringBuilder();
            sb.AppendLine("//===============");
            sb.AppendLine("//Auto-Generated");
            sb.AppendLine("//===============");
            sb.AppendLine();
            sb.AppendLine("namespace UnityGameFramework.Runtime");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class ResourcesPathConfig");
            sb.AppendLine("\t{");
            foreach (var classify in Classify)
            {
                var path = Utility.Path.GetRegularPath(Path.Combine("Assets/AssetsPackage", classify.Key.Replace("_", "/")));
                sb.AppendLine($"\t\tpublic static class {classify.Key}");
                sb.AppendLine("\t\t{");
                foreach (var fileName in classify.Value)
                {
                    var name = fileName.Split(".")[0];
                    sb.AppendLine($"\t\t\tpublic static readonly string {name} = \"{Utility.Path.GetRegularPath(Path.Combine(path, fileName))}\";");
                }
                sb.AppendLine("\t\t}");
                sb.AppendLine();
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(TargetPath, sb.ToString());
        }
    }
}
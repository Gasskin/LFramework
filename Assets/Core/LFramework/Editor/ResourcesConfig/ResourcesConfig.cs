using System.IO;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Editor.ResourceTools;

namespace LFramework
{
    public static class ResourcesConfig
    {
        [ResourceBuilderConfigPath]
        public static string ResourceBuilderConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "Core/LFramework/Editor/ResourcesConfig/ResourceBuilder.xml"));
        [ResourceEditorConfigPath]
        public static string ResourceEditorConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "Core/LFramework/Editor/ResourcesConfig/ResourceEditor.xml"));
        [ResourceCollectionConfigPath]
        public static string ResourceCollectionConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "Core/LFramework/Editor/ResourcesConfig/ResourceCollection.xml"));
    }
}
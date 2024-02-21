using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Game.Module.Entity
{
    public class ComponentView : MonoBehaviour
    {
        public readonly List<EntityComponent> m_Components = new();

        private readonly List<Vector3> m_Rgb = new()
        {
            new Vector3(220, 255, 183),
            new Vector3(255, 104, 104),
            new Vector3(255, 187, 100),
            new Vector3(255, 234, 167),
        };
        private readonly List<Color> m_Colors = new();
        private Dictionary<string, bool> m_Draw = new();
        
        
        private GUIStyle m_TextField;

        private void OnEnable()
        {
            m_Colors.Clear();
            foreach (var rgb in m_Rgb)
            {
                m_Colors.Add(new Color(rgb.x / 255f, rgb.y / 255f, rgb.z / 255f, 1f));
            }
        }

        [OnInspectorGUI]
        private void OnInspectGUI()
        {
            m_TextField = new GUIStyle("LabelField")
            {
                fontStyle = FontStyle.Bold
            };

            for (int i = 0; i < m_Components.Count; i++)
            {
                var component = m_Components[i];
                var type = component.GetType();
                m_TextField.normal.textColor = m_Colors[i % m_Colors.Count];

                m_Draw.TryAdd(type.Name, false);
                EditorGUILayout.BeginHorizontal();
                m_Draw[type.Name] = EditorGUILayout.Toggle(m_Draw[type.Name],GUILayout.Width(20));
                EditorGUILayout.TextField(type.Name, m_TextField);
                EditorGUILayout.EndHorizontal();

                if (!m_Draw[type.Name])
                {
                    continue;
                }
                
                // Go组件展示其Entity的属性，其他组件展示自身的属性
                if (type == typeof(GameObjectComponent))
                {
                    var entityType = component.Entity.GetType();
                    if (entityType.GetCustomAttribute<DrawEntityPropertyAttribute>() != null)
                    {
                        EditorGUILayout.TextArea(component.Entity.ToString());
                    }
                }
                else
                {
                    if (type.GetCustomAttribute<DrawEntityPropertyAttribute>() != null)
                    {
                        EditorGUILayout.TextArea(component.ToString());
                    }
                }
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Vanta.Core
{
    
    public class EditorUtilities
    {

        public static string CreateEnumString(string name, List<string> entries, int tabCount = 0)
        {
            var tab = "";
            for (int i = 0; i < tabCount; i++) tab += "\t";
            
            string enumCode = tab + "public enum " + name + "\n" + tab + "{\n";
            for (int i = 0; i < entries.Count; i++)
            {
                enumCode += tab + "\t" + entries[i] + " = " + i;
                enumCode += i < entries.Count - 1 ? ",\n" : "\n";
            }
            enumCode += tab + "}";

            return enumCode;
        }
        
        public bool HasType(string typeName) 
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
            {
                foreach (Type type in assembly.GetTypes()) 
                {
                    if (type.Name == typeName)
                    {
                        return true;
                    }
                }
            }
 
            return false;
        }
        
        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum 
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
        
        public static void HorizontalLine ( Color color ) {
            GUIStyle horizontalLine;
            horizontalLine = new GUIStyle();
            horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
            horizontalLine.fixedHeight = 1;
            
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box( GUIContent.none, horizontalLine );
            GUI.color = c;
        }

        public static T GetPrivateProperty<T>(object obj, string propertyName)
        {
            return (T) obj.GetType()
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(obj);
        }

        public static void SetPrivateProperty<T>(object obj, string propertyName, T value)
        {
            obj.GetType()
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(obj, value);
        }
        
        public static T GetPrivateField<T>(object obj, string fieldName)
        {
            return (T) obj.GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(obj);
        }

        public static void SetPrivateField<T>(object obj, string fieldName, T value)
        {
            obj.GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(obj, value);
        }
        
    }

}



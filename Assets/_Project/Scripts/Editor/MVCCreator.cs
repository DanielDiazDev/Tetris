using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    public class MVCCreator : EditorWindow
    {
        private string scriptName = "NewMVC";
        private string targetPath = "Assets"; // Ruta por defecto

        [MenuItem("Tools/Create MVC Scripts")]
        public static void ShowWindow()
        {
            GetWindow<MVCCreator>("MVC Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("MVC Generator Settings",  EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            // 1. Campo para el nombre
            scriptName = EditorGUILayout.TextField("Base Name", scriptName);
            GUILayout.Space(5);
            
            // 2. Sección para elegir la ruta
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Save Path");
            // Mostramos la ruta, pero deshabilitamos la edición manual para evitar errores de escritura
            GUI.enabled = false; 
            EditorGUILayout.TextField(targetPath);
            GUI.enabled = true;
            if(GUILayout.Button("...", GUILayout.Width(30)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "Assets/_Project/Scripts", "");
                // Validamos que la ruta esté dentro del proyecto
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (selectedPath.StartsWith(Application.dataPath))
                    {
                        // Convertimos la ruta absoluta a relativa (ej: Assets/Scripts/...)
                        targetPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error",
                            "Please select a folder inside the project's Assets folder.", "OK");
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            
            // 3. Botón de Generar
            if (GUILayout.Button("Generate Scripts", GUILayout.Height(40)))
            {
                if (!string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(targetPath))
                {
                    MVCScriptGenerator.CreateMVCScripts(scriptName, targetPath);
                
                    // Opcional: Cerrar ventana al terminar
                    // Close(); 
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please enter a valid name and path.", "OK");
                }
            }
        }
    }

    public class MVCScriptGenerator
    {
        // Ahora recibimos 'rootPath' como argumento
        public static void CreateMVCScripts(string baseName, string rootPath)
        {
            // Creamos una subcarpeta con el nombre del módulo para mantener el orden
            string fullPath = Path.Combine(rootPath, baseName);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            // Creamos los 3 scripts
            CreateScriptFromTemplate(fullPath, $"{baseName}Model.cs", "01-C# MVC__ModelTemplate-NewModel.cs.txt", baseName);
            CreateScriptFromTemplate(fullPath, $"{baseName}Controller.cs", "02-C# MVC__ControllerTemplate-NewController.cs.txt", baseName);
            CreateScriptFromTemplate(fullPath, $"{baseName}View.cs", "03-C# MVC__ViewTemplate-NewView.cs.txt", baseName);

            // Refrescamos la base de datos de Unity para que aparezcan los archivos
            AssetDatabase.Refresh();
        
            Debug.Log($" MVC Scripts for '{baseName}' created successfully in: {fullPath}");
        }

        private static void CreateScriptFromTemplate(string path, string fileName, string templateFile, string baseName)
        {
            // Asegúrate de que esta carpeta 'ScriptTemplates' exista en tu proyecto dentro de Assets
            string templatePath = Path.Combine(Application.dataPath, "ScriptTemplates", templateFile);
            if (!File.Exists(templatePath))
            {
                Debug.LogError($" Template file not found at: {templatePath}");
                return;
            }

            string content = File.ReadAllText(templatePath);
        
            // Reemplazo del placeholder
            content = content.Replace("#SCRIPTNAME#", baseName);
        
            // Escritura del archivo final
            File.WriteAllText(Path.Combine(path, fileName), content);
        }
    }
}
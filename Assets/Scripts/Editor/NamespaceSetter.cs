using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class NamespaceSetter
{
    private const string TargetNamespace = "Tour_ENDI_TourStub1"; // ← измени здесь

    [MenuItem("Tools/Set Namespace")]
    private static void ForceSetNamespace()
    {
        var selected = Selection.activeObject;

        if (selected == null)
        {
            Debug.LogError("Выберите папку.");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(selected);

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("Нужно выбрать папку.");
            return;
        }

        string absolutePath = Path.GetFullPath(folderPath);
        var files = Directory.GetFiles(absolutePath, "*.cs", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            ProcessFile(file);
        }

        AssetDatabase.Refresh();
        Debug.Log("Namespace успешно обновлён.");
    }

    private static void ProcessFile(string filePath)
    {
        string content = File.ReadAllText(filePath);

        // --- 1. Обработка file-scoped namespace ---
        var fileScopedMatch = Regex.Match(content, @"^namespace\s+[a-zA-Z0-9_.]+\s*;", RegexOptions.Multiline);

        if (fileScopedMatch.Success)
        {
            content = Regex.Replace(
                content,
                @"^namespace\s+[a-zA-Z0-9_.]+\s*;",
                $"namespace {TargetNamespace};",
                RegexOptions.Multiline
            );

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
            return;
        }

        // --- 2. Обработка обычного namespace ---
        var blockMatch = Regex.Match(content, @"namespace\s+[a-zA-Z0-9_.]+\s*\{");

        if (blockMatch.Success)
        {
            content = Regex.Replace(
                content,
                @"namespace\s+[a-zA-Z0-9_.]+\s*\{",
                $"namespace {TargetNamespace}\n{{"
            );

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
            return;
        }

        // --- 3. Если namespace нет — добавляем ---
        AddNamespaceToFileWithoutNamespace(filePath, content);
    }

    private static void AddNamespaceToFileWithoutNamespace(string filePath, string content)
    {
        var lines = File.ReadAllLines(filePath);
        StringBuilder sb = new StringBuilder();

        int index = 0;

        // Копируем using + пустые строки после них
        while (index < lines.Length &&
               (lines[index].TrimStart().StartsWith("using") ||
                string.IsNullOrWhiteSpace(lines[index])))
        {
            sb.AppendLine(lines[index]);
            index++;
        }

        sb.AppendLine($"namespace {TargetNamespace}");
        sb.AppendLine("{");

        for (int i = index; i < lines.Length; i++)
        {
            sb.AppendLine("    " + lines[i]);
        }

        sb.AppendLine("}");

        File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(false));
    }
}

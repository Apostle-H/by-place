using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Services
{
    public class AssetsService
    {
        public static string InstanceIdToPath(int instanceId) => 
            AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceId));
        
        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            var foldersNames = path.Split('/');
            var parentFolderPath = string.Empty;
            foreach (var folderName in foldersNames)
            {
                if (!AssetDatabase.IsValidFolder(parentFolderPath + folderName))
                    AssetDatabase.CreateFolder(parentFolderPath[..^1], folderName);

                parentFolderPath += $"{folderName}/";
            }

            var fullPath = $"{path}/{assetName}.asset";
            var asset = LoadAsset<T>(path, assetName);

            if (asset != null) 
                return asset;
            
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);

            return asset;
        }

        public static T CreateSubAsset<T>(ScriptableObject parent, string assetName) where T : ScriptableObject
        {
            var asset = (T)AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(parent))
                .FirstOrDefault(asset => asset.name == assetName);
            if (asset != default)
                return asset;
            asset = ScriptableObject.CreateInstance<T>();
            asset.name = assetName;
            AssetDatabase.AddObjectToAsset(asset, parent);

            return asset;
        }
        
        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject => 
            AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");

        public static T LoadAsset<T>(int instanceId) where T : ScriptableObject =>
            AssetDatabase.LoadAssetAtPath<T>(InstanceIdToPath(instanceId));
        
        public static void SaveAsset(Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DeleteAsset(int instanceId)
        {
            if (AssetDatabase.IsSubAsset(instanceId))
                DeleteSubAsset(instanceId);
            else
                DeleteRootAsset(instanceId);
        }
        
        private static void DeleteRootAsset(int instanceId) => AssetDatabase.DeleteAsset(InstanceIdToPath(instanceId));

        private static void DeleteSubAsset(int instanceId) => Object.DestroyImmediate(EditorUtility.InstanceIDToObject(instanceId), true);

        public static void RenameAsset(int instanceId, string newName) => 
            AssetDatabase.RenameAsset(InstanceIdToPath(instanceId), newName);

        public static void MoveAsset(int instanceId, int parentInstanceId)
        {
            if (AssetDatabase.IsSubAsset(instanceId))
                DeleteSubAsset(instanceId);
            else
                AssetDatabase.MoveAsset(InstanceIdToPath(instanceId), InstanceIdToPath(parentInstanceId));
        }

        public static void MoveAsset(int instanceId, string newPath) => 
            AssetDatabase.MoveAsset(InstanceIdToPath(instanceId), newPath);
    }
}
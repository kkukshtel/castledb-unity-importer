using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace CastleDBImporter
{
    [ScriptedImporter(1, "cdb")]
    public class CastleDBImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset castle = new TextAsset(File.ReadAllText(ctx.assetPath));

            ctx.AddObjectToAsset("main obj", castle);
            ctx.SetMainObject(castle);

            CastleDBParser newCastle = new CastleDBParser(castle);
            CastleDBGenerator.GenerateTypes(newCastle.Root, GetCastleDBConfig());
        }

        public CastleDBConfig GetCastleDBConfig()
        {
            var guids = AssetDatabase.FindAssets("CastleDBConfig t:CastleDBConfig");
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath(path, typeof(CastleDBConfig)) as CastleDBConfig;
        }
    }
}
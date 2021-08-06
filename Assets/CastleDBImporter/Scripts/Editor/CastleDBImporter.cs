using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace CastleDBImporter
{
	[ScriptedImporter(1, "cdb")]
	public class CastleDBImporter : ScriptedImporter
	{
        private CastleDBParser parser = null;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			TextAsset castle = new TextAsset(File.ReadAllText(ctx.assetPath));

			ctx.AddObjectToAsset("main obj", castle);
			ctx.SetMainObject(castle);

            parser = new CastleDBParser(castle);

            EditorApplication.delayCall += new EditorApplication.CallbackFunction(GenerateTypes); // Delay type generation until the asset manager has finished importing
		}

		public CastleDBConfig GetCastleDBConfig()
		{
			var guids = AssetDatabase.FindAssets("CastleDBConfig t:CastleDBConfig");
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath(path, typeof(CastleDBConfig)) as CastleDBConfig;
		}

        private void GenerateTypes()
        {
            CastleDBGenerator.GenerateTypes(parser.Root, GetCastleDBConfig());
            parser = null;
        }
	}
}
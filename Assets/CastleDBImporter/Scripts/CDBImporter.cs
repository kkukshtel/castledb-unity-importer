using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace CastleDBImporter
{
	[ScriptedImporter(1, "cdb")]
	public class CDBImporter : ScriptedImporter
	{
		public float m_Scale = 1;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			// CastleDB castle = ScriptableObject.CreateInstance<CastleDB>();
			// CastleDB castle = new CastleDBImporter.CastleDB(File.ReadAllText(ctx.assetPath));
			TextAsset castle = new TextAsset(File.ReadAllText(ctx.assetPath));
			// castle.Init(File.ReadAllText(ctx.assetPath));

			// JsonUtility.FromJsonOverwrite(File.ReadAllText(ctx.assetPath), scriptable);
			// var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			// var position = JsonUtility.FromJson<Vector3>(File.ReadAllText(ctx.assetPath));

			// cube.transform.position = position;
			// cube.transform.localScale = new Vector3(m_Scale, m_Scale, m_Scale);

			// 'cube' is a a GameObject and will be automatically converted into a prefab
			// (Only the 'Main Asset' is elligible to become a Prefab.)

			ctx.AddObjectToAsset("main obj", castle);
			ctx.SetMainObject(castle);

			CastleDBParser newCastle = new CastleDBParser(castle);
			// CastleAssemblyGenerator generator = new CastleAssemblyGenerator();
            // generator.GenerateAssemblies(newCastle.GenerateDB());
			AssembyBuilderExample.BuildAssembly(newCastle.Root);
		}
	}
}
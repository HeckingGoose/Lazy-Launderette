using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "osf")]
public class OysterImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        TextAsset subAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("text", subAsset);
        ctx.SetMainObject(subAsset);

        // Yes I found this on a forum post
        // Here's the link:
        // https://discussions.unity.com/t/loading-a-file-with-a-custom-extension-as-a-textasset/731294/5
    }
}

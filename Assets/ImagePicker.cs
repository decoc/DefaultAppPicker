using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class ImagePicker : MonoBehaviour
{
    public Text textUI;
    public Image imageUI;

#if WINDOWS_UWP
    async void Start()
    {
        var args = UnityEngine.WSA.Application.arguments; //System.Environment.GetCommandLineArgs();
        textUI.text += $"{args}";

        if (args.EndsWith(".png"))
        {
            var bytes = new byte[]{};
            var pictureFile = await StorageFile.GetFileFromPathAsync(@$"{args}");
            bytes = await GetByteFromFile(pictureFile);
            var tex = new Texture2D(100, 100);
            tex.LoadImage(bytes);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            imageUI.sprite = sprite;
        }
    }
#endif
    
#if WINDOWS_UWP
    private static async Task<byte[]> GetByteFromFile(StorageFile storageFile)
    {
        var stream = await storageFile.OpenReadAsync();

        using (var dataReader = new DataReader(stream))
        {
            var bytes = new byte[stream.Size];
            await dataReader.LoadAsync((uint)stream.Size);
            dataReader.ReadBytes(bytes);

            return bytes;
        }
    }    
#endif
}

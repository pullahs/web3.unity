using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ImportNFTTextureExample : MonoBehaviour
{
    public class Response {
        public string image;
    }

    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0x3a8A85A6122C92581f590444449Ca9e66D8e8F35";
        string tokenId = "5";

        // fetch uri from chain
        string uri = await ERC1155.URI(chain, network, contract, tokenId);
        print("uri: " + uri);

        // fetch json from uri
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            await webRequest.SendWebRequest();
            try
            {
                Response data = JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            }
            // dispose webrequest after its used.
            finally
            {
                webRequest.Dispose();
            }
        }


        // parse json to get image uri
        string imageUri = data.image;
        print("imageUri: " + imageUri);

        // fetch image and display in game
        using (UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri))
        {
            await textureRequest.SendWebRequest();
            try
            {
                this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            }
            // dispose webrequest after its used.
            finally
            {
                textureRequest.Dispose();
            }
        }

    }
}

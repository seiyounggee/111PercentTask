using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureManager : MonoSingleton<RenderTextureManager>
{
    [SerializeField] Camera renderCam;

    public Player RenderTexturePlayerCharacter { get; set; }

    public void ActivateRenderTexture_Player(int _characterSkinID = -1) 
    {
        if (RenderTexturePlayerCharacter != null)
        {
            Destroy(RenderTexturePlayerCharacter.gameObject);
            RenderTexturePlayerCharacter = null;
        }

        var prefab = PrefabManager.Instance.PlayerPrefab;
        GameObject playerObj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.Euler(new Vector3(0f, -200f, 0f)));
        playerObj.layer = UnityEngine.LayerMask.NameToLayer(CommonDefine.LayerName_Player);
        playerObj.name = "RenderTexture_Player";

        RenderTexturePlayerCharacter = playerObj.GetComponent<Player>();
        RenderTexturePlayerCharacter.Setup(Player.Type.OutgamePlayer);
        RenderTexturePlayerCharacter.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        RenderTexturePlayerCharacter.transform.position = renderCam.transform.position + renderCam.transform.forward * 7.2f;
        RenderTexturePlayerCharacter.transform.position -= Vector3.up * 1f;

        renderCam.SafeSetActive(true);
    }
}

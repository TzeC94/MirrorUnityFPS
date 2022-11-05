using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItemScript : MonoBehaviour
{
    public ItemData itemData;
    public Image ui_Icon;

    public void Setup() {

        ResourceManage.LoadAsset<Sprite>(itemData.itemIcon, LoadComplete);

    }

    private void LoadComplete(Sprite itemSprite) {

        ui_Icon.sprite = itemSprite;

    }
    
}

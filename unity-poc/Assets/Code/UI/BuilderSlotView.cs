using System;
using DG.Tweening;
using UFTM;
using UFTM.Datatypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class BuilderSlotView : AbstractView, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public RawImage Icon;
        public Image Overlay;
        private string slotName;

        public Action<string> OnTooltip;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnTooltip != null)
            {
                OnTooltip(slotName);
            }
            
            Debug.Log(slotName);

            Overlay.DOFade(1f, 0.3f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnTooltip != null)
            {
                OnTooltip(String.Empty);
            }
            
            Overlay.DOFade(0f, 0.3f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Click");
        }

        public void Setup(Tileset tileset, TilesetBrush brush)
        {
            slotName = brush.Name;
            Icon.texture = tileset.BaseTexture;
            Icon.uvRect = GetRect(tileset.BaseTexture, brush.Icon != 0 ? brush.Icon : brush.TileIds[15], tileset.TileSize);
            Overlay.DOFade(0f, 0.3f);
        }

        private Rect GetRect(Texture2D texture, int tile, int size)
        {
            tile -= 1;
            var cols = texture.width / size;
            var rows = texture.height / size;
            var row = tile / cols;
            var col = tile % cols;
            
            return new Rect(col / (float) cols, 1f - (row + 1) / (float) rows,
                            size / (float) texture.width, size / (float) texture.height);
        }
    }
}
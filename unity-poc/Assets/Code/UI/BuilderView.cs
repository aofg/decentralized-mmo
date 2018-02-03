using System;
using DG.Tweening;
using UFTM;
using UI.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public interface IBuilderView
    {
        void Setup(Tileset tileset);
    }
    
    public class BuilderView : AbstractView, IBuilderView
    {
        public BuilderSlotView SlotPrefab;
        public RectTransform SlotContainer;
        public RectTransform HoverRect;
        public Text HelpText;

        private Tileset tileset;
        
        public void Setup(Tileset tileset)
        {
            if (this.tileset == tileset)
            {
                return;
            }
            
            this.tileset = tileset;
            SlotContainer.RemoveAllChildren();
            
            foreach (var tilesetBrush in tileset.Brushes)
            {
                var brushView = Instantiate(SlotPrefab);
                brushView.OnTooltip += s =>
                {
                    HelpText.text = string.Empty;
                    if (!string.IsNullOrEmpty(s))
                    {
                        HelpText.DOText(s, s.Length * 0.05f, false);
                    }
                };  
                brushView.Setup(tileset, tilesetBrush);
                brushView.Rect.SetParent(SlotContainer);
                brushView.Rect.localScale = Vector3.one;
            }
        }
    }
}

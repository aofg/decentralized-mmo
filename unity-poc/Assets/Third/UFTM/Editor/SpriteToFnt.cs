using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Third.UFTM.Editor
{
    public class SpriteToFnt : EditorWindow
    {
        private Vector2 scroll;
        private bool toOrfrom;
        private string fntChars;

        public struct CharRecord
        {
//            char id=32 x=25 y=52 width=5 height=6 xoffest=0 yoffset=0 xadvance=7 page=0 chnl=0 letter=" "
            public byte X;
            public byte Y;
            public byte Width;
            public byte Height;
            public byte OffsetX;
            public byte OffsetY;
            public byte AdvanceX;
            public char Letter;

            public CharRecord(SpriteMetaData sprite, int height)
            {
                try
                {
                    Letter = (char) (byte) int.Parse(sprite.name);
                    X = (byte) Mathf.FloorToInt(sprite.rect.x);
                    Y = (byte) Mathf.FloorToInt(height - sprite.rect.y - sprite.rect.height);
                    Width = (byte) Mathf.FloorToInt(sprite.rect.width);
                    Height = (byte) Mathf.FloorToInt(sprite.rect.height);
                    OffsetX = 0;
                    OffsetY = 0;
                    AdvanceX = (byte) (Width + 1);
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Incorrect sprite:\n\nName: '{0}'\nRect: {1}", sprite.name, sprite.rect);
                    throw e;
                }
            }

            public CharRecord(string raw)
            {
                var parts = raw.Split(' ');
                Letter = (char)byte.Parse(parts[1].Split('=')[1]);
                X = byte.Parse(parts[2].Split('=')[1]);
                Y = byte.Parse(parts[3].Split('=')[1]);
                Width = byte.Parse(parts[4].Split('=')[1]);
                Height = byte.Parse(parts[5].Split('=')[1]);
                OffsetX = 0;
                OffsetY = 0;
                AdvanceX = 0;
            }

            public override string ToString()
            {
                return string.Format(
                    "char id={0} x={1} y={2} width={3} height={4} xoffest={5} yoffset={6} xadvance={7} page=0 chnl=0 letter=\"{8}\"",
                    (byte)Letter, X, Y, Width, Height, OffsetX, OffsetY, AdvanceX, Letter);
            }
        }
        [MenuItem("Assets/Convert To Fnt")]
        public static void OpenConverter()
        {
            var instance = CreateInstance<SpriteToFnt>();
            instance.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Mode toggle"))
            {
                toOrfrom = !toOrfrom;
            }
            
            scroll = GUILayout.BeginScrollView(scroll);
            if (!Selection.activeObject)
            {
                GUILayout.Label("Select sprite atlas");
                return;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var texture = Selection.activeObject as Texture2D;
            if (string.IsNullOrEmpty(path))
            {
                GUILayout.Label("Select sprite atlas from PROJECT view");
                return;
            }

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                GUILayout.Label("Select sprite atlas from PROJECT view");
                return;
            }
            
            if (toOrfrom)
            {
                OnFromGUI(importer, texture);
            }
            else
            {
                OnToGUI(importer, texture);
            }
            
            GUILayout.EndScrollView();
        }

        private void OnFromGUI(TextureImporter importer, Texture2D texture)
        {

            var sprites = importer.spritesheet;
            
            if (sprites.Length <= 1) 
            {
                GUILayout.Label("Select sprite atlas from PROJECT view");
                return;
            }
            
            var records = sprites.Select(sprite => new CharRecord(sprite, texture.height));

            GUILayout.TextArea(string.Join("\n", records.Select(r => r.ToString()).ToArray()), GUILayout.MaxHeight(300));
            if (GUILayout.Button("Copy"))
            {
                GUIUtility.systemCopyBuffer = string.Join("\n", records.Select(r => r.ToString()).ToArray());
            }
        }

        private void OnToGUI(TextureImporter importer, Texture2D texture)
        {
            fntChars = EditorGUILayout.TextArea(fntChars, GUILayout.MaxHeight(300));
            
            if (GUILayout.Button("Import"))
            {
                importer.filterMode = FilterMode.Point;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritesheet = fntChars.Split('\n').Select(line => new CharRecord(line)).Select(record =>
                    new SpriteMetaData
                    {
                        name = ((byte) record.Letter).ToString(),
                        rect = new Rect(record.X, texture.height - record.Y - record.Height, record.Width, record.Height)
                    }).ToArray();
            
                importer.SaveAndReimport();
            }
            
        }
    }
}
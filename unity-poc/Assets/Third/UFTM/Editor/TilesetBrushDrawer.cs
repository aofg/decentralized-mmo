using System;
using UFTM;
using UFTM.Datatypes;
using UnityEditor;
using UnityEngine;

namespace Third.UFTM.Editor
{
    public class SelectTileWizard : ScriptableWizard
    {
        private static Tileset cachedAtlas;
        private static Tileset SelectedAtlas
        {
            get
            {
                if (!cachedAtlas && !string.IsNullOrEmpty(EditorPrefs.GetString("UFTM_Atlas")))
                {
                    cachedAtlas = AssetDatabase.LoadAssetAtPath<Tileset>(EditorPrefs.GetString("UFTM_Atlas"));
                }

                return cachedAtlas;
            }
        }
        
        private int cachedIndex;
        private Action<int, int> callback;

        public static void CreateWizard(int index, Action<int, int> callback)
        {
            var instance = DisplayWizard<SelectTileWizard>("Select Tile", "Cancel");
            instance.cachedIndex = index;
            instance.callback = callback;
        }

        protected override bool DrawWizardGUI()
        {
            for (int row = 0; row < SelectedAtlas.Rows; row++)
            {
                GUILayout.BeginHorizontal();
                {
                    for (int col = 0; col < SelectedAtlas.Cols; col++)
                    {
                        if (GUILayout.Button("", GUIStyle.none, GUILayout.Width(35), GUILayout.Height(35)))
                        {
                            callback(cachedIndex, row * SelectedAtlas.Cols + col);
                            Close();
                            return false;
                        }
                        var last = GUILayoutUtility.GetLastRect();
                        last.x += 2;
                        last.y += 2;
                        last.width -= 4;
                        last.height -= 4;
                        GUI.DrawTextureWithTexCoords(last, SelectedAtlas.BaseTexture,
                            new Rect(col / (float) SelectedAtlas.Cols, 1f - (row + 1) / (float) SelectedAtlas.Rows,
                                32f / SelectedAtlas.BaseTexture.width, 32f / SelectedAtlas.BaseTexture.height));
                    }
                }
                GUILayout.EndHorizontal();
            }

            return true;
        }
    }
    
    [CustomPropertyDrawer(typeof(TilesetBrush))]
    public class TilesetBrushDrawer : PropertyDrawer
    {
        private static Tileset cachedAtlas;
        private static Texture2D cachedPattern;

        private static Tileset SelectedAtlas
        {
            get
            {
                if (!cachedAtlas && !string.IsNullOrEmpty(EditorPrefs.GetString("UFTM_Atlas")))
                {
                    cachedAtlas = AssetDatabase.LoadAssetAtPath<Tileset>(EditorPrefs.GetString("UFTM_Atlas"));
                }

                return cachedAtlas;
            }
        }

        private static Texture2D Pattern
        {
            get
            {
                if (!cachedPattern)
                {
                    cachedPattern = Resources.Load<Texture2D>("Pattern16");
                }

                return cachedPattern;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (SelectedAtlas == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            else if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            } else
            {
                return (EditorGUIUtility.singleLineHeight + 3) * 5 + 60 + 38 * 2;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (string.IsNullOrEmpty(EditorPrefs.GetString("UFTM_Atlas")) || SelectedAtlas == null)
            {
                SelectAtlasUI(position);
            }
            else if (!property.isExpanded)
            {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            }
            else
            {
                var line = new Rect(position);
                line.height = EditorGUIUtility.singleLineHeight;
                property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, label, EditorStyles.boldFont);
                position.y += EditorGUIUtility.singleLineHeight + 3;
                position.height -= EditorGUIUtility.singleLineHeight + 3;

                line = new Rect(position);
                line.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(line, property.FindPropertyRelative("Name"));
                position.y += EditorGUIUtility.singleLineHeight + 3;
                position.height -= EditorGUIUtility.singleLineHeight + 3;
                
                line = new Rect(position);
                line.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(line, property.FindPropertyRelative("Icon"));
                position.y += EditorGUIUtility.singleLineHeight + 3;
                position.height -= EditorGUIUtility.singleLineHeight + 3;
                
                line = new Rect(position);
                line.height = 57;
                property.FindPropertyRelative("Description").stringValue = EditorGUI.TextArea(line, property.FindPropertyRelative("Description").stringValue);
                position.y += 60;
                position.height -= 60;

                position.x += EditorGUI.indentLevel * 13;
                position.width -= EditorGUI.indentLevel * 13;
                
                line = new Rect(position);
                line.height = EditorGUIUtility.singleLineHeight;
                line.width -= 100;
                
                GUI.Label(line, "Selected atlas" + SelectedAtlas);
                line.x += line.width + 10;
                line.width = 90;
                
                if (GUI.Button(line, "Reselect"))
                {
                    EditorPrefs.SetString("UFTM_Atlas", null);
                    cachedAtlas = null;
                }

                position.y += EditorGUIUtility.singleLineHeight + 3;
                position.height -= EditorGUIUtility.singleLineHeight + 3;

                var size = 35;

                var tiles = property.FindPropertyRelative("TileIds");
                tiles.arraySize = 16;

                EditorGUI.BeginProperty(position, GUIContent.none, tiles);

//                var cached = EditorPrefs.GetInt("UFTM_CachedIndex", -1);
//                var received = EditorPrefs.GetInt("UFTM_ReceivedTile", -1);

                for (var row = 0; row < 2; row++)
                {
                    for (var col = 0; col < 8; col++)
                    {
                        var rect = new Rect(position);
                        rect.width = size;
                        rect.height = size;
                        rect.x += col * (size + 3);
                        rect.y += row * (size + 3);
                        var index = row * 8 + col;
                        var received = EditorPrefs.GetInt("UFTM_Tile_" + tiles.propertyPath + index, -1);

                        if (received != -1)
                        {
                            EditorPrefs.DeleteKey("UFTM_Tile_" + tiles.propertyPath + index);       
                            var prop = tiles.GetArrayElementAtIndex(index);
                            prop.intValue = received + 1;
                        }
                        
                        if (GUI.Button(rect, "", GUIStyle.none))
                        {       
                            var prop = tiles.GetArrayElementAtIndex(index);
                            prop.intValue = 0;
                            SelectTileWizard.CreateWizard(index, (tileIndex, tileId) =>
                            {
                                EditorPrefs.SetInt("UFTM_Tile_" + tiles.propertyPath + tileIndex, tileId);                                
                            });
                        }

                        var last = new Rect(rect);
                        last.x += 2;
                        last.y += 2;
                        last.width -= 4;
                        last.height -= 4;

                        var raw = property.FindPropertyRelative("TileIds").GetArrayElementAtIndex(index).intValue;
                        if (raw > 0)
                        {
                            var selectedIndex = raw - 1;
                            var selectedRow = selectedIndex / SelectedAtlas.Cols;
                            var selectedCol = selectedIndex % SelectedAtlas.Cols;
                            GUI.DrawTextureWithTexCoords(last, SelectedAtlas.BaseTexture,
                                new Rect(selectedCol / (float) SelectedAtlas.Cols,
                                    1f - (selectedRow + 1) / (float) SelectedAtlas.Rows,
                                    32f / SelectedAtlas.BaseTexture.width, 32f / SelectedAtlas.BaseTexture.height));
                        }
                        else
                        {
                            GUI.DrawTextureWithTexCoords(last, Pattern, new Rect(index / 16f, 1f, 1f / 16f, 1f));
                        }
                    }
                }

                position.y += 38 * 2;
                position.height = EditorGUIUtility.singleLineHeight;

                var l10 = new Rect(position);
                l10.width /= 4;
                
                var l1 = new Rect(position);
                l1.width = l10.width;
                l1.x = l10.x + l10.width;
                
                var r1 = new Rect(position);
                r1.width = l10.width;
                r1.x = l1.x + l1.width;
                
                var r10 = new Rect(position);
                r10.width = l10.width;
                r10.x = r1.x + r1.width;

                if (GUI.Button(l10, "<<<")) { Shift(tiles, -10); }
                if (GUI.Button(l1,  "<"  )) { Shift(tiles, -1); }
                if (GUI.Button(r1,  ">"  )) { Shift(tiles,  1); }
                if (GUI.Button(r10, ">>>")) { Shift(tiles,  10); }
                
                EditorGUI.EndProperty();
            }
            EditorGUI.EndProperty();
            EditorUtility.SetDirty( property.serializedObject.targetObject );
        }

        private void Shift(SerializedProperty tiles, int amount)
        {
            for (var index = 0; index < 16; index++)
            {
                var prop = tiles.GetArrayElementAtIndex(index);
                prop.intValue = prop.intValue + amount;
            }
        }

        private void SelectAtlasUI(Rect position)
        {
            var atlas = EditorGUI.ObjectField(position, "Select Atlas", null, typeof(Tileset), false);
            if (atlas != null)
            {
                cachedAtlas = null;
                EditorPrefs.SetString("UFTM_Atlas", AssetDatabase.GetAssetPath(atlas));
            }
        }
    }
}
using System;
using UnityEngine;

namespace UnityEditor {
    internal class GrassGUI : ShaderGUI {

        public enum _ShapeTypeValues {
            Pointed,
            Rectangular
        }

        public enum _LeafsValues {
            x1,
            x2
        }

        MaterialProperty _ShapeType = null;
        MaterialProperty _Size = null;
        MaterialProperty _LowerWidth = null;
        MaterialProperty _UpperWidth = null;
        MaterialProperty _LeafCuts = null;
        MaterialProperty _VisibleRange = null;
        MaterialProperty _LODDistance = null;
        MaterialProperty _DetailReductionFactor = null;
        MaterialProperty _LeafMultiplier = null;
        MaterialProperty _EnableBasePlane = null;
        MaterialProperty _EnableGradient = null;
        MaterialProperty _BaseColor = null;
        MaterialProperty _MiddleColor = null;
        MaterialProperty _TopColor = null;
        MaterialProperty _GradientCenter = null;
        MaterialProperty _EnableNoise = null;
        MaterialProperty _NoiseTex = null;
        MaterialProperty _NoiseStrength = null;
        MaterialProperty _EnableRandomLength = null;
        MaterialProperty _LengthIntensity = null;
        MaterialProperty _LengthMaxDiference = null;
        MaterialProperty _EnableWind = null;
        MaterialProperty _WindTex = null;
        MaterialProperty _WindStrength = null;
        MaterialProperty _WindSpeed = null;
        MaterialProperty _WindTexCol = null;
        MaterialProperty _EnableMask = null;
        MaterialProperty _AffectBase = null;
        MaterialProperty _MaskTex = null;
        MaterialProperty _EnablePressure = null;
        MaterialProperty _PressureTex = null;
        MaterialProperty _PressureStrength = null;

        MaterialEditor m_MaterialEditor;
        public void FindProperties (MaterialProperty[] props) {
            _ShapeType = FindProperty ("_GrassShapeType", props);
            _Size = FindProperty ("_Size", props);
            _LowerWidth = FindProperty ("_LowerWidth", props);
            _UpperWidth = FindProperty ("_UpperWidth", props);

            _LeafCuts = FindProperty ("_LeafCuts", props);
            _LeafMultiplier = FindProperty ("_LeafMultiplier", props);

            _VisibleRange = FindProperty ("_VisibleRange", props);
            _LODDistance = FindProperty ("_LODDistance", props);
            _DetailReductionFactor = FindProperty ("_DetailReductionFactor", props);

            _EnableBasePlane = FindProperty ("_EnableBasePlane", props);

            _EnableGradient = FindProperty ("_EnableGradient", props);
            _BaseColor = FindProperty ("_BaseColor", props);
            _MiddleColor = FindProperty ("_MiddleColor", props);
            _TopColor = FindProperty ("_TopColor", props);
            _GradientCenter = FindProperty ("_GradientCenter", props);

            _EnableNoise = FindProperty ("_EnableNoise", props);
            _NoiseTex = FindProperty ("_NoiseTex", props);
            _NoiseStrength = FindProperty ("_NoiseStrength", props);
            _EnableRandomLength = FindProperty ("_EnableRandomLength", props);
            _LengthIntensity = FindProperty ("_LengthIntensity", props);
            _LengthMaxDiference = FindProperty ("_LengthMaxDiference", props);

            _EnableWind = FindProperty ("_EnableWind", props);
            _WindTex = FindProperty ("_WindTex", props);
            _WindStrength = FindProperty ("_WindStrength", props);
            _WindSpeed = FindProperty ("_WindSpeed", props);
            _WindTexCol = FindProperty ("_WindTexCol", props);

            _EnableMask = FindProperty ("_EnableMask", props);
            _AffectBase = FindProperty ("_AffectBase", props);
            _MaskTex = FindProperty ("_MaskTex", props);

            _EnablePressure = FindProperty ("_EnablePressure", props);
            _PressureTex = FindProperty ("_PressureTex", props);
            _PressureStrength = FindProperty ("_PressureStrength", props);
        }

        public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties) {
            FindProperties (properties);
            m_MaterialEditor = materialEditor;
            Material targetMat = materialEditor.target as Material;

            ShaderPropertiesGUI (targetMat, properties);
        }

        public void ShaderPropertiesGUI (Material material, MaterialProperty[] properties) {
            EditorGUIUtility.labelWidth = 0f;

            GUILayout.Label ("Leaf form and size", EditorStyles.boldLabel);

            ShowShapeTypePopUp ();

            m_MaterialEditor.ShaderProperty (_Size, "Leaf Length");

            m_MaterialEditor.RangeProperty (_LowerWidth, "Base width");
            m_MaterialEditor.RangeProperty (_UpperWidth, "Upper width");

            EditorGUILayout.Space ();

            GUILayout.Label ("Leafs detail", EditorStyles.boldLabel);

            m_MaterialEditor.RangeProperty (_LeafCuts, "Leaf cuts (Leaf Quality)");
            m_MaterialEditor.RangeProperty (_VisibleRange, "Visible range");
            m_MaterialEditor.RangeProperty (_LODDistance, "LoD Distance");
            m_MaterialEditor.RangeProperty (_DetailReductionFactor, "Detail reduction factor");

            ShowLeafsPerTrianglePopUp ();

            EditorGUILayout.Space ();

            m_MaterialEditor.ShaderProperty (_EnableBasePlane, "Enable mesh base");

            EditorGUILayout.Space ();

            GUILayout.Label ("Color", EditorStyles.boldLabel);

            m_MaterialEditor.ShaderProperty (_EnableGradient, "Enable Gradient");
            m_MaterialEditor.ColorProperty (_BaseColor, "Base color");

            if (_EnableGradient.floatValue == 1) {
                m_MaterialEditor.ColorProperty (_MiddleColor, "Middle color");
                m_MaterialEditor.ColorProperty (_TopColor, "Top color");

                m_MaterialEditor.RangeProperty (_GradientCenter, "Gradient center");
            }

            EditorGUILayout.Space ();

            GUILayout.Label ("Noise", EditorStyles.boldLabel);

            m_MaterialEditor.ShaderProperty (_EnableNoise, "Enable Noise");

            if (_EnableNoise.floatValue == 1) {
                m_MaterialEditor.ShaderProperty (_NoiseTex, "Noise texture");

                m_MaterialEditor.ShaderProperty (_NoiseStrength, "Noise Strength");
                m_MaterialEditor.ShaderProperty (_EnableRandomLength, "Enable Random Length");

                if (_EnableRandomLength.floatValue == 1) {
                    m_MaterialEditor.ShaderProperty (_LengthIntensity, "Length Intensity Strength");
                    m_MaterialEditor.ShaderProperty (_LengthMaxDiference, "Max length difference");
                }
            }

            EditorGUILayout.Space ();

            GUILayout.Label ("Wind", EditorStyles.boldLabel);

            m_MaterialEditor.ShaderProperty (_EnableWind, "Enable wind");

            if (_EnableWind.floatValue == 1.0) {
                m_MaterialEditor.TextureProperty (_WindTex, "Wind Texture", true);
                m_MaterialEditor.ShaderProperty (_WindTexCol, "Colors to look for");
                m_MaterialEditor.ShaderProperty (_WindStrength, "Strength");
                m_MaterialEditor.ShaderProperty (_WindSpeed, "Speed");
            }

            EditorGUILayout.Space ();

            GUILayout.Label ("Mesh Grass Mask", EditorStyles.boldLabel);

            m_MaterialEditor.ShaderProperty (_EnableMask, "Enable mask");

            if (_EnableMask.floatValue == 1.0) {
                m_MaterialEditor.ShaderProperty (_AffectBase, "Affect Mesh Base");
                m_MaterialEditor.TextureProperty (_MaskTex, "Mask Texture", true);
            }

            GUILayout.Label ("Smash", EditorStyles.boldLabel);

            m_MaterialEditor.ShaderProperty (_EnablePressure, "Enable smash");

            if (_EnablePressure.floatValue == 1.0) {
                m_MaterialEditor.TextureProperty (_PressureTex, "Smash texture", true);
                m_MaterialEditor.RangeProperty (_PressureStrength, "Smash Strength");
            }
        }

        private void ShowShapeTypePopUp () {
            EditorGUI.showMixedValue = _ShapeType.hasMixedValue;
            var shapeType = (_ShapeTypeValues) _ShapeType.floatValue;

            EditorGUI.BeginChangeCheck ();
            String[] names = Enum.GetNames (typeof (_ShapeTypeValues));

            shapeType = (_ShapeTypeValues) EditorGUILayout.Popup ("Leaf shape", (int) shapeType, names);

            if (EditorGUI.EndChangeCheck ()) {
                m_MaterialEditor.RegisterPropertyChangeUndo ("Leaf shape");
                _ShapeType.floatValue = (float) shapeType;
            }

            EditorGUI.showMixedValue = false;
        }

        private void ShowLeafsPerTrianglePopUp () {
            EditorGUI.showMixedValue = _LeafMultiplier.hasMixedValue;
            var leafMult = (_LeafsValues) _LeafMultiplier.floatValue;

            EditorGUI.BeginChangeCheck ();
            String[] names = Enum.GetNames (typeof (_LeafsValues));

            leafMult = (_LeafsValues) EditorGUILayout.Popup ("Leafs per triangle", (int) leafMult, names);

            if (EditorGUI.EndChangeCheck ()) {
                m_MaterialEditor.RegisterPropertyChangeUndo ("Leafs per triangle");
                _LeafMultiplier.floatValue = (float) leafMult;
            }

            EditorGUI.showMixedValue = false;
        }

        public override void AssignNewShaderToMaterial (Material material, Shader oldShader, Shader newShader) {
            base.AssignNewShaderToMaterial (material, oldShader, newShader);
        }
    }
}
using System;
using TrigBaker.BakingLib;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TrigBaker.Editor
{
    public class Mathimation : EditorWindow
    {
        [MenuItem("Window/Mathimation")]
        static void ShowWindow()
        {
            GetWindow(typeof(Mathimation));
        }
        public enum AnimatableProperty
        {
            RotationX,
            RotationY,
            RotationZ,
            PositionX,
            PositionY,
            PositionZ
        }

        private Animation animation;
        private float animationLength;
        private AnimatableProperty targetProp;
        private bool repeat;
        private float scale;
        private float offset;
        private string formula;
    
        void OnGUI()
        {
            GUILayout.BeginVertical();
            animation = (Animation) EditorGUILayout.ObjectField(animation, typeof(Animation), true);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Length");  
            animationLength = EditorGUILayout.FloatField(animationLength);
            repeat = EditorGUILayout.Toggle("Repeat", repeat);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            targetProp = (AnimatableProperty) EditorGUILayout.EnumPopup(targetProp, GUILayout.MaxWidth(position.width / 3f));
            GUILayout.Label("=", GUILayout.ExpandWidth(false));
            formula = GUILayout.TextArea(formula);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Remove all curves"))
            {
                animation.clip.ClearCurves();
            }
            if (GUILayout.Button("Make curves"))
            {
                Debug.Log(animation.name);
                var simObj = new GameObject("sim");
                var clip = animation;
                if (!animation.clip.legacy)
                    animation.clip.legacy = true;
                animation.clip = (new Baker(animation.transform))
                    .BakeAnimation(formula, animation.clip ?? new AnimationClip(), new BakerParams
                    {
                        AnimationDuration = animationLength,
                        TemporalResolution = animation.clip.frameRate / 8
                    }, typeof(Transform), targetProp switch
                    {
                        AnimatableProperty.RotationX => "localRotation.x",
                        AnimatableProperty.RotationY => "localRotation.y",
                        AnimatableProperty.RotationZ => "localRotation.z",
                        AnimatableProperty.PositionX => "localPosition.x",
                        AnimatableProperty.PositionY => "localPosition.y",
                        AnimatableProperty.PositionZ => "localPosition.z",
                        _ => throw new ArgumentOutOfRangeException()
                    });
                DestroyImmediate(simObj);
            }

            //animation = Baker.BakeAnimation(formula, animation, new() { AnimationDuration = animationLength });
            GUILayout.EndVertical();
        }
    }
}


#endif
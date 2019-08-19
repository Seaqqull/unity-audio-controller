﻿using UnityEngine;

namespace Audio.Visualizer
{
    /// <summary>
    /// Visualize audibility or loudness of audio.
    /// </summary>
    public class AudioVisualizer : MonoBehaviour
    {
        [SerializeField] private Audio.AudioController _audios;

        /// <summary>
        /// Is transition visualized or drawn as two siple circle.
        /// </summary>
        [SerializeField] private bool _vizualize = true;
        /// <summary>
        /// Is drawn detection or 3D.
        /// </summary>
        [SerializeField] private bool _drawDetection = true;
        /// <summary>
        /// Is drawn only when selected.
        /// </summary>
        [SerializeField] private bool _drawOnSelected = true;

        /// <summary>
        /// Index of drawn audio.
        /// </summary>
        [SerializeField] private int _drawZone = 0;
        /// <summary>
        /// Count of steps, on visualizations.
        /// </summary>
        [SerializeField] [Range(0.1f, sbyte.MaxValue)] private int _step = 10;
        [SerializeField] [Range(0.0f, 1.0f)] private float _alpha = 0.05f;

        [SerializeField] private Color _colorZone = Color.black;
        [SerializeField] private Color _colorNoiseBad = Color.red;
        [SerializeField] private Color _colorNoiseGood = Color.green;


        private void OnDrawGizmos()
        {
            if ((!_audios) ||
                (_drawOnSelected) ||
                (_audios.Audios.Count == 0) ||
                ((_drawZone < 0) || (_drawZone >= _audios.Audios.Count))) return;

            if (_drawDetection)
            {
                DrawZone(
                    _audios.Audios[_drawZone].InnerRadiusDetection, _audios.Audios[_drawZone].OutherRadiusDetection,
                    _audios.Audios[_drawZone].LoudnessSpread);
            }
            else
            {
                DrawZone(
                    _audios.Audios[_drawZone].InnerRadius3D, _audios.Audios[_drawZone].OutherRadius3D,
                    _audios.Audios[_drawZone].VolumeSpread);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if ((!_audios) ||
                (!_drawOnSelected) ||
                (_audios.Audios.Count == 0) ||
                ((_drawZone < 0) || (_drawZone >= _audios.Audios.Count))) return;

            if (_drawDetection)
            {
                DrawZone(
                    _audios.Audios[_drawZone].InnerRadiusDetection, _audios.Audios[_drawZone].OutherRadiusDetection,
                    _audios.Audios[_drawZone].LoudnessSpread);
            }
            else
            {
                DrawZone(
                    _audios.Audios[_drawZone].InnerRadius3D, _audios.Audios[_drawZone].OutherRadius3D, 
                    _audios.Audios[_drawZone].VolumeSpread);
            }            
        }


        /// <summary>
        /// Draws circle, that represents audibility or loudness of audio.
        /// </summary>
        /// <param name="innerRadius">Minimal raius.</param>
        /// <param name="outherRadius">Maximal radius.</param>
        private void DrawZone(float innerRadius, float outherRadius, AnimationCurve spread)
        {
#if UNITY_EDITOR
            if (innerRadius >= outherRadius) return;
            if (_vizualize)
            {
                float progress;
                float stepLength = (outherRadius - innerRadius) / _step;

                for (float i = outherRadius; i >= innerRadius; i -= stepLength)
                {
                    progress = Utility.Data.FloatHelper.Map(i, innerRadius, outherRadius, 0, 1);
                    progress = spread.Evaluate(progress);

                    UnityEditor.Handles.color = new Color(
                        Mathf.Lerp(_colorNoiseBad.r, _colorNoiseGood.r, progress),
                        Mathf.Lerp(_colorNoiseBad.g, _colorNoiseGood.g, progress),
                        Mathf.Lerp(_colorNoiseBad.b, _colorNoiseGood.b, progress), _alpha);

                    UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, -transform.right, 360, i);
                }
            }
            else
            {
                UnityEditor.Handles.color = _colorZone;

                UnityEditor.Handles.DrawWireArc(transform.position,
                    Vector3.up, Vector3.forward, 360, innerRadius);
                UnityEditor.Handles.DrawWireArc(transform.position,
                    Vector3.up, Vector3.forward, 360, outherRadius);
            }
#endif
        }

    }
}

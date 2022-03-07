using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.StateMachine
{
    public class DamageReceivingState : State
    {

        [SerializeField]
        private List<Renderer> _renderers;

        private Dictionary<Renderer, Color> _renderersColors = new Dictionary<Renderer, Color>();
        private Dictionary<Renderer, (float, float, float)> _renderersDeltaChanging = new Dictionary<Renderer, (float, float, float)>();

        public float MaximumRedness = 1;

        void Awake()
        {
            foreach (var renderer in _renderers)
            {
                _renderersColors.Add(renderer, renderer.material.GetColor("_BaseColor"));
            }

            foreach (var renderer in _renderers)
            {
                var deltaR = 1 - renderer.material.GetColor("_BaseColor").r;
                var deltaG = 0 - renderer.material.GetColor("_BaseColor").g;
                var deltaB = 0 - renderer.material.GetColor("_BaseColor").b;
                _renderersDeltaChanging.Add(renderer, (deltaR, deltaG, deltaB));
            }
        }

        public override void OnEnter()
        {
            //CameraShaker.Instance.Shake(0.05f,1);

            StopCoroutine(Shaking());
            StartCoroutine(Shaking());

            foreach (var renderer in _renderers)
            {
                var color = new Color(
                    MaximumRedness,
                    0,
                    0,
                    0
                    );

                renderer.material.SetColor("_BaseColor", color);
            }
        }

        public override void OnExit()
        {
            foreach (var renderer in _renderersColors)
            {
                renderer.Key.material.color = renderer.Value;
            }
        }

        public override void OnUpdate()
        {
            foreach (var renderer in _renderers)
            {
                var delta = _renderersDeltaChanging[renderer];

                var color = new Color(
                    renderer.material.GetColor("_BaseColor").r - delta.Item1 * Time.deltaTime,
                    renderer.material.GetColor("_BaseColor").g - delta.Item2 * Time.deltaTime,
                    renderer.material.GetColor("_BaseColor").b - delta.Item3 * Time.deltaTime,
                    renderer.material.GetColor("_BaseColor").a
                    );

                renderer.material.SetColor("_BaseColor", color);
            }
        }

        private IEnumerator Shaking()
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.material.SetFloat("_Frequency", i);
                }
                yield return new WaitForSecondsRealtime(0.01f);
            }
            for (int i = 4; i >= 0; i--)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.material.SetFloat("_Frequency", i);
                }
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }
}
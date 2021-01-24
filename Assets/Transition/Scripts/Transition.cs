using System;
using DG.Tweening;
using TransitionFade.Interface;
using UniRx;
using UnityEngine;

namespace TransitionFade {
        [Serializable]
        public class Transition : MonoBehaviour,IUtilTransition{
            [SerializeField] private Material transitionMaterial;
            public delegate void Callback();
            private Callback callbackComplete;
            private static readonly int Property = Shader.PropertyToID("_CutOff");
            private float TransitionRate { get; set; }
            private bool IsFade { get; set; }

            private void Start() {
               if (transitionMaterial == null) {
                   Debug.LogError("Transition.cs:マテリアルが設定されてない");
               }
               this.ObserveEveryValueChanged(x => TransitionRate).Subscribe(_ => {
                       transitionMaterial.SetFloat(Property, TransitionRate);
                   }
               ).AddTo(this);

            }
            
            private void OnApplicationQuit(){
                transitionMaterial.SetFloat(Property, 1.0f);
            }

            public bool IsActiveFade() {
               return IsFade;
            }

            /// <summary>
            /// カットアウトさせる
            /// </summary>
            /// <param name="startVal"></param>
            /// <param name="endVal"></param>
            /// <param name="duration"></param>
            public IUtilTransition Fade(float startVal,float endVal,float duration) {
                IsFade = true;
                TransitionRate = startVal;
                DOTween
                    .To(() => TransitionRate, (x) => TransitionRate = x, endVal, duration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() => {
                        IsFade = false;
                        callbackComplete?.Invoke();
                    });
                return this;
            }

            /// <summary>
            /// 完了
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public IUtilTransition Complete(Callback action) {
                this.callbackComplete = action;
                return this;
            }

            /// <summary>
            /// フェードアウト
            /// </summary>
            public IUtilTransition FadeOut() {
                return Fade( 0, 1,0.2f);
            }


            /// <summary>
            /// フェードイン
            /// </summary>
            public IUtilTransition FadeIn() {
                return Fade(1, 0,0.2f);
            }
        }
}

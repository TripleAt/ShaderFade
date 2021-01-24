using System.Collections;
using System.Collections.Generic;
using TransitionFade.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PressButton : MonoBehaviour {
    [SerializeField] 
    private TransitionFade.Transition transition;
    private IUtilTransition fadeObj;   //Unityは早くインターフェースでSerializeField使えるようにして欲しい……
    private bool isDoneFade;

    private void Start() {
        Button btn = GetComponent<Button>();
        fadeObj = transition;
        
        //ボタンが押されなおかつ、フェード中ではない?
        btn.OnClickAsObservable().Where(_ =>!fadeObj.IsActiveFade()).Subscribe(_ => {
            if (isDoneFade) {   
                //フェードが完了してたら、フェードインする
                fadeObj.FadeIn().Complete(() => {
                    isDoneFade = false;
                    Debug.Log("フェードイン終了");
                });
                return;
            }

            //フェードが行われてなかったら、フェードアウトする
            fadeObj.FadeOut().Complete(() => {
                Debug.Log("フェードアウト終了");
                isDoneFade = true;
            });
        });
    }

}




namespace TransitionFade.Interface {
    public interface IUtilTransition {
        bool IsActiveFade();
        IUtilTransition Fade(float startVal, float endVal, float duration);
        IUtilTransition Complete(Transition.Callback action);
        IUtilTransition FadeOut();
        IUtilTransition FadeIn();
    }
}

using UnityEngine;

public class AnimatorController {

    [SerializeField]
    private Animator _animator;

    private string _lastAnim;
    private string _currentAnim;

    private float _animDuration;
    private bool _isLooping;

    public AnimatorController(Animator animator) {
        _animator = animator;
        _currentAnim = "";
        _lastAnim = "";
        _isLooping = false;
        _animDuration = 0.0f;
    }

    public void ChangeAnimation(string name) {
        if (_currentAnim.Contains(name)) return;

        _lastAnim = _currentAnim;
        _currentAnim = name;

        _animator.Play(_currentAnim);
    }

    public bool AnimationFinished() {
        return (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    }

    public void Update() {
        AnimatorStateInfo anim = _animator.GetCurrentAnimatorStateInfo(0);
        _isLooping = anim.loop;
        _animDuration = anim.length;
    }

}

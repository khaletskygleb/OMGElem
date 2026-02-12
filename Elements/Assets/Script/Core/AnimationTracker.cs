using System;

public class AnimationTracker
{
    private int _counter;
    private Action _onComplete;

    public void Setup(Action onComplete) => _onComplete = onComplete;

    public void Register() => _counter++;

    public void Complete()
    {
        _counter--;
        if (_counter <= 0)
            _onComplete?.Invoke();
    }
}

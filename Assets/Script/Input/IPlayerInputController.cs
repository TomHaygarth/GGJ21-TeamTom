using UnityEngine;

public interface IPlayerInputController
{
    Vector2 MovementAxis { get; }
    bool DigPressed { get; }
    bool DigReleased { get; }

    float DirectionDeadzone { get; }
}

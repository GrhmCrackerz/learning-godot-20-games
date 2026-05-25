using Godot;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 300f;

    [Export]
    public float SpeedIncreasePerBounce { get; set; } = 15f;

    [Export]
    public float MaxSpeed { get; set; } = 800f;

    [Signal]
    public delegate void GoalScoredEventHandler(string side);

    private Vector2 _screenSize;
    private bool _ballInPlay = false;
    private float _currentSpeed;
    private Vector2 _direction;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        Position = _screenSize / 2;
        Launch();
    }

    private void Launch()
    {
        _currentSpeed = Speed;
        float verticalDirection = GD.Randf() > 0.5f ? 1f : -1f;
        float horizontalDirection = GD.Randf() > 0.5f ? 1f : -1f;

        _direction = new Vector2(horizontalDirection, verticalDirection).Normalized();
        _ballInPlay = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_ballInPlay) return;

        // Set velocity from direction and current speed
        Velocity = _direction * _currentSpeed;

        // MoveAndCollide moves the ball and returns collision info if hit occurred
        // Unlike MoveAndSlide it gives us direct control over what happens on hit
        var collision = MoveAndCollide(Velocity * (float)delta);

        if (collision != null)
        {
            // Reflect the direction off the surface that was hit
            // collision.GetNormal() is the surface's outward facing direction
            _direction = _direction.Bounce(collision.GetNormal());

            // Increase speed on each bounce
            _currentSpeed = Mathf.Min(_currentSpeed + SpeedIncreasePerBounce, MaxSpeed);
        }

        // Check for goals
        if (Position.X < 0)
        {
            EmitSignal(SignalName.GoalScored, "right");
            Reset();
        }
        else if (Position.X > _screenSize.X)
        {
            EmitSignal(SignalName.GoalScored, "left");
            Reset();
        }
    }

    private void Reset()
    {
        _ballInPlay = false;
        Velocity = Vector2.Zero;
        Position = _screenSize / 2;

        var timer = GetTree().CreateTimer(1.0);
        timer.Timeout += Launch;
    }
}
using Godot;
using System;

public partial class Movement : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 300;

	[Export]
	public string ActionUp { get; set; } = "paddle_left_up";

	[Export]
	public string ActionDown { get; set; } = "paddle_left_down";

	private float _halfHeight;

    public override void _Ready()
    {
		var shape = GetNode<CollisionShape2D>("CollisionShape2D");
		var rect = (RectangleShape2D)shape.Shape;
		_halfHeight = rect.Size.Y / 2;
    }

	public void GetInput()
	{
		float direction = Input.GetAxis(ActionUp, ActionDown);
		Velocity = new Vector2(0, direction * Speed);
	}

	private void ClampToScreen()
	{
		Rect2 screen = GetViewportRect();
		float clampedY = Mathf.Clamp(
			Position.Y,
			_halfHeight,
			screen.Size.Y - _halfHeight
		);
		Position = new Vector2(Position.X, clampedY);
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
		ClampToScreen();
	}
}

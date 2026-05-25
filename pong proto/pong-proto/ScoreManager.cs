using Godot;
using System;

public partial class ScoreManager : Node2D
{
	[Export]
	public Label LeftScoreLabel { get; set; }

	[Export]
	public Label RightScoreLabel { get; set; }

	[Export]
	public Ball Ball { get; set; }

	[Export]
	public Panel GameOverPanel { get; set; }

	[Export]
	public Label WinnerLabel { get; set; }

	[Export]
	public Button RestartButton { get; set; }

	private int _leftScore = 0;
	private int _rightScore = 0;
	private const int WinningScore = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Ball.GoalScored += OnGoalScored;
		RestartButton.Pressed += OnRestartPressed;
		UpdateLabels();
	}

	private void OnGoalScored(string side)
	{
		if (side == "left")
			_leftScore++;
		else
			_rightScore++;

		UpdateLabels();
		CheckForWinner();
	}

	private void UpdateLabels()
	{
		LeftScoreLabel.Text = _leftScore.ToString();
		RightScoreLabel.Text = _rightScore.ToString();
	}

	private void CheckForWinner()
	{
		if (_leftScore >= WinningScore)
			EndGame("Left Player Wins!");
		else if (_rightScore >= WinningScore)
			EndGame("Right Player Wins!");
	}

	private void EndGame(string message)
	{
		WinnerLabel.Text = message;
		GameOverPanel.Visible = true;
		GetTree().Paused = true;
		RestartButton.ProcessMode = ProcessModeEnum.Always;
	}

	private void OnRestartPressed()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}
}

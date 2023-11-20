using Godot;
using System;

public partial class AngryPig : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private bool _chasePlayer = false;
	public const float Speed = 50.0f;

	private Node2D _player;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
		_animatedSprite.Play("Idle");
		//_animatedSprite.AnimationChanged += AnimatedSprite_AnimationChanged;
	}

	//private void AnimatedSprite_AnimationChanged()
	//{
	//	if (_animatedSprite is null)
	//	{
	//		return;
	//	}

	//	if (_animatedSprite.Animation == "Death")
	//	{
	//		_animatedSprite.AnimationFinished += () =>
	//		{
	//			this.QueueFree();
	//		};
	//	}        
	//}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		_player = GetNode<Node2D>($"../../NinjaFrog/NinjaFrog");

		if (!_chasePlayer)
		{
			velocity.X = 0;
		}

		if (_chasePlayer && _player is not null)
		{
			var direction = (_player.Position - this.Position).Normalized();

			_animatedSprite.FlipH = direction.X > 0;
			velocity.X = direction.X * Speed;
		}


		Velocity = velocity;
		MoveAndSlide();
	}
	private void OnPlayerDetectionBodyEntered(Node2D body)
	{
		if (IsBodyNinjaFrog(body) && _animatedSprite.Animation != "Death")

		{
			GD.Print($"{nameof(OnPlayerDetectionBodyEntered)} - Current animation - {_animatedSprite.Animation}");
			_animatedSprite.Play("Run");
			_chasePlayer = true;
		}
	}

	private void OnPlayerDetectionBodyExited(Node2D body)
	{
		if (IsBodyNinjaFrog(body) && _animatedSprite.Animation != "Death")
		{
			GD.Print($"{nameof(OnPlayerDetectionBodyExited)} - Current animation - {_animatedSprite.Animation}");
			_animatedSprite.Play("Idle");
			_chasePlayer = false;
		}
	}
	private void OnPlayerStomp(Node2D body)
{
	if (IsBodyNinjaFrog(body))
		{
			GD.Print($"{nameof(OnPlayerStomp)} - Current animation - {_animatedSprite.Animation}");
			_animatedSprite.Play("Death");
			_chasePlayer = false;
			_animatedSprite.AnimationFinished += () =>
			{
				this.QueueFree();
			};
		}
}


	private bool IsBodyNinjaFrog(Node2D body) =>
		body is not null
		&& body.Name.Equals(NinjaFrogConstants.Name);
}



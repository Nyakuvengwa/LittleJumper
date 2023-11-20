using Godot;

public partial class NinjaFrog : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimationPlayer _animatedPlayer;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		base._Ready();
		_animatedPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
		_animatedSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
		_animatedPlayer.Play(NinjaFrogAnimation.Idle);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			_animatedPlayer.Play(NinjaFrogAnimation.Jump);
		}
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction == Vector2.Left)
		{
			_animatedSprite.FlipH = true;

		}
		else if (direction == Vector2.Right)
		{
			_animatedSprite.FlipH = false;
		}

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (velocity.Y == 0)
			{
				_animatedPlayer.Play(NinjaFrogAnimation.Run);
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			if (velocity.Y == 0)
				_animatedPlayer.Play(NinjaFrogAnimation.Idle);
		}

		if (velocity.Y > 0)
			_animatedPlayer.Play(NinjaFrogAnimation.Fall);

		Velocity = velocity;
		MoveAndSlide();
	}
}

public static class NinjaFrogConstants
{
	public const string Name = "NinjaFrog";
}

public static class NinjaFrogAnimation
{
	public const string Run = "ninja_frog_run_animation";
	public const string Idle = "ninja_frog_idle_animation";
	public const string Fall = "ninja_frog_fall_animation";
	public const string Jump = "ninja_frog_jump_animation";
}

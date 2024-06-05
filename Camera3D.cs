using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
	[Export] private Node3D box;
	private float radius = 0f;
	private float angle = 0f;
	private float sideRotationSpeed = 3f;
	private float height = 10f;
	private float verticalRotationSpeed = 10f;
	private float distanceMax = 9.9f;
	private float distanceMin = 0.1f;

	public override void _Ready()
	{
		RotateVertical(0f);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("RotateLeft")) {
			RotateSideWay((float) -delta);
		}
		if (Input.IsActionPressed("RotateRight")) {
			RotateSideWay((float) delta);
		}
		if (Input.IsActionPressed("RotateUp")) {
			RotateVertical((float) delta);
		}
		if (Input.IsActionPressed("RotateDown")) {
			RotateVertical((float) -delta);
		}
	}

	public void RotateSideWay(float delta) {
		angle += sideRotationSpeed * delta;
		if (angle > 2 * Mathf.Pi)
		{
			angle -= 2 * Mathf.Pi;
		}

		UpdateCameraPosition();
	} 

	public void RotateVertical(float delta) {
		height += verticalRotationSpeed * delta;
		radius -= verticalRotationSpeed * delta;
	
		if (height < distanceMin) {
			height = distanceMin;
		}
		if (height > distanceMax) {
			height = distanceMax;
		}

		if (radius > distanceMax) {
			radius = distanceMax;
		}
		if (radius < distanceMin) {
			radius = distanceMin;
		}

		UpdateCameraPosition();
	}

	private void UpdateCameraPosition() {
		Vector3 cameraPosition = new Vector3(
			box.Position.X + radius * Mathf.Cos(angle),
			height,
			box.Position.Z + radius * Mathf.Sin(angle)
		);

		Position = cameraPosition;

		// Check if the up vector and direction are aligned
		Vector3 direction = box.Position - cameraPosition;
		Vector3 up = Vector3.Up;

		// If aligned, slightly offset the up vector
		if (up.Cross(direction).LengthSquared() < 0.0001f) {
			up = new Vector3(0.0f, 1.0f, 0.1f).Normalized();
		}

		LookAt(box.Position, up);
	}
}

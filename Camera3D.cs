using Godot;
using System;

public enum CameraControlButton {
	RotateUp = 0,
	RotateLeft = 1,
	RotateDown = 2,
	RotateRight = 3,
}

public partial class Camera3D : Godot.Camera3D
{
	public event EventHandler<(CameraControlButton, bool)> ButtonPressed;

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
		if (Input.IsActionPressed(CameraControlButton.RotateLeft.ToString())) {
			RotateSideWay((float) -delta);
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateLeft, true));
		} else if (Input.IsActionJustReleased(CameraControlButton.RotateLeft.ToString())) {
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateLeft, false));
		}

		if (Input.IsActionPressed(CameraControlButton.RotateRight.ToString())) {
			RotateSideWay((float) delta);
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateRight, true));
		} else {
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateRight, false));
		}

		if (Input.IsActionPressed(CameraControlButton.RotateUp.ToString())) {
			RotateVertical((float) delta);
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateUp, true));
		} else {
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateUp, false));
		} 

		if (Input.IsActionPressed(CameraControlButton.RotateDown.ToString())) {
			RotateVertical((float) -delta);
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateDown, true));
		} else {
			ButtonPressed?.Invoke(this, (CameraControlButton.RotateDown, false));
		}
	}

	private void RotateSideWay(float delta) {
		angle += sideRotationSpeed * delta;
		if (angle > 2 * Mathf.Pi)
		{
			angle -= 2 * Mathf.Pi;
		}

		UpdateCameraPosition();
	} 

	private void RotateVertical(float delta) {
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

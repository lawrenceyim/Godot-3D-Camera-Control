using Godot;

public partial class CameraControlUi : Control
{
	[Export] private Camera3D camera3D;
	[Export] private TextureRect textureW;
	[Export] private TextureRect textureA;
	[Export] private TextureRect textureS;
	[Export] private TextureRect textureD;
	private Color pressedColor = new Color(0, 0, .8f, 1);
	private Color defaultColor = new Color(0, 0, 0, 1);

	public override void _Ready()
	{
		camera3D.ButtonPressed += ChangeButtonUi;
	}

	public void ChangeButtonUi(object sender, (CameraControlButton, bool) buttonPressed)
	{
		switch (buttonPressed.Item1)
		{
			case CameraControlButton.RotateUp:
				ChangeButtonUi(textureW, buttonPressed.Item2);
				return;
			case CameraControlButton.RotateLeft:
				ChangeButtonUi(textureA, buttonPressed.Item2);
				return;
			case CameraControlButton.RotateDown:
				ChangeButtonUi(textureS, buttonPressed.Item2);
				return;
			case CameraControlButton.RotateRight:
				ChangeButtonUi(textureD, buttonPressed.Item2);
				return;
			default:
				return;
		}
	}

	private void ChangeButtonUi(TextureRect textureRect, bool pressed)
	{
		if (pressed)
		{
			textureRect.SelfModulate = pressedColor;
		}
		else
		{
			textureRect.SelfModulate = defaultColor;
		}
	}
}

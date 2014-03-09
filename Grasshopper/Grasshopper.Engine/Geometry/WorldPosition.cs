using SharpDX;

namespace Grasshopper.Engine.Geometry
{
	public class WorldPosition
	{
		public Vector3 Location { get; set; }
		public float Scale { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public float Roll { get; set; }
		
		public Matrix WorldMatrix { get; private set; }

		public WorldPosition()
			: this(Vector3.Zero, 1f, 0, 0, 0)
		{
		}

		public WorldPosition(Vector3 location, float scale, float yaw, float pitch, float roll)
		{
			Location = location;
			Scale = scale;
			Yaw = yaw;
			Pitch = pitch;
			Roll = roll;
			Update();
		}

		public void Update()
		{
			var orientation = Quaternion.RotationYawPitchRoll(Yaw, Pitch, Roll);
			WorldMatrix = Matrix.AffineTransformation(Scale, orientation, Location);
		}
	}
}

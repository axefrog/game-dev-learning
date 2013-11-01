using SharpDX;

namespace SharpDx4.Game.Geometry
{
	public class ModelInstance
	{
		public Vector3 Position { get; set; }
		public float Scale { get; set; }
		public Quaternion Orientation { get; set; }
		
		public Matrix WorldMatrix { get; private set; }

		public ModelInstance()
		{
			Position = Vector3.Zero;
			Scale = 1f;
			Orientation = Quaternion.Zero;
		}

		public void Update()
		{
			WorldMatrix = Matrix.AffineTransformation(Scale, Orientation, Position);
		}
	}
}
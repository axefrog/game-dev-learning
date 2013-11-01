using SharpDX;

namespace SharpDx4.Game.Geometry
{
	public class Camera
	{
		public Vector3 EyePosition { get; set; }
		public Vector3 TargetPosition { get; set; }
		public Vector3 UpVector { get; set; }

		public float NearPlane { get; set; }
		public float FarPlane { get; set; }

		public Camera()
			: this(Vector3.UnitY, Vector3.UnitZ, Vector3.Up, .1f, 100f)
		{
		}

		public Camera(Vector3 eyePosition, Vector3 targetPosition, Vector3 upVector)
			: this(eyePosition, targetPosition, upVector, .1f, 100f)
		{
		}

		public Camera(Vector3 eyePosition, Vector3 targetPosition, Vector3 upVector, float nearPlane, float farPlane)
		{
			EyePosition = eyePosition;
			TargetPosition = targetPosition;
			UpVector = upVector;
			NearPlane = nearPlane;
			FarPlane = farPlane;
		}
	}
}
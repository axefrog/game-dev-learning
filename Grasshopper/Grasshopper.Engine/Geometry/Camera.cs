using System;
using SharpDX;

namespace Grasshopper.Engine.Geometry
{
	public class Camera
	{
		public Vector3 Position { get; set; }
		public Vector3 Target { get; set; }
		public Vector3 Up { get; set; }
		public float FieldOfView { get; set; }
		public float NearPlane { get; set; }
		public float FarPlane { get; set; }

		public Camera(Vector3 position, Vector3 target, Vector3 up, float fieldOfView = (float)Math.PI/3f, float nearPlane = 0.5f, float farPlane = 100f)
		{
			Position = position;
			Target = target;
			Up = up;
			FieldOfView = fieldOfView;
			NearPlane = nearPlane;
			FarPlane = farPlane;
		}

		public Camera(Vector3 position, Vector3 target) : this(position, target, Vector3.Up)
		{
		}

		public Camera() : this(new Vector3(0, 2, -3), Vector3.Zero, Vector3.Up)
		{
		}
	}
}

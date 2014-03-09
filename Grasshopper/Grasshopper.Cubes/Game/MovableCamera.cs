using System.Windows.Forms;
using Grasshopper.Engine;
using Grasshopper.Engine.Geometry;
using SharpDX;

namespace Grasshopper.Cubes.Game
{
	class MovableCamera : Camera
	{
		private long _prev;
		private Quaternion _q;
		public void Update(long elapsedMilliseconds)
		{
			const float moveSpeed = 10f;
			const float turnSpeed = 1f;
			var input = InputManager.Default;

			var isForward = input.IsKeyDown(Keys.W);
			var isBackward = input.IsKeyDown(Keys.S);
			if(isForward || isBackward)
			{
				var v = Position - Target;
				v.Normalize();
				v = v * ((elapsedMilliseconds - _prev) / 1000f) * moveSpeed;
				if(isForward)
				{
					Position -= v;
					Target -= v;
				}
				else
				{
					Position += v;
					Target += v;
				}
			}

			var isLeft = input.IsKeyDown(Keys.A);
			var isRight = input.IsKeyDown(Keys.D);
			if(isLeft || isRight)
			{
				//var n = Vector3.Cross(Position - Target, Up);
				//n.Normalize();
				//Matrix.RotationY(1f).
			}
			_prev = elapsedMilliseconds;
		}
	}
}

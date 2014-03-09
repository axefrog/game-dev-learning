using System;
using System.Linq;
using Grasshopper.Engine.Geometry;
using SharpDX;

namespace Grasshopper.Cubes.Game
{
	class AnimatedCube : WorldPosition
	{
		private static readonly Random Rand = new Random();
		private readonly long[] _secs;
		//private readonly float _pulseScale;

		public AnimatedCube(Vector3 location, float scale, float yaw, float pitch, float roll) : base(location, scale, yaw, pitch, roll)
		{
			_secs = Enumerable.Range(0, 4).Select(n => Rand.NextLong(1000, 10000)).ToArray();
		}

		public override void Update(long elapsedMilliseconds)
		{
			Yaw = (float)(2 * Math.PI * (elapsedMilliseconds % _secs[0]) / _secs[0]);
			Pitch = (float)(2 * Math.PI * (elapsedMilliseconds % _secs[1]) / _secs[1]);
			Roll = (float)(2 * Math.PI * (elapsedMilliseconds % _secs[2]) / _secs[2]);
			//var phase = (float)Math.Sin(2 * Math.PI * (elapsedMilliseconds % _secs[3]) / _secs[3]) / 2 + .5f;
			//var range = _pulseScale * Scale - Scale;
			//var scale = Scale + phase * range;
			//var orientation = Quaternion.RotationYawPitchRoll(Yaw, Pitch, Roll);
			//WorldMatrix = Matrix.AffineTransformation(scale, orientation, Position);

			base.Update(elapsedMilliseconds);
		}
	}
}

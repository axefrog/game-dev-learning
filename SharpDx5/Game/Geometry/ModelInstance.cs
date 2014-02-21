using System;
using System.Linq;
using SharpDX;

namespace SharpDx5.Game.Geometry
{
	public class ModelInstance
	{
		public Vector3 Position { get; set; }
		public float Scale { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public float Roll { get; set; }
		public Matrix WorldMatrix { get; private set; }

		public ModelInstance(long elapsedMilliseconds) : this(Vector3.Zero, 1f, 0, 0, 0)
		{
		}

		public ModelInstance(Vector3 position, float scale, float yaw, float pitch, float roll)
		{
			Position = position;
			Scale = scale;
			Yaw = yaw;
			Pitch = pitch;
			Roll = roll;

			_secs = Enumerable.Range(0, 4).Select(n => _rand.NextLong(100, 10000)).ToArray();
			_pulseScale = _rand.NextFloat(.1f, 4f);
			
			Update(0);
		}

		private static readonly Random _rand = new Random();
		private readonly long[] _secs;
		private readonly float _pulseScale;

		public void Update(long elapsedMilliseconds)
		{
			Yaw = (float)(2*Math.PI*(elapsedMilliseconds%_secs[0])/_secs[0]);
			Pitch = (float)(2*Math.PI*(elapsedMilliseconds%_secs[1])/_secs[1]);
			Roll = (float)(2*Math.PI*(elapsedMilliseconds%_secs[2])/_secs[2]);
			var phase = (float)Math.Sin(2*Math.PI*(elapsedMilliseconds%_secs[3])/_secs[3])/2 + .5f;
			var range = _pulseScale*Scale - Scale;
			var scale = Scale + phase*range;
			var orientation = Quaternion.RotationYawPitchRoll(Yaw, Pitch, Roll);
			WorldMatrix = Matrix.AffineTransformation(scale, orientation, Position);
		}
	}
}
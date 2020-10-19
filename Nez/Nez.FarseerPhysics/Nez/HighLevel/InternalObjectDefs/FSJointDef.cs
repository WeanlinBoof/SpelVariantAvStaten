using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;


namespace Nez.Farseer {
	internal abstract class FSJointDef {
		public Body BodyA;
		public Body BodyB;
		public bool CollideConnected;

		abstract public Joint CreateJoint();
	}


	internal class FSDistanceJointDef : FSJointDef {
		public Vector2 OwnerBodyAnchor;
		public Vector2 OtherBodyAnchor;
		public float Frequency;
		public float DampingRatio;

		public override Joint CreateJoint() {
			DistanceJoint joint = new DistanceJoint(BodyA, BodyB, OwnerBodyAnchor * FSConvert.DisplayToSim,
				OtherBodyAnchor * FSConvert.DisplayToSim) {
				CollideConnected = CollideConnected,
				Frequency = Frequency,
				DampingRatio = DampingRatio
			};
			return joint;
		}
	}


	internal class FSFrictionJointDef : FSJointDef {
		public Vector2 Anchor;
		public float MaxForce;
		public float MaxTorque;

		public override Joint CreateJoint() {
			FrictionJoint joint = new FrictionJoint(BodyA, BodyB, Anchor) {
				CollideConnected = CollideConnected,
				MaxForce = MaxForce,
				MaxTorque = MaxTorque
			};
			return joint;
		}
	}


	internal class FSWeldJointDef : FSJointDef {
		public Vector2 OwnerBodyAnchor;
		public Vector2 OtherBodyAnchor;
		public float DampingRatio;
		public float FrequencyHz;

		public override Joint CreateJoint() {
			WeldJoint joint = new WeldJoint(BodyA, BodyB, OwnerBodyAnchor * FSConvert.DisplayToSim,
				OtherBodyAnchor * FSConvert.DisplayToSim) {
				CollideConnected = CollideConnected,
				DampingRatio = DampingRatio,
				FrequencyHz = FrequencyHz
			};
			return joint;
		}
	}


	internal class FSAngleJointDef : FSJointDef {
		public float MaxImpulse = float.MaxValue;
		public float BiasFactor = 0.2f;
		public float Softness;

		public override Joint CreateJoint() {
			AngleJoint joint = new AngleJoint(BodyA, BodyB) {
				CollideConnected = CollideConnected,
				MaxImpulse = MaxImpulse,
				BiasFactor = BiasFactor,
				Softness = Softness
			};
			return joint;
		}
	}


	internal class FSRevoluteJointDef : FSJointDef {
		public Vector2 OwnerBodyAnchor;
		public Vector2 OtherBodyAnchor;
		public bool LimitEnabled;
		public float LowerLimit;
		public float UpperLimit;
		public bool MotorEnabled;
		public float MotorSpeed;
		public float MaxMotorTorque;
		public float MotorImpulse;

		public override Joint CreateJoint() {
			RevoluteJoint joint = new RevoluteJoint(BodyA, BodyB, OwnerBodyAnchor * FSConvert.DisplayToSim,
				OtherBodyAnchor * FSConvert.DisplayToSim) {
				CollideConnected = CollideConnected,
				LimitEnabled = LimitEnabled,
				LowerLimit = LowerLimit,
				UpperLimit = UpperLimit,
				MotorEnabled = MotorEnabled,
				MotorSpeed = MotorSpeed,
				MaxMotorTorque = MaxMotorTorque,
				MotorImpulse = MotorImpulse
			};
			return joint;
		}
	}


	internal class FSPrismaticJointDef : FSJointDef {
		public Vector2 OwnerBodyAnchor;
		public Vector2 OtherBodyAnchor;
		public Vector2 Axis = Vector2.UnitY;
		public bool LimitEnabled;
		public float LowerLimit;
		public float UpperLimit;
		public bool MotorEnabled;
		public float MotorSpeed = 0.7f;
		public float MaxMotorForce = 2;
		public float MotorImpulse;

		public override Joint CreateJoint() {
			PrismaticJoint joint = new PrismaticJoint(BodyA, BodyB, OwnerBodyAnchor * FSConvert.DisplayToSim,
				OtherBodyAnchor * FSConvert.DisplayToSim) {
				CollideConnected = CollideConnected,
				Axis = Axis,
				LimitEnabled = LimitEnabled,
				LowerLimit = LowerLimit,
				UpperLimit = UpperLimit,
				MotorEnabled = MotorEnabled,
				MotorSpeed = MotorSpeed,
				MaxMotorForce = MaxMotorForce,
				MotorImpulse = MotorImpulse
			};
			return joint;
		}
	}


	internal class FSRopeJointDef : FSJointDef {
		public Vector2 OwnerBodyAnchor;
		public Vector2 OtherBodyAnchor;
		public float MaxLength;

		public override Joint CreateJoint() {
			RopeJoint joint = new RopeJoint(BodyA, BodyB, OwnerBodyAnchor * FSConvert.DisplayToSim,
				OtherBodyAnchor * FSConvert.DisplayToSim) {
				CollideConnected = CollideConnected,
				MaxLength = MaxLength * FSConvert.DisplayToSim
			};
			return joint;
		}
	}


	internal class FSMotorJointDef : FSJointDef {
		public Vector2 LinearOffset;
		public float MaxForce = 1;
		public float MaxTorque = 1;
		public float AngularOffset;

		public override Joint CreateJoint() {
			MotorJoint joint = new MotorJoint(BodyA, BodyB) {
				CollideConnected = CollideConnected,
				LinearOffset = LinearOffset * FSConvert.DisplayToSim,
				MaxForce = MaxForce,
				MaxTorque = MaxTorque,
				AngularOffset = AngularOffset
			};
			return joint;
		}
	}


	internal class FSWheelJointDef : FSJointDef {
		public Vector2 Anchor;
		public Vector2 Axis = Vector2.UnitY;
		public bool MotorEnabled;
		public float MotorSpeed;
		public float MaxMotorTorque;
		public float Frequency = 2;
		public float DampingRatio = 0.7f;

		public override Joint CreateJoint() {
			WheelJoint joint = new WheelJoint(BodyA, BodyB, Anchor * FSConvert.DisplayToSim, Axis) {
				CollideConnected = CollideConnected,
				Axis = Axis,
				MotorEnabled = MotorEnabled,
				MotorSpeed = MotorSpeed,
				MaxMotorTorque = MaxMotorTorque,
				Frequency = Frequency,
				DampingRatio = DampingRatio
			};
			return joint;
		}
	}


	internal class FSPulleyJointDef : FSJointDef {
		public Vector2 OwnerBodyAnchor;
		public Vector2 OtherBodyAnchor;
		public Vector2 OwnerBodyGroundAnchor;
		public Vector2 OtherBodyGroundAnchor;
		public float Ratio;

		public override Joint CreateJoint() {
			PulleyJoint joint = new PulleyJoint(BodyA, BodyB, OwnerBodyAnchor * FSConvert.DisplayToSim,
				OtherBodyGroundAnchor * FSConvert.DisplayToSim,
				OwnerBodyGroundAnchor * FSConvert.DisplayToSim, OtherBodyGroundAnchor * FSConvert.DisplayToSim, Ratio) {
				CollideConnected = CollideConnected
			};
			return joint;
		}
	}


	internal class FSGearJointDef : FSJointDef {
		public Joint OwnerJoint;
		public Joint OtherJoint;
		public float Ratio = 1;

		public override Joint CreateJoint() {
			GearJoint joint = new GearJoint(BodyA, BodyB, OwnerJoint, OtherJoint, Ratio) {
				CollideConnected = CollideConnected
			};
			return joint;
		}
	}


	internal class FSMouseJointDef : FSJointDef {
		public Vector2 WorldAnchor;
		public float MaxForce;
		public float Frequency = 5;
		public float DampingRatio = 0.7f;

		public override Joint CreateJoint() {
			FixedMouseJoint joint = new FixedMouseJoint(BodyA, WorldAnchor * FSConvert.DisplayToSim) {
				CollideConnected = CollideConnected
			};

			// conditionally set the maxForce
			if (MaxForce > 0) {
				joint.MaxForce = MaxForce;
			}

			joint.Frequency = Frequency;
			joint.DampingRatio = DampingRatio;
			return joint;
		}
	}
}
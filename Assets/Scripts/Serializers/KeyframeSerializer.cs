using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Serializers
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class KeyframeSerializer
    {
        public static void WriteKeyframe(this NetworkWriter writer, Keyframe keyFrame)
        {
            writer.Write(keyFrame.time);
            writer.Write(keyFrame.value);
            writer.Write(keyFrame.inTangent);
            writer.Write(keyFrame.outTangent);
            writer.Write(keyFrame.inWeight);
            writer.Write(keyFrame.outWeight);
            writer.Write((int)keyFrame.weightedMode);
        }

        public static Keyframe ReadKeyframe(this NetworkReader reader)
        {
            float time = reader.Read<float>();
            float value = reader.Read<float>();
            float inTangent = reader.Read<float>();
            float outTangent = reader.Read<float>();
            float inWeight = reader.Read<float>();
            float outWeight = reader.Read<float>();
            return new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
        }
    }
}
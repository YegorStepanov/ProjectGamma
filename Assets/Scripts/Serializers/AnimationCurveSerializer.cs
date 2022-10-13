using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Serializers
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class AnimationCurveSerializer
    {
        public static void WriteAnimationCurve(this NetworkWriter writer, AnimationCurve curve)
        {
            writer.Write((int)curve.preWrapMode);
            writer.Write((int)curve.postWrapMode);
            writer.Write(curve.keys.Length);
            foreach (Keyframe key in curve.keys)
            {
                writer.Write(key);
            }
        }

        public static AnimationCurve ReadAnimationCurve(this NetworkReader reader)
        {
            var preWrapMode = (WrapMode)reader.Read<int>();
            var postWrapMode = (WrapMode)reader.Read<int>();
            int length = reader.Read<int>();
            var keys = new Keyframe[length];
            for (int i = 0; i < length; i++)
            {
                keys[i] = reader.Read<Keyframe>();
            }

            var curve = new AnimationCurve(keys)
            {
                preWrapMode = preWrapMode,
                postWrapMode = postWrapMode,
            };
            return curve;
        }
    }
}
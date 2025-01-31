using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DiffusePartHook : AnimationHook
    {
        public uint Part { get; private set; }
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

        public override void Unpack(BinaryReader reader, bool isToD = true)
        {
            base.Unpack(reader);

            Part    = reader.ReadUInt32();
            Start   = reader.ReadSingle();
            End     = reader.ReadSingle();
            Time    = reader.ReadSingle();
        }
    }
}

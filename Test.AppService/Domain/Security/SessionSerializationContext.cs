using System;
using System.Buffers;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security
{
    public static class SessionSerializationContext
    {
        private const int GuidSize = 2 * sizeof(long);
        private const int Size = 2 * sizeof(long) + GuidSize ; 

    

        public static string Serialize(UserSession session)
        {
            byte[] buff = ArrayPool<byte>.Shared.Rent(Size);

            try
            {
                using BinaryWriter writer = new(new MemoryStream(buff));
                writer.Write(session.CreatedAt.ToBinary());
                writer.Write(session.ExpiredAt.ToBinary());
                writer.Write(session.UserGuid.ToByteArray());
                writer.Flush();
                return Convert.ToBase64String(buff);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buff);
            }
        }

        public static UserSession Deserialize(string state)
        {
            byte[] buff = ArrayPool<byte>.Shared.Rent(Size);
            UserSession session = new();

            try
            {
                if (Convert.TryFromBase64String(state, buff, out int bytes) && bytes == Size)
                {
                    using BinaryReader reader = new (new MemoryStream(buff));
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    session.CreatedAt = DateTime.FromBinary(reader.ReadInt64());
                    session.ExpiredAt = DateTime.FromBinary(reader.ReadInt64());
                    session.Guid = new Guid( reader.ReadBytes(GuidSize));
                }
            }
            catch
            {
                ArrayPool<byte>.Shared.Return(buff);
            }
            return session;
        }
    }
}


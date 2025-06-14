﻿using System;
using System.Buffers;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security
{
    public class SessionSerializationContext
    {
        private readonly static int SIZE = 2 * sizeof(long) + sizeof(int); 

        public SessionSerializationContext()
        {
        }

        public static string Serialize(UserSession session)
        {
            byte[] buff = ArrayPool<byte>.Shared.Rent(SIZE);

            try
            {
                using BinaryWriter writer = new(new MemoryStream(buff));
                writer.Write(session.CreatedAt.ToBinary());
                writer.Write(session.ExpiredAt.ToBinary());
                writer.Write(session.UserId);
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
            byte[] buff = ArrayPool<byte>.Shared.Rent(SIZE);
            UserSession session = new();

            try
            {
                if (Convert.TryFromBase64String(state, buff, out int bytes) && bytes == SIZE)
                {
                    using BinaryReader reader = new (new MemoryStream(buff));
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    session.CreatedAt = DateTime.FromBinary(reader.ReadInt64());
                    session.ExpiredAt = DateTime.FromBinary(reader.ReadInt64());
                    session.UserId = reader.ReadInt32();
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


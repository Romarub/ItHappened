﻿using System;

namespace ItHappened.Domain
{
    public class User
    {
        public User(Guid guid, string name, DateTimeOffset dateTimeOffset)
        {
            Guid = guid;
            Name = name;
            DateTimeOffset = dateTimeOffset;
        }

        public Guid Guid { get; }
        public string Name { get; }
        public DateTimeOffset DateTimeOffset { get; }
    }
}
using System;

namespace DialogueSystem.Utils
{
    public static class IDGenerator
    {
        public static int NewId()
        {
            var newId = Guid.NewGuid().GetHashCode();
            while (newId == -1)
                newId = Guid.NewGuid().GetHashCode();

            return newId;
        }
    }
}
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commons.Models
{
    /// <summary>
    /// user实体
    /// </summary>
    public class UserEntity
    {
        [BsonId]
        public Int64 Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserEntity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            UserEntity item = (UserEntity)obj;

            return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(UserEntity left, UserEntity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(UserEntity left, UserEntity right)
        {
            return !(left == right);
        }
    }
}

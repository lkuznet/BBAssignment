using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Domain
{
    public class Entity : IEquatable<Entity>
    {
        public int Id { get; private set; }
        public Entity()
        {
            Id = new int();
        }

        #region Identity Management
        public bool Equals(Entity other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}

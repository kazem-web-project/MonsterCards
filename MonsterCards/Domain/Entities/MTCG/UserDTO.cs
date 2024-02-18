using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Entities.MTCG
{
    [Serializable]
    public class UserDTO
    {
        public string  Name { get; set; }
        public string  Bio { get; set; }
        public string  Image { get; set; }

        public UserDTO(string name, string bio, string image)
        {
            this.Name = name;
            this.Bio = bio;
            this.Image = image;
        }

        public UserDTO()
        {
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }

}

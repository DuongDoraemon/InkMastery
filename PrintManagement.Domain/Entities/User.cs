using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public GenderEnum? Gender { get; set; } = GenderEnum.UnKnown;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public Guid? TeamId { get; set; }
        public virtual Team? Team { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Permissions>? Permissions { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}

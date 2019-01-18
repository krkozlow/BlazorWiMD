using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.IdentityAccess.Domain.Model
{
    public class User : Entity
    {
        public bool IsConnected { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public IList<Group> Groups { get; set; }

        public IReadOnlyList<string> GetPublicGroups()
        {
            return Groups.Where(x => x.IsPublic).Select(x => x.Name).ToList();
        }

        public void AddToGroup(string groupName)
        {
            Groups.Add(new Group { Name = groupName, IsPublic = true });
        }

        public void RemoveFromGroup(string groupName)
        {
            var toRemove = Groups.FirstOrDefault(x => x.Name == groupName);

            if (toRemove == null)
            {
                throw new ArgumentException($"User does not belong to group {groupName}");
            }

            Groups.Remove(toRemove);
        }

        public void Connect()
        {
            IsConnected = true;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }
    }
}

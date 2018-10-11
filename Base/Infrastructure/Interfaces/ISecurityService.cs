using System;
namespace Infrastructure.Interfaces
{
    public interface ISecurityService
    {
        bool IsAuthed(out Guid userId);
    }
}

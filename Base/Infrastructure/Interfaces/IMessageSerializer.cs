using System;
namespace Infrastructure.Interfaces
{
    public interface IMessageSerializer
    {
        string Serialize(object obj);
    }
}

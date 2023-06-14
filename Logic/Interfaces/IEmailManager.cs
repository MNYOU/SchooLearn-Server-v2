namespace Logic.Interfaces;

public interface IEmailManager
{
    void SendMessageAsync(string email, string subject, string message);
}
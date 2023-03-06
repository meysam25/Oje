namespace Oje.Infrastructure.Interfac
{
    public interface ISignatureEntity
    {
        string Signature { get; set; }
        void FilledSignature();
        bool IsSignature();
        string GetSignatureChanges();
    }
}

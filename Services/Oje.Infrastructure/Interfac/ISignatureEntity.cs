namespace Oje.Infrastructure.Interfac
{
    public interface ISignatureEntity
    {
        byte[] Signature { get; set; }
        void FilledSignature();
        void UpdateSignature();
        bool IsSignature();
        string GetSignatureChanges();
    }
}

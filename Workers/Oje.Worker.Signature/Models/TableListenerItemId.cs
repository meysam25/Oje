namespace Oje.Worker.Signature.Models
{
    public class TableListenerItemId
    {
        public TableListenerItemId()
        {
            CreateDate = DateTime.Now;
        }
        public object Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

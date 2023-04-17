namespace Oje.Section.Tender.Models.View
{
    public class TenderProposalFormJsonConfigFileCreateUpdateVM
    {
        public int? id { get; set; }
        public int? fid { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public bool? isRequired { get; set; }
        public bool? isActive { get; set; }
    }
}

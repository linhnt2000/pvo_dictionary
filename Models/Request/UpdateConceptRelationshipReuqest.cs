namespace thu6_pvo_dictionary.Models.Request
{
    public class UpdateConceptRelationshipReuqest
    {
        public int conceptId { get; set; }
        public int parentId { get; set; }
        public int conceptLinkId { get; set; }
        public bool isForced { get; set; }
    }
}

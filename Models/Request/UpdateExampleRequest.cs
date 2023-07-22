namespace thu6_pvo_dictionary.Models.Request
{
    public class UpdateExampleRequest
    {
        public int ExampleId { get; set; }
        public string Detail { get; set; }
        public string DetailHtml { get; set; }
        public string Note { get; set; }
        public int ToneId { get; set; }
        public int ModeId { get; set; }
        public int RegisterId { get; set; }
        public int NuanceId { get; set; }
        public int DialectId { get; set; }
    }
}
